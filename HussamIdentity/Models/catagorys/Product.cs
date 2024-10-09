﻿using System.ComponentModel.DataAnnotations.Schema;

namespace HussamIdentity.Models.catagorys
{
    public class Product
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }
        public int ProdcctPrice { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }


    }
}
