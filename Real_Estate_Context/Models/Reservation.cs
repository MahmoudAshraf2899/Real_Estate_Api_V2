﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Real_Estate_Context.Models
{
    [Table("reservation")]
    public partial class Reservation
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("customerId")]
        public int? CustomerId { get; set; }
        [Column("locationId")]
        public int? LocationId { get; set; }
        [Column("subject")]
        [StringLength(500)]
        public string Subject { get; set; }
        [Column("date", TypeName = "datetime")]
        public DateTime? Date { get; set; }
        [Column("createdBy")]
        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("Reservations")]
        public virtual Admin CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("Reservations")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("LocationId")]
        [InverseProperty("Reservations")]
        public virtual Location Location { get; set; }
    }
}