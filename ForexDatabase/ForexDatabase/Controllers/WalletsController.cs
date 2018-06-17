using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using ForexDatabase.DAL;
using Model;

namespace ForexDatabase.Controllers
{
    public class WalletsController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/Wallets
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IQueryable<Wallet> GetWallets()
        {
            return db.Wallets;
        }

        // GET: api/Wallets/5
        [ResponseType(typeof(IQueryable<Wallet>))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetWallet(int id)
        {
            IQueryable<Wallet> wallets = db.Wallets;
            IQueryable<Wallet> walletsOfUser = wallets.Where(w => w.UserId.Equals(id)); 
            if (walletsOfUser == null)
            {
                return NotFound();
            }

            return Ok(walletsOfUser);
        }
        

        // PUT: api/Wallets/5
        [ResponseType(typeof(void))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPut]
        public IHttpActionResult PutWallet(int id, Wallet wallet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != wallet.Id)
            {
                return BadRequest();
            }

            db.Entry(wallet).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WalletExists(id))
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

        // POST: api/Wallets
        [ResponseType(typeof(Wallet))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        public IHttpActionResult PostWallet(Wallet wallet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Wallets.Add(wallet);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = wallet.Id }, wallet);
        }

        // DELETE: api/Wallets/5
        [ResponseType(typeof(Wallet))]
        public IHttpActionResult DeleteWallet(int id)
        {
            Wallet wallet = db.Wallets.Find(id);
            if (wallet == null)
            {
                return NotFound();
            }

            db.Wallets.Remove(wallet);
            db.SaveChanges();

            return Ok(wallet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WalletExists(int id)
        {
            return db.Wallets.Count(e => e.Id == id) > 0;
        }
    }
}