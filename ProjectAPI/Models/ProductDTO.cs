﻿namespace ProjectAPI.Models
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public decimal? Price { get; set; }
        public string? Image { get; set; }
        public CategoryDTO Category { get; set; }
    }
}
