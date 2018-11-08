using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
namespace productcategories.Models
{
    public class Products 
    {
        // auto-implemented properties need to match columns in your table
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MinLength(3)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }
        public List<Associations> Associations {get;set;}


        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}