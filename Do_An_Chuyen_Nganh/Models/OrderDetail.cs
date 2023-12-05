﻿using Do_An_Chuyen_Nganh.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Do_An_Chuyen_Nganh.Models
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrderId { get; set; }


        public int ProductId { get; set; }

        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public int Quantity { get; set; }

        public Order? Order { get; set; }
        public Product? Product { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.ĐangXửLý;

    }
}
