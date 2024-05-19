using CrimeManagement.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimeManagement.Models
{
    public class CriminalRecord
    {
        [Key]
        [Display(Name = "Record ID")]
        public int RecordId { get; set; }
        
        [Required]
        [Display(Name = "Offence Commited")]
        public string OffenceCommited { get; set; }

        [Required(ErrorMessage = "Sentence is required")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Sentence must be a positive number")]
        public string Sentence { get; set; }

        [Required]
        [Display(Name = "Issued At")]
        [StringLength(50, ErrorMessage = "Issued At cannot exceed 50 characters.")]
        public string IssuedAt { get; set; }
        [Required]
        [Display(Name = "Issued By")]
        [StringLength(50, ErrorMessage = "Issued By cannot exceed 50 characters.")]
        public string IssuedBy { get; set; }

        [Required]
        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime IssueDate { get; set; } = DateTime.Now;

        [Required]
        public string Status { get; set; } = "Acknowledged";

        public int SuspectNo { get; set; }

        
        public Suspect? Suspect { get; set; }

        public string CaseManagerId { get; set; }
       
        public ApplicationUser? CaseManager { get; set; }
        /*public CaseManager? CaseManager { get; set; }*/


    }
}
