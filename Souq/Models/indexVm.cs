namespace Souq.Models
{
    public class indexVm
    {

        public indexVm()
        {
            Categories = new List<Category>();
            products = new List<Product>();
            Reviews = new List<Review>();
            Latestproducts = new List<Product>();
        }
        public List<Category> Categories{ get; set; }
        public List<Product> products { get; set; }
        public List<Product> Latestproducts { get; set; }


        public List<Review> Reviews { get; set; }

    }
}
