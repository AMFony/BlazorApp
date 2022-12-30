using Project_Blazor.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Blazor.Shared.DTO
{
    public class RentCarDTO
    {
        [ForeignKey("Rent")]
        public int RentID { get; set; }
        [ForeignKey("Vehicle")]
        public int VehicleID { get; set; }
        [Required]
        public int Quantity { get; set; }


    }
}
