using CrimeManagement.Data;
using CrimeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrimeManagement.Controllers
{
    [Authorize(Roles ="Police Officer")]
    public class SuspectController : Controller
    {
        private readonly ApplicationDbContext _dataContext;

        public SuspectController(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }
        

        public async Task<IActionResult> Upsert(int? suspectNo)
        {
            if(suspectNo == null || suspectNo == 0)
            {
                return View();
            }
            else
            {
                Suspect? suspect = await _dataContext.Suspects.FindAsync(suspectNo);
                return View(suspect);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(Suspect record)
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var error = ModelState[key].Errors.FirstOrDefault();
                    if (error != null)
                    {
                        var errorMessage = error.ErrorMessage;
                        Console.WriteLine($"Validation Error for {key}: {errorMessage}");
                    }
                }
                // ...
            }
            if (ModelState.IsValid)
            {
                await _dataContext.AddAsync(record);
                await _dataContext.SaveChangesAsync();

                TempData["success"] = "New Suspect created successfully";
                return RedirectToAction("New", "CriminalRecord", new { suspectNo = record.SuspectNo });

            }

            return View(record);
        }

        

        [HttpPost]
        public async Task<IActionResult> Edit(Suspect suspect)
        {

            if (ModelState.IsValid)
            {
                _dataContext.Update(suspect);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Suspect updated successfully";
                //return RedirectToAction("Index");
                return RedirectToAction(nameof(HomeController.Index), "Home", new { suspectId = suspect.SuspectId });

            }

            return View(suspect);
        }

    }
}
