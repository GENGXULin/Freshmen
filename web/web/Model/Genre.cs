using web.Model;
using web.Models;

namespace web.Models
{
    public class Genre
    {

        public int genreID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}
