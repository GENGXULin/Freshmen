using web.Model;
using web.Models;

namespace web.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int Genre { get; set; }
        public int subGenre { get; set; }
        public DateTime? Published { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }

        public decimal? Price { get; set; } 
        public int? Quantity { get; set; }  

        public virtual Genre GenreNavigation { get; set; }
        public virtual subGenre subGenreNavigation { get; set; }
    }
}