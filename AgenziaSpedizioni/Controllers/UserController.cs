using AgenziaSpedizioni.Models;
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
                _db.Users.Add(user);
                _db.SaveChanges();
                TempData["success"] = "Registrazione avvenuta con successo!";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Qualcosa è andato storto!";
            return View();
        }

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
