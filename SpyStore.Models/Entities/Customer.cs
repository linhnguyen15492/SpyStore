using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.Models.Entities
{
    [Table("Customers", Schema = "Store")]
    public class Customer : EntityBase
    {
        [DataType(DataType.Text), MaxLength(50), Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, DataType(DataType.EmailAddress), MaxLength(50), Display(Name = "Email Address")]
        public string EmailAddress { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), MaxLength(50)]
        public string Password { get; set; } = string.Empty;

        [InverseProperty(nameof(Order.Customer))]
        public List<Order> Orders { get; set; } = new List<Order>();

        [InverseProperty(nameof(ShoppingCartRecord.Customer))]
        public virtual List<ShoppingCartRecord> ShoppingCartRecords { get; set; } = new List<ShoppingCartRecord>();
    }
}
