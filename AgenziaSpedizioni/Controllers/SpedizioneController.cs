using AgenziaSpedizioni.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spedizioni.Data;
using System.Security.Claims;

namespace AgenziaSpedizioni.Controllers
{
    public class SpedizioneController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SpedizioneController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(int? id)
        {
            if (!User.Identity.IsAuthenticated || id == -1)
            {
                return RedirectToAction("Login", "Login");
            }
            IEnumerable<Spedizione> spedizioni = _db.Spedizioni.Where(spedizione => spedizione.UserId == id).OrderBy(spedizione => spedizione.DataSpedizione).Reverse();
            return View(spedizioni);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Spedizione spedizione)
        {
            ModelState.Remove("DettagliSpedizioni");
            if (ModelState.IsValid)
            {
                spedizione.UserId = FindId();

                try
                {
                    _db.Add(spedizione);
                    _db.SaveChanges();
                    TempData["success"] = "Spedizione aggiunta con successo!";
                    return RedirectToAction("Index", new { id = spedizione.UserId });
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                }
            }
            return View();
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            Spedizione? spedizione = await _db.Spedizioni.FindAsync(id);

            if (spedizione == null)
                return NotFound();

            ViewBag.spedizione = spedizione;

            IEnumerable<DettagliSpedizione> dettagliSpedizioni = _db.DettagliSpedizioni.Where(ds => ds.IdSpedizione == spedizione.Id).OrderBy(ds => ds.dataUpdate).Reverse();
            return View(dettagliSpedizioni);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateDetails(Guid? id)
        {
            TempData["idSpedizione"] = id;
            return View();
        }

        [HttpPost, ActionName("CreateDetails")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateDetailsPOST(DettagliSpedizione dettagliSpedizione)
        {
            try
            {
                _db.DettagliSpedizioni.Add(dettagliSpedizione);
                _db.SaveChanges();
                TempData["success"] = "Aggiornamento aggiunto con successo!";
                return RedirectToAction("Details", new { id = dettagliSpedizione.IdSpedizione });
            }
            catch (Exception ex)
            {
                TempData["error"] = "Qualcosa è andato storto.";
                return View();
            }
        }

        public int FindId()
        {
            var idClaim = User.FindFirst(ClaimTypes.UserData);

            if (idClaim == null)
            {
                TempData["error"] = "Qualcosa è andato storto.";
            }

            int id = Convert.ToInt16(idClaim.Value);
            return id;
        }
    }
}
