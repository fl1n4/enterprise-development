using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Domain.Entities;
public class Request
{
    public required Client Client { get; set; }
    public required RealEstateObject Property { get; set; }
    public RequestType? Type { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? DateCreated { get; set; }
}
