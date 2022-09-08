using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.Core.Entities
{
    public class Address : Entity
    {
#pragma warning disable CS8618

        private Address()
        {
        }

#pragma warning restore CS8618

        public Address(
            string? firstName,
            string? lastName,
            string? companyName,
            string country,
            string? region,
            string city,
            string streetAddress,
            string postalCode,
            int clubId)
        {
            FirstName = firstName;
            LastName = lastName;
            CompanyName = companyName;
            Country = country;
            Region = region;
            City = city;
            StreetAddress = streetAddress;
            PostalCode = postalCode;
            ClubId = clubId;
        }

        public Address(
            string? firstName,
            string? lastName,
            string? companyName,
            string country,
            string? region,
            string city,
            string streetAddress,
            string postalCode,
            string playerId)
        {
            FirstName = firstName;
            LastName = lastName;
            CompanyName = companyName;
            Country = country;
            Region = region;
            City = city;
            StreetAddress = streetAddress;
            PostalCode = postalCode;
            PlayerId = playerId;
        }

        public string Country { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
        public string? Region { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public string? PlayerId { get; set; }
        public int? ClubId { get; set; }
    }
}