using ShopsApi.Entities;
using System.Collections.Generic;

namespace ShopsApi
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public string WorkTime { get; set; }
        public int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public int AdressId { get; set; }
        public virtual Adress Adress { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
