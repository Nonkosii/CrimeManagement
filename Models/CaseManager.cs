/*using System.ComponentModel.DataAnnotations;

namespace CrimeManagement.Models
{
    public class CaseManager
    {
        [Key]
        public int CaseManagerId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        // Navigation property for associated criminal records
        public ICollection<CriminalRecord> CriminalRecords
        {
            get; set;
        }
    }
}
*/