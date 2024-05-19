using CrimeManagement.Data;
using CrimeManagement.Helpers;
using CrimeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace CrimeManagement.Controllers
{
    [Authorize]
    public class CaseManagerController : Controller
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthorizationHelper _authorizationHelper;

        public CaseManagerController(ApplicationDbContext dataContext, UserManager<ApplicationUser> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _authorizationHelper = new AuthorizationHelper();
        }
        public async Task<IActionResult> Index()
        {
            var caseManagers = await _userManager.GetUsersInRoleAsync("Case Manager");
           /* var caseManager = _dataContext.CaseManagers.ToList();*/
            return View(caseManagers);
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAllManagers()
        {
            var caseManagers = await _userManager.GetUsersInRoleAsync("Case Manager");

            return Json(new { Data = caseManagers });
        }

        public async Task<IActionResult> ViewAllCases(string caseManagerId)
        {


            var criminalRecord = await _dataContext.CriminalRecords
                .Where(cr => cr.CaseManagerId == caseManagerId)
                .Include(cr => cr.Suspect)
                .ToListAsync();

            var formattedData = criminalRecord.Select(cr => new
            {
                RecordId = cr.RecordId,
                SuspectNo = cr.Suspect?.SuspectNo,  // Assuming SuspectNo is a property of Suspect
                OffenceCommited = cr.OffenceCommited,
                Sentence = cr.Sentence,
                IssuedAt = cr.IssuedAt,
                IssuedBy = cr.IssuedBy,
                IssueDate = cr.IssueDate.ToString("yyyy/MM/dd"),
                Status = cr.Status,
                /*SuspectFirstName = cr.Suspect?.FirstName,  // Include Suspect data
                SuspectLastName = cr.Suspect?.LastName,    // Include Suspect data*/
            });

            //return Json(new { Data = formattedData });

            var jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                // Add other serialization options if needed
            };

            return Json(new { Data = formattedData }, jsonOptions);



            //return Json(new { Data = criminalRecord });
        }

        #endregion

        public async Task<IActionResult> ViewCase(string caseManagerId)
        {


            var criminalRecord = await _dataContext.CriminalRecords
                .Where(cr => cr.CaseManagerId == caseManagerId)
                .Include(cr => cr.Suspect)
                .ToListAsync();



            return View(criminalRecord);
        }

        [Authorize(Roles = "Case Manager, Captain")]
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
        public async Task<IActionResult> Edit(CriminalRecord criminalRecord, string returnUrl)
        {
            // Check if the current user is the case manager or captain for this criminal record
            if (!_authorizationHelper.IsCurrentUserCaseManagerOrCaptain(criminalRecord.CaseManagerId, _userManager, User))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {


                _dataContext.Update(criminalRecord);

                await _dataContext.SaveChangesAsync();

                TempData["success"] = "Criminal Record updated successfully";

                //return RedirectToAction("Index", "Home");
                return Redirect(returnUrl);
            }

            return View(criminalRecord);
        }

    }
}

