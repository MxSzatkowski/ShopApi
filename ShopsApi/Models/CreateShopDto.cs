using System.ComponentModel.DataAnnotations;

namespace ShopsApi.Models
{
    public class CreateShopDto
    {
        [Required]

        public string Name { get; set; }
        [Required]

        public string Description { get; set; }
        public string Type { get; set; }

        public string WorkTime { get; set; }

        public string ContactEmail { get; set; }

        public string ContactNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }

    }
}
