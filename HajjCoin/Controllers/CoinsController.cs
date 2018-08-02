using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HajjCoin.Models;

namespace HajjCoin.Controllers
{
    public class CoinsController : ApiController
    {
        private HajjCoinsModel db = new HajjCoinsModel();

        // GET: api/Coins
        public IQueryable<Coins> GetCoins()
        {
            return db.Coins;
        }

        // GET: api/Coins/5
        [ResponseType(typeof(Coins))]
        public IHttpActionResult GetCoins(int id)
        {
            Coins coins = db.Coins.Find(id);
            if (coins == null)
            {
                return NotFound();
            }

            return Ok(coins);
        }

        // PUT: api/Coins/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCoins(int id, Coins coins)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != coins.CoinID)
            {
                return BadRequest();
            }

            db.Entry(coins).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoinsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Coins
        [ResponseType(typeof(Coins))]
        public IHttpActionResult PostCoins(Coins coins)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Coins.Add(coins);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CoinsExists(coins.CoinID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = coins.CoinID }, coins);
        }

        // DELETE: api/Coins/5
        [ResponseType(typeof(Coins))]
        public IHttpActionResult DeleteCoins(int id)
        {
            Coins coins = db.Coins.Find(id);
            if (coins == null)
            {
                return NotFound();
            }

            db.Coins.Remove(coins);
            db.SaveChanges();

            return Ok(coins);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CoinsExists(int id)
        {
            return db.Coins.Count(e => e.CoinID == id) > 0;
        }





        #region My Code
        [HttpPost]
        [Route("api/hajjCoin/AddCard")]
        public IHttpActionResult AddCard()
        {
            configCards configCards = new configCards();
            configCards.CardID =  Guid.NewGuid();
            configCards.HolderID = 2;
            configCards.NoOfCoins = 50;
            configCards.ActionDate = System.DateTime.Now;
            configCards.BranchID = 4;
            db.configCards.Add(configCards);

            db.SaveChanges();
            return Ok(configCards);


        }


        [HttpPost]
        [Route("api/hajjCoin/VerifyTransaction/{cardQR}/{Total}")]///{SupplierID}")]
        public IHttpActionResult VerifyTransaction(Guid cardQR, int Total)
        {
            //return class contains password or not found if there is no password
            Messages msg = new Messages();
            using (db = new HajjCoinsModel())
            {
                configCards configCards = new configCards();
                var card = db.configCards.Where(a => a.CardID == cardQR);
                if (card != null)
                {
                    var coin = db.configCards.Where(m => m.NoOfCoins >= Total);

                    //Check no of coins
                    if (coin != null)
                    {

                        msg.message = "Verified";
                        msg.status = true;

                        return Ok(msg);

                    }
                    else
                    {
                        msg.message = "Coins are not enough";
                        msg.status = false;

                        return Ok(msg);
                    }
                }
                else
                {

                    msg.message = "Card Not Found";
                    msg.status = false;

                    return Ok(msg);
                }
            }







        }

        [HttpPost]
        [Route("api/hajjCoin/performTransaction/{cardQR}/{Total}/{SupplierID}")]
        public IHttpActionResult PerformTransaction(Guid cardQR, int Total, int SupplierID)
        {
            //return class contains password or not found if there is no password
            Messages msg = new Messages();
            using (db = new HajjCoinsModel())
            {
                configCards configCards = new configCards();
                var card = db.configCards.Where(a => a.CardID == cardQR);
                if (card != null)
                {
                    var coin = db.configCards.Where(m => m.NoOfCoins >= Total);

                    //Check no of coins
                    if (coin != null)
                    {


                        var r = db.Coins.Where(a => a.CardID == cardQR).ToList();

                        if (r != null)
                        {
                            //Option 1
                            /* idea from: https://stackoverflow.com/questions/22907820/lambda-expression-join-multiple-tables-with-select-and-where-clause */

                            ////return coins which is not used in transaction
                            //var x = db.Coins.Join(db.Coins, u => u.CoinID, t => t.CoinID,
                            //(u, t) => new { u, t }).
                            //Join(db.TransactionsCoinsSupplier, tc => tc.u.CoinID, to => to.CoinsID,
                            //(tc, to) => new { tc, to })
                            //.Where(m => m.tc.u.CoinID != 1)
                            //.Select(m => new Coins
                            //{
                            //    CoinID = m.tc.u.CoinID
                            //}).FirstOrDefault();

                            //Option 2 
                            /*idea from: http://www.tutorialsteacher.com/linq/linq-joining-operator-join */
                            var innerJoin = db.Coins.Join(// outer sequence 
                      db.TransactionsCoinsSupplier,  // inner sequence 
                      c => c.CoinID,    // outerKeySelector
                      t => t.CoinsID,  // innerKeySelector
                      (c, t) => new  // result selector
                      {
                          Coin = c.CoinID
                      }).FirstOrDefault();

                            Transaction tr = new Transaction();
                            tr.TotalCoinsUsed = Total;
                            tr.TransactionDate = System.DateTime.Now;
                            tr.SupplyerID = SupplierID;
                            db.Transaction.Add(tr);
                            db.SaveChanges();

                            //for (int i = 0; i < Total; i++)
                            //{
                            //    TransactionsCoinsSupplier tcs = new TransactionsCoinsSupplier();
                            //    tcs.Coins.Coin =
                            //}
                            //TransactionsCoinsSupplier tcs = new TransactionsCoinsSupplier();
                            //tcs.TransactionID = tr.TransactionID;
                            //tcs.CoinsID = x.CoinID;

                        }








                        msg.message = "Verified";
                        msg.status = true;

                        return Ok(msg);

                    }
                    else
                    {
                        msg.message = "Coins are not enough";
                        msg.status = false;

                        return Ok(msg);
                    }
                }
                else
                {

                    msg.message = "Card Not Found";
                    msg.status = false;

                    return Ok(msg);
                }
            }
        }







    }



    #endregion


}

