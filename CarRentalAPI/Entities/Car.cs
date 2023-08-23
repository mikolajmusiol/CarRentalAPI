﻿namespace CarRentalAPI.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int YearOfProduction { get; set; }
        public string? Color { get; set; }
        public int HorsePower { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}