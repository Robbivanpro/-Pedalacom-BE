using System;

namespace PedalacomOfficial.Models;

/// <summary>
/// DTO for Customer and Address information.
/// </summary>
public class CustomerAddressDTO
{
    // Dati del Cliente
    public int CustomerId { get; set; }
    public string? Title { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string? Suffix { get; set; }
    public string? CompanyName { get; set; }
    public string? EmailAddress { get; set; }
    public string? Phone { get; set; }

    // Dati dell'Indirizzo
    public int AddressId { get; set; }
    public string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string City { get; set; }
    public string StateProvince { get; set; }
    public string CountryRegion { get; set; }
    public string PostalCode { get; set; }



    // Altre proprietà comuni
    public DateTime ModifiedDate { get; set; }

    public string AddressType { get; set; } = null!;

}
