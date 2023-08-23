namespace CarRentalAPI.Models
{
    public class UpdateCarDto
    {
        public int? Mileage { get; set; }
        public string? Color { get; set; }
        public int? HorsePower { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }
}
