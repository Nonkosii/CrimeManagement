using CrimeManagement.Data;
using CrimeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace CrimeManagement.Controllers
{
    [Authorize]
    public class CriminalRecordController : Controller
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public CriminalRecordController(ApplicationDbContext dataContext, UserManager<ApplicationUser> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }
        public IActionResult New(int suspectNo)
        {
            return View(new CriminalRecord { SuspectNo = suspectNo });
        }

        [HttpPost]
        public async Task<IActionResult> New(CriminalRecord model)
        {
            if (ModelState.IsValid)
            {
                var suspect = _dataContext.Suspects
                    .Include(s => s.CriminalRecords)
                    .FirstOrDefault(s => s.SuspectNo == model.SuspectNo);

                if (suspect != null)
                {
                    // Associate the CriminalRecord with the found Suspect
                    model.Suspect = suspect;

                    // Get the role ID for "Case Manager"
                    var caseManagerRole = await _dataContext.Roles
                        .Where(r => r.Name == "Case Manager")
                        .FirstOrDefaultAsync();

                    if (caseManagerRole != null)
                    {
                        // Get the IDs of users with the "Case Manager" role
                        var caseManagers = _dataContext.UserRoles
                            .Where(ur => ur.RoleId == caseManagerRole.Id)
                            .Select(ur => ur.UserId)
                            .ToList();

                        // Find the user with the least number of cases
                        var leastBusyCaseManager = await _dataContext.Users
                            .Where(u => caseManagers.Contains(u.Id))
                            .OrderBy(u => u.AssignedCriminalRecords.Count)
                            .FirstOrDefaultAsync();

                        if (leastBusyCaseManager != null)
                        {
                            if (leastBusyCaseManager.AssignedCriminalRecords == null)
                            {
                                leastBusyCaseManager.AssignedCriminalRecords = new List<CriminalRecord>();
                            }

                            model.CaseManagerId = leastBusyCaseManager.Id;
                            leastBusyCaseManager.AssignedCriminalRecords.Add(model);

                            _dataContext.CriminalRecords.Add(model);
                            _dataContext.SaveChanges();

                            TempData["success"] = "Criminal Record Added Successfully";
                            return RedirectToAction(nameof(HomeController.Index), "Home", new { suspectId = suspect.SuspectId });
                            //return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }

            // Handle the case when no Case Manager is found or other validation errors
            return View(model);
        }


        /*[HttpPost]
        public IActionResult New(CriminalRecord model)
        {
            

            if (ModelState.IsValid)
            {

                var suspect = _dataContext.Suspects
            .Include(s => s.CriminalRecords)
            .FirstOrDefault(s => s.SuspectNo == model.SuspectNo);

                

                    if (suspect != null)
                    {
  
                        // Associate the CriminalRecord with the found Suspect
                        model.Suspect = suspect;

                    // Find the case manager with the least number of cases
                    var caseManagers = _dataContext.CaseManagers
                        .Include(cm => cm.CriminalRecords)
                        .OrderBy(cm => cm.CriminalRecords.Count)
                        .ToList();

                    if (caseManagers.Any())
                    {
                        var selectedCaseManager = caseManagers.First(); // Case manager with the least cases
                        model.CaseManagerId = selectedCaseManager.CaseManagerId;
                    }

                    _dataContext.CriminalRecords.Add(model);
                        _dataContext.SaveChanges();

                        TempData["success"] = "Criminal Record Added Successfully";
                        return RedirectToAction("Index", "Home");
                    }
                

            }


            

            return View(model);
        }*/

        public async Task<IActionResult> Edit(int recordId)
        {
            
            CriminalRecord? criminalRecord = await _dataContext.CriminalRecords.FindAsync(recordId);
            if (criminalRecord == null)
            {
                return NotFound();
            }
            return View(criminalRecord);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CriminalRecord criminalRecord)
        {

            if (ModelState.IsValid)
            {
                var updatedCriminalRecord = await _dataContext.CriminalRecords
                       .Include(cr => cr.Suspect)
                       .FirstOrDefaultAsync(cr => cr.RecordId == criminalRecord.RecordId);

                if (updatedCriminalRecord == null)
                {
                   
                    return NotFound();
                }

                // Check if Suspect is null before accessing its properties
                if (updatedCriminalRecord.Suspect == null)
                {
                    
                    return NotFound("Associated Suspect not found");
                }

                // Update the properties of the existing CriminalRecord
                _dataContext.Entry(updatedCriminalRecord).CurrentValues.SetValues(criminalRecord);


                await _dataContext.SaveChangesAsync();

                TempData["success"] = "Criminal Record updated successfully";

                //return RedirectToAction("Index", "Home");
                return RedirectToAction(nameof(HomeController.Index), "Home", new { suspectId = updatedCriminalRecord.Suspect.SuspectId });
            }

            return View(criminalRecord);
        }



    }
}
