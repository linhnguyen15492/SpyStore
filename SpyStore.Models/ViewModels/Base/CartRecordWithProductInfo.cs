using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.Models.ViewModels.Base
{
    // The final view model combines the ShoppingCartRecord, Product, and Category tables.
    public class CartRecordWithProductInfo : ProductAndCategoryBase
    {
        [DataType(DataType.Date), Display(Name = "Date Created")]
        public DateTime? DateCreated { get; set; }

        public int? CustomerId { get; set; }

        public int Quantity { get; set; }

        [DataType(DataType.Currency), Display(Name = "Line Total")]
        public decimal LineItemTotal { get; set; }
    }
}
