using CrimeManagement.Data;
using CrimeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

namespace CrimeManagement.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dataContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        /*public async Task<IActionResult> Index(long suspectId)
        {
            var combinedModels = await _dataContext.Suspects
                .Include(s => s.CriminalRecords)
                .Select(s => new Combined
                {
                    Suspect = s,
                    CriminalRecord = s.CriminalRecords.FirstOrDefault()
                })
                .ToListAsync();

            combinedModels.RemoveAll(cm => cm.CriminalRecord == null);

            string suspectIdString = suspectId.ToString();

            if (suspectIdString.Length == 13)
            {
                combinedModels = combinedModels.Where(n => n.Suspect.SuspectId.ToString() == suspectIdString).ToList();
            }
            else
            {
                // Handle the case where the suspectId is not 13 digits
                ModelState.AddModelError("suspectId", "Suspect ID must be exactly 13 digits long.");
            }

            return View(combinedModels);
        }*/

        

        public async Task<IActionResult> Index(long? suspectId)
        {
            // Create an empty list initially
            var combinedModels = new List<Combined>();

            

            if (suspectId.HasValue)
            {
                combinedModels = await _dataContext.Suspects
                    .Include(s => s.CriminalRecords)
                    .Where(s => s.SuspectId == suspectId)
                    .Select(s => new Combined
                    {
                        Suspect = s,
                        CriminalRecords = s.CriminalRecords.ToList()
                    })
                    .ToListAsync();

                
            }

            return View(combinedModels);
        }

        public IActionResult Dashboard()
        {
            var suspectsWithOffenses = _dataContext.Suspects
                .Include(s => s.CriminalRecords)
                .Select(suspect => new Combined
                {
                    Suspect = suspect, // Set the Suspect property

                    CriminalRecords = suspect.CriminalRecords.ToList(),
                    TotalOffenses = suspect.CriminalRecords.Count()
                })
                .ToList();

            return View(suspectsWithOffenses);
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
