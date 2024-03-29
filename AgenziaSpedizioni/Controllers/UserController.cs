﻿using AgenziaSpedizioni.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spedizioni.Data;

namespace AgenziaSpedizioni.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            IEnumerable<User> objList = _db.Users;
            return View(objList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            string hashPassword = PasswordManager.HashPassword(user.Password);
            user.Password = hashPassword;
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Users.Add(user);
                    _db.SaveChanges();
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
                {
                    ModelState.AddModelError("Username", "Username già in uso");
                    return View();
                }
                TempData["success"] = "Registrazione avvenuta con successo!";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Qualcosa è andato storto!";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            var user = _db.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Update(user);
                _db.SaveChanges();
                TempData["success"] = "Modifiche avvenute con successo";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Errore nella modifica delle informazioni";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var user = _db.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePOST(int? id)
        {
            var user = _db.Users.Find(id);

            if (user == null)
            {
                TempData["error"] = "Errore nella rimozione dell'utente";
                return NotFound();
            }

            _db.Users.Remove(user);
            _db.SaveChanges();
            TempData["success"] = "Utente rimosso con successo";
            return RedirectToAction("Index");
        }
    }
}
