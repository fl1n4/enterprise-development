using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.Domain.Entities;
public class Client
{
    public required string FullName { get; set; }
    public required string PassportNumber { get; set; }
    public required string Phone { get; set; }
}
