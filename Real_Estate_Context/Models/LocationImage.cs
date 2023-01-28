﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Real_Estate_Context.Models
{
    [Table("locationImages")]
    public partial class LocationImage
    {
        [Key]
        [Column("imageId")]
        public int ImageId { get; set; }
        [Column("imagePath")]
        [StringLength(500)]
        public string ImagePath { get; set; }
        [Column("locationId")]
        public int? LocationId { get; set; }

        [ForeignKey("LocationId")]
        [InverseProperty("LocationImages")]
        public virtual Location Location { get; set; }
    }
}