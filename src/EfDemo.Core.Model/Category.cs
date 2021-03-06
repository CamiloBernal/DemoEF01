﻿namespace EfDemo.Core.Model
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public EntityStatus CategoryStatus { get; set; }
        public long CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
    }
}