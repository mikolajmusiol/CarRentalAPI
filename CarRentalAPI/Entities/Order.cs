namespace CarRentalAPI.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public TimeSpan RentalPeriod { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public virtual List<Car> Cars { get; set; }
    }
}