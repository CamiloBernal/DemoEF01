using System;
using System.ComponentModel.DataAnnotations;
using EfDemo.Core.Model;

namespace EfDemo.Data.Presentation.Web.DefaultSite.Models
{
    public class CategoryModel
    {

        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Category name:")]
        [MaxLength(50)]
        public string CategoryName { get; set; }
        
        [Required]
        [Display(Name = "Category description:")]
        public string CategoryDescription { get; set; }

        [Required]
        [Display(Name = "Category status:")]
        public EntityStatus CategoryStatus { get; set; }


        public static explicit operator Category(CategoryModel v)  => new Category
        {
            CategoryName = v.CategoryName,
            CategoryDescription = v.CategoryDescription,
            CategoryStatus = v.CategoryStatus,
            CategoryId =  v.CategoryId

        };
        

    }
}