﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using dotnet_CRUD.Models;

namespace dotnet_CRUD.Controllers
{
    public class OrdersController : BaseController
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Customers).Include(o => o.Employees).Include(o => o.Shippers);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName");
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName");
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName");
            return View();
        }

        // POST: Orders/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Orders orders)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Orders.Add(orders);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", orders.CustomerID);
                ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
                ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
                return View(orders);
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                var exceptionMessage = FetchValidationErrorMessage(dbEntityValidationException);
                ModelState.AddModelError("", exceptionMessage);
                return View(orders);
            }
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", orders.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
            return View(orders);
        }

        // POST: Orders/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Orders orders)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(orders).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", orders.CustomerID);
                ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
                ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
                return View(orders);
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                var exceptionMessage = FetchValidationErrorMessage(dbEntityValidationException);
                ModelState.AddModelError("", exceptionMessage);

                ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", orders.CustomerID);
                ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
                ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
                return View(orders);
            }
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orders orders = db.Orders.Find(id);
            var orderDetails = db.Order_Details.Where(od => od.OrderID.Equals(id));

            //有FK關聯，先刪除order detail
            db.Order_Details.RemoveRange(orderDetails);
            db.Orders.Remove(orders);
            db.SaveChanges();
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
