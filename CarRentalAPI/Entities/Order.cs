namespace CarRentalAPI.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public DateTime RentalFrom { get; set; }
        public DateTime RentalTo { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public int CarId { get; set; }
        public virtual Car Car { get; set; }
    }
}