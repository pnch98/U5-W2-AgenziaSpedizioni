using AgenziaSpedizioni.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spedizioni.Data;
using System.Diagnostics;

namespace AgenziaSpedizioni.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Search(Search datiSearch)
        {
            if (datiSearch == null)
            {
                TempData["error"] = "Dati inseriti non validi";
                return View("Index");
            }

            var userDb = _db.Users.FirstOrDefault(u => u.Identificativo == datiSearch.Identificativo);

            if (userDb == null)
            {
                TempData["error"] = "Utente non trovato";
                return View("Index");
            }

            var spedizioneDb = _db.Spedizioni.FirstOrDefault(sp => sp.Id == datiSearch.IdSpedizione);

            if (spedizioneDb == null)
            {
                TempData["error"] = "Spedizione non trovata";
                return View("Index");
            }

            if (spedizioneDb.UserId != userDb.Id)
            {
                TempData["messageError"] = "Il codice spedizione inserito non è associato all'utente!";
                return View("Index");
            }

            return RedirectToAction("Details", "Spedizione", new { id = datiSearch.IdSpedizione });
        }

        public async Task<IActionResult> FetchOggi()
        {
            try
            {
                var spedizioni = await _db.Spedizioni.Where(s => s.DataConsegnaPrevista == DateOnly.FromDateTime(DateTime.Today)).ToListAsync();
                return Json(spedizioni);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero delle spedizioni di oggi");
                return StatusCode(500, "Errore interno del server");
            }

        }
        public async Task<IActionResult> FetchInConsegna()
        {
            try
            {
                var spedizioniInConsegna =
                    await _db.Spedizioni.Include(s => s.DettagliSpedizioni)
                    .Where(s => s.DettagliSpedizioni.Any(ds => ds.Stato == "In consegna")).CountAsync();
                return Json(spedizioniInConsegna);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero delle spedizioni in consegna");
                return StatusCode(500, "Errore interno del server");
            }
        }

        // Conoscere il numero totale delle spedizioni memorizzate raggruppate per luogo di destinazione
        public async Task<IActionResult> FetchPerDestinazione()
        {
            try
            {
                var listaPerDestinazione =
                    await _db.Spedizioni
                    .GroupBy(s => s.CittaDestinataria)
                    .Select(g => new { Destinazione = g.Key, NumeroSpedizioni = g.Count() })
                    .ToListAsync();
                return Json(listaPerDestinazione);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero delle spedizioni raggruppate per destinazione");
                return StatusCode(500, "Errore interno del server");
            }
        }

    }
}
