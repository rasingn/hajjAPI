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
            configCards.CardID = Guid.NewGuid();
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
        public IHttpActionResult VerifyTransaction(string cardQR, int Total)
        {
            var cQR = Guid.Parse(cardQR);
            //return class contains password or not found if there is no password
            Messages msg = new Messages();
            using (db = new HajjCoinsModel())
            {
                configCards configCards = new configCards();

                var card = db.configCards.Where(a => a.CardID == cQR).ToList();
                if (card.Count > 0)
                {
                    var coin = db.configCards.Where(m => m.NoOfCoins >= Total).ToList();

                    //Check no of coins
                    if (coin.Count > 0)
                    {

                        msg.message = "Amount of " + Total + " hajjCoins will be debited from your account.";
                        msg.status = true;

                        return Ok(msg);

                    }
                    else
                    {
                        msg.message = "Your hajjCoins balance is not enough";
                        msg.status = false;

                        return Ok(msg);
                    }
                }
                else
                {

                    msg.message = "Your hajjCoins card Not Found";
                    msg.status = false;

                    return Ok(msg);
                }
            }







        }

        [HttpPost]
        [Route("api/hajjCoin/performTransaction/{cardQR}/{Total}/{SupplierID}/{password}")]
        public IHttpActionResult PerformTransaction(string cardQR, int Total, int SupplierID, int password)
        {
            var cQR = Guid.Parse(cardQR);
            //return class contains password or not found if there is no password
            Messages msg = new Messages();
            using (db = new HajjCoinsModel())
            {
                configCards configCards = new configCards();
                var CoinsCount = db.configCards.Where(a => a.CardID == cQR).Select(a => a.NoOfCoins).FirstOrDefault();
                if (CoinsCount >= Total)
                {




                    //var coinsList = db.Coins.Where(a => a.CardID == cQR).ToList();

                    //var validCoins = (from coinsList in db.Coins

                    //                  join usedList in db.TransactionsCoinsSupplier
                    //                  on coinsList.CoinID equals usedList.CoinsID
                    //                  where db.TransactionsCoinsSupplier.Contains(usedList.CoinsID)
                    //                  select new { coinsList.CoinID }).ToList();
                    var trans_coinList = db.TransactionsCoinsSupplier.Select(x=>x.CoinsID).ToArray();
                    var validCoins = db.Coins.Where(p=> !trans_coinList.Contains(p.CoinID)).ToList();


                    if (validCoins.Count > 0)
                    {
                        var cardCoin = db.configCards.Where(a => a.CardID == cQR).FirstOrDefault();
                        db.Entry(cardCoin).State = EntityState.Modified;
                        cardCoin.NoOfCoins -= Total;
                        db.SaveChanges();

                        Transaction tr = new Transaction();
                        tr.TotalCoinsUsed = Total;
                        tr.TransactionDate = System.DateTime.Now;
                        tr.SupplyerID = SupplierID;
                        db.Transaction.Add(tr);
                        db.SaveChanges();

                        for (int i = 0; i < Total; i++)
                        {
                            TransactionsCoinsSupplier coin = new TransactionsCoinsSupplier();
                            coin.CoinsID = validCoins[i].CoinID;
                            coin.TransactionID = tr.TransactionID;
                            db.TransactionsCoinsSupplier.Add(coin);




                        }

                        msg.message = Total + " hajjCoins successfully received";
                        msg.status = true;

                        return Ok(msg);





                    }
                    else
                    {

                        msg.message = "Your hajjCoins balance is not enough";
                        msg.status = false;

                        return Ok(msg);
                    }



                }
                else
                {
                    msg.message = "Your hajjCoins balance is not enough";
                    msg.status = false;

                    return Ok(msg);
                }
            }







        }



        #endregion


    }
}

