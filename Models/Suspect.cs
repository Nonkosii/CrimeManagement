using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CrimeManagement.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SouthAfricanIdAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not long idNumber)
            {
                return new ValidationResult("Invalid ID Length.");
            }

            string idString = idNumber.ToString("D13");

            // Use the Luhn algorithm to validate the South African ID number
            if (!LuhnAlgorithm.ValidateSouthAfricanId(idString))
            {
                return new ValidationResult("Invalid South African ID number format.");
            }

            string dobString = idString.Substring(0, 6);

            if (DateTime.TryParseExact(dobString, "yyMMdd", null, System.Globalization.DateTimeStyles.None, out _))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid South African ID number format.");
        }
    }

    

    public static class LuhnAlgorithm
    {
        public static bool ValidateSouthAfricanId(string idNumber)
        {
            if (idNumber.Length != 13)
            {
                return false;
            }

            int[] digits = idNumber.Select(c => int.Parse(c.ToString())).ToArray();

            for (int i = digits.Length - 2; i >= 0; i -= 2)
            {
                int doubledDigit = digits[i] * 2;
                digits[i] = doubledDigit > 9 ? doubledDigit - 9 : doubledDigit;
            }

            int sum = digits.Sum();

            return sum % 10 == 0;
        }
    }

    public class Suspect
    {
        [Key]
        [Display(Name = "Suspect No")]
        public int SuspectNo { get; set; }

        [Required(ErrorMessage = "ID number is required.")]
        [Display(Name = "Suspect ID")]
        [SouthAfricanId(ErrorMessage = "Invalid South African ID number format.")]
        public long SuspectId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        public ICollection<CriminalRecord>? CriminalRecords { get; set; }
    }
}
