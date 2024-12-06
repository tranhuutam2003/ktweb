using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tam_222631136.Models
{
    public partial class Product
    {
        public Product()
        {
            Comments = new HashSet<Comment>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Price is required")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Price must be a number.")]
        [Range(100.01, double.MaxValue, ErrorMessage = "Price must be greater than 100.")]
        public double UnitPrice { get; set; }

        public string? Image { get; set; }

        public bool Available { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public int ProviderId { get; set; }


        public virtual Category? Category { get; set; }
        public virtual Provider? Provider { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
