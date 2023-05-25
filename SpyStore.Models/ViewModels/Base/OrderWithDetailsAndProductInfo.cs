using Microsoft.VisualBasic;
using SpyStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.Models.ViewModels.Base
{
    // The order detail screen shows product information as well, so the next view model is the Order Detail records combined with the product information.
    public class OrderWithDetailsAndProductInfo : EntityBase
    {
        public int CustomerId { get; set; }

        [DataType(DataType.Currency), Display(Name = "Total")]
        public decimal? OrderTotal { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Ordered")]
        public DateTime OrderDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Shipped")]
        public DateTime ShipDate { get; set; }

        public IList<OrderDetailWithProductInfo> OrderDetails { get; set; }
    }
}
