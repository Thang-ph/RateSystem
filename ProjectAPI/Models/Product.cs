using System;
using System.Collections.Generic;

namespace ProjectAPI.Models
{
    public partial class Product
    {
        public Product()
        {
            Comments = new HashSet<Comment>();
            Reviews = new HashSet<Review>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public decimal? Price { get; set; }
        public string? Image { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
