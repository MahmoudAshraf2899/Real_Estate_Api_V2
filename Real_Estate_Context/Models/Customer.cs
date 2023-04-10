﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Real_Estate_Context.Models
{
    [Table("customer")]
    public partial class Customer
    {
        public Customer()
        {
            Reservations = new HashSet<Reservation>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(350)]
        public string Name { get; set; }
        [Column("phone")]
        [StringLength(50)]
        public string Phone { get; set; }
        [Column("telephone")]
        [StringLength(50)]
        public string Telephone { get; set; }
        [Column("address")]
        [StringLength(500)]
        public string Address { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
        [Column("createdBy")]
        public int? CreatedBy { get; set; }
        [Column("createdAt", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("Customers")]
        public virtual Admin CreatedByNavigation { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}