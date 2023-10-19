﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models
{
    public class Product
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Display(Name = "List Price")]
        [Range(1, 1000)]
        public double ListPrice { get; set; }
        [Required]
        [Display(Name = "Price for 2 - 49")]
        [Range(1, 1000)]
        public double Price { get; set; }
        [Required]
        [Display(Name = "Price for 50 - 99")]
        [Range(1, 1000)]
        public double Price50 { get; set; }
        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1, 1000)]
        public double Price100 { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        public int CategoryId { get; set; }

        public string ImageUrl { get; set; }



    }
}
