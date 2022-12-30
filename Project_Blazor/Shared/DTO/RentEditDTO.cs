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
   
    public class RentEditDTO
    {
        [Key]
        public int RentID { get; set; }
        [Required, Column(TypeName = "date"),
        Display(Name = "Rent Date"),
            DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
            ApplyFormatInEditMode = true)]
        public DateTime RentDate { get; set; }
        [Column(TypeName = "date"),
            Display(Name = "Delivery Date"),
            DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
            ApplyFormatInEditMode = true)]
        public DateTime? DeliveryDate { get; set; }
        [Required, EnumDataType(typeof(Status))]
        public Status Status { get; set; }
        [Required]
        public int CustomerID { get; set; }

        public virtual ICollection<RentCarEditDTO> RentCars { get; set; } = new List<RentCarEditDTO>();
    }
}
