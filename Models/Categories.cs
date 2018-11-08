using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
namespace productcategories.Models
{
    public class Categories
    {
        // auto-implemented properties need to match columns in your table
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        public List<Associations> Associations {get;set;}
        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
        public Categories(){
            created_at=DateTime.Now;
            updated_at=DateTime.Now;
        }
    }
}