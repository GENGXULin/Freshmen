using System.ComponentModel.DataAnnotations;
using web.Entities;

namespace web.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        [MaxLength(10, ErrorMessage = "Postal code must be no more than 10 characters")]
        public string PostCode { get; set; }


        [Required(ErrorMessage = "Recipient is required")]
        public string Recipient { get; set; }

        public string UserName { get; set; }


        public bool isDefault { get; set; }
    }

}