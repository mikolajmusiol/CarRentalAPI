namespace CarRentalAPI.Models
{
    public class CarDto
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string YearOfProduction { get; set; }
        public string Color { get; set; }
        public int HorsePower { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
