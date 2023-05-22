using SpyStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.Models.ViewModels.Base
{
    public class ProductAndCategoryBase : EntityBase
    {
        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; } = string.Empty;

        public int ProductId { get; set; }

        [MaxLength(3800)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(50)]
        [Display(Name = "Model")]
        public string ModelName { get; set; } = string.Empty;

        [Display(Name = "Is Featured Product")]
        public bool IsFeatured { get; set; }

        [MaxLength(50)]
        [Display(Name = "Model Number")]
        public string ModelNumber { get; set; } = string.Empty;

        [MaxLength(150)]
        public string ProductImage { get; set; } = string.Empty;

        [MaxLength(150)]
        public string ProductImageLarge { get; set; } = string.Empty;

        [MaxLength(150)]
        public string ProductImageThumb { get; set; } = string.Empty;

        [DataType(DataType.Currency), Display(Name = "Cost")]
        public decimal UnitCost { get; set; }

        [DataType(DataType.Currency), Display(Name = "Price")]
        public decimal CurrentPrice { get; set; }

        [Display(Name = "In Stock")]
        public int UnitsInStock { get; set; }
    }
}
