using web.Models;


namespace web.Model
{
    public class subGenre
    {

        public int ID { get; set; }
        public int genreID { get; set; }
        public int subGenreID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}
