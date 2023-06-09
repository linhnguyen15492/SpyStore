﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.Models.Entities
{
    [Table("Products", Schema = "Store")]
    public class Product : EntityBase
    {
        [MaxLength(3800)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(50)]
        public string ModelName { get; set; } = string.Empty;

        public bool IsFeatured { get; set; }

        [MaxLength(50)]
        public string ModelNumber { get; set; } = string.Empty;

        [MaxLength(150)]
        public string ProductImage { get; set; } = string.Empty;

        [MaxLength(150)]
        public string ProductImageLarge { get; set; } = string.Empty;

        [MaxLength(150)]
        public string ProductImageThumb { get; set; } = string.Empty;

        [DataType(DataType.Currency)]
        public decimal UnitCost { get; set; }

        [DataType(DataType.Currency)]
        public decimal CurrentPrice { get; set; }

        public int UnitsInStock { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        [InverseProperty(nameof(ShoppingCartRecord.Product))]
        public List<ShoppingCartRecord> ShoppingCartRecords { get; set; } = new List<ShoppingCartRecord>();

        [InverseProperty(nameof(OrderDetail.Product))]
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
