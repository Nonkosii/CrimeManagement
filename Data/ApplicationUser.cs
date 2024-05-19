using CrimeManagement.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimeManagement.Data
{
    public class ApplicationUser : IdentityUser
    {

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        public ICollection<CriminalRecord> AssignedCriminalRecords { get; set; }
    }
}
