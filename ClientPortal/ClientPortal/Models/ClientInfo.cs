using System;
using System.ComponentModel.DataAnnotations;

namespace ClientPortal.Models
{
    public class ClientInfo
    {
        [Required(ErrorMessage = "Please select the option that best describes you")]
        [Range(1, 32767, ErrorMessage = "Please select the option that best describes you")]
        public short Gender { set; get; }
        public short CurrentLocation { set; get; }
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { set; get; }
        public string MiddleName { set; get; }
        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { set; get; }
        public bool HasAka { set; get; }
        public string Aka1 { set; get; }
        public string Aka2 { set; get; }
        public short CountryOfBirth { set; get; }
        [Required(ErrorMessage = "Please enter your date of birth")]
        public DateTime DOB { set; get; }
        public string DisabilityYN { set; get; }
        public string MedicAlertYN { set; get; }
        public bool HasDisability { set; get; }
        public bool HasMedicAlert { set; get; }
        public bool IsVeteran { set; get; }
        [Required(ErrorMessage = "Please select the option that best describes you")]
        [Range(1, 32767, ErrorMessage = "Please select the option that best describes you")]
        public short VeteranStateID { set; get; }
        [Required(ErrorMessage = "Please select the option that best describes you")]
        [Range(1, 32767, ErrorMessage = "Please select the option that best describes you")]
        public short CitizenshipTypeID { set; get; }
        [Required(ErrorMessage = "Please select the option that best describes you")]
        [Range(1, 32767, ErrorMessage = "Please select the option that best describes you")]
        public bool IsIndigenous { set; get; }
        [Required(ErrorMessage = "Please select the option that best describes you")]
        [Range(1, 32767, ErrorMessage = "Please select the option that best describes you")]
        public short AboriginalIndicatorID { set; get; }
        public short GeoRegionTypeID { set; get; }
        public bool UsedApproxAge { set; get; }
        [Required(ErrorMessage = "Please enter an approximate value of your age")]
        [Range(1, 120, ErrorMessage = "Please enter a valid age between 1 and 120")]
        public short AproximativeAge { set; get; }
    }
}