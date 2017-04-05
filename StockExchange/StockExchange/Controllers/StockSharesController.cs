using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StockExchange.Models;
using Microsoft.AspNet.Identity;

namespace StockExchange.Controllers
{
    public class StockSharesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StockShares
        public async Task<ActionResult> Index()
        {
            string userid = User.Identity.GetUserId();
            return View(await db.StockShares.Where(u => u.Users.Id == userid).ToListAsync());
        }

        // GET: StockShares/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockShares stockShares = await db.StockShares.FindAsync(id);
            if (stockShares == null)
            {
                return HttpNotFound();
            }
            return View(stockShares);
        }

        // GET: StockShares/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StockShares/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Share")] StockShares stockShares)
        {
            if (ModelState.IsValid)
            {
                stockShares.Id = Guid.NewGuid();
                db.StockShares.Add(stockShares);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(stockShares);
        }

        // GET: StockShares/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockShares stockShares = await db.StockShares.FindAsync(id);
            if (stockShares == null)
            {
                return HttpNotFound();
            }
            return View(stockShares);
        }

        // POST: StockShares/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Share")] StockShares stockShares)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stockShares).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(stockShares);
        }

        // GET: StockShares/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockShares stockShares = await db.StockShares.FindAsync(id);
            if (stockShares == null)
            {
                return HttpNotFound();
            }
            return View(stockShares);
        }

        // POST: StockShares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            StockShares stockShares = await db.StockShares.FindAsync(id);
            db.StockShares.Remove(stockShares);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
