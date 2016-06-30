namespace EfDemo.Core.Model
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public EntityStatus CategoryStatus { get; set; }

        public User CreatedBy { get; set; }
    }
}