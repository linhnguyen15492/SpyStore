﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.Models.ViewModels.Base
{
    // When viewing an order online, the order details contain product and category information as well.
    public class OrderDetailWithProductInfo : ProductAndCategoryBase
    {
        public int OrderId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [DataType(DataType.Currency), Display(Name = "Total")]
        public decimal? LineItemTotal { get; set; }
    }
}
