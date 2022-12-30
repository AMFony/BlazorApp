using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project_Blazor.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project_Blazor.Shared.DTO
{
    public class VehicleDTO
    {
        public int VehicleID { get; set; }
        [Required, StringLength(50), Display(Name = "Vehicle Name")]
        public string VehicleName { get; set; } = default!;
        [Required, DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Price { get; set; }
        [Required, StringLength(150)]
        public string Picture { get; set; } = default!;
        public bool IsAvailable { get; set; }
        
    }
}
