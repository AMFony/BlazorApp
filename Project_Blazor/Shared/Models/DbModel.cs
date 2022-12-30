using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_Blazor.Shared.Models
{
    public enum Status { Pending = 1, Rented, Cancelled }
    public class Customer
    {
        public int CustomerID { get; set; }
        [Required, StringLength(50), Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = default!;
        [Required, StringLength(150)]
        public string Address { get; set; } = default!;
        
       
        [Required, StringLength(50), EmailAddress]
        public string Email { get; set; } = default!;

        public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();
    }
    public class Rent
    {
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
        [Required, ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public Customer Customer { get; set; } = default!;
        public virtual ICollection<RentCar> RentCars { get; set; } = new List<RentCar>();

    }
    public class RentCar
    {
        [ForeignKey("Rent")]
        public int RentID { get; set; }
        [ForeignKey("Vehicle")]
        public int VehicleID { get; set; }
        [Required]
        public int Quantity { get; set; }

        public virtual Rent Rent { get; set; } = default!;
        public virtual Vehicle Vehicle { get; set; } = default!;


    }
    public class Vehicle
    {
        public int VehicleID { get; set; }
        [Required, StringLength(50), Display(Name = "Vehicle Name")]
        public string VehicleName { get; set; } = default!;
        [Required, Column(TypeName = "money"), DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Price { get; set; }
        [Required, StringLength(150)]
        public string Picture { get; set; } = default!;
        public bool IsAvailable { get; set; }
        public virtual ICollection<RentCar> RentCars { get; set; } = new List<RentCar>();
    }
    public class VehicleDbContext : DbContext
    {
        public VehicleDbContext(DbContextOptions<VehicleDbContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; } = default!;
        public DbSet<Rent> Rents { get; set; } = default!;
        public DbSet<RentCar> RentCars { get; set; } = default!;
        public DbSet<Vehicle> Vehicles { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RentCar>().HasKey(o => new { o.RentID, o.VehicleID });
        }
    }
}
