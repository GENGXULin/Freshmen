using web.Model;

namespace web.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
