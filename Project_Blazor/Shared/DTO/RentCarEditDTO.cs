using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Blazor.Shared.DTO
{
    
       public class RentCarEditDTO
    {
        [Key]
        public int RentID { get; set; }

        [Required]
        public int VehicleID { get; set; } 
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
