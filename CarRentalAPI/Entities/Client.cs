namespace CarRentalAPI.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
        public virtual List<Order> Orders { get; set; }
    }
}