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
    public class configCardsController : ApiController
    {
        private HajjCoinsModel db = new HajjCoinsModel();

        // GET: api/configCards
        public IQueryable<configCards> GetconfigCards()
        {
            return db.configCards;
        }

        // GET: api/configCards/5
        [ResponseType(typeof(configCards))]
        public IHttpActionResult GetconfigCards(Guid id)
        {
            configCards configCards = db.configCards.Find(id);
            if (configCards == null)
            {
                return NotFound();
            }

            return Ok(configCards);
        }

        // PUT: api/configCards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutconfigCards(Guid id, configCards configCards)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != configCards.CardID)
            {
                return BadRequest();
            }

            db.Entry(configCards).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!configCardsExists(id))
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

        // POST: api/configCards
        [ResponseType(typeof(configCards))]
        public IHttpActionResult PostconfigCards(configCards configCards)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.configCards.Add(configCards);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (configCardsExists(configCards.CardID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = configCards.CardID }, configCards);
        }

        // DELETE: api/configCards/5
        [ResponseType(typeof(configCards))]
        public IHttpActionResult DeleteconfigCards(Guid id)
        {
            configCards configCards = db.configCards.Find(id);
            if (configCards == null)
            {
                return NotFound();
            }

            db.configCards.Remove(configCards);
            db.SaveChanges();

            return Ok(configCards);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool configCardsExists(Guid id)
        {
            return db.configCards.Count(e => e.CardID == id) > 0;
        }
    }
}