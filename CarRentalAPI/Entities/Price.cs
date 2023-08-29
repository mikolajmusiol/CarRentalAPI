namespace CarRentalAPI.Entities
{
    public class Price
    {
        public int Id { get; set; }
        public decimal? PriceForAnHour { get; set; }
        public decimal? PriceForADay { get; set; }
        public decimal? PriceForAWeek { get; set; }
        public int CarId { get; set; }
    }
}
