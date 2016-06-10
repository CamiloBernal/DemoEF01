namespace Domain
{
    public class Phone
    {
        public int Id { get; set; }
        public virtual Country Country { get; set; }
        public string Number { get; set; }
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}