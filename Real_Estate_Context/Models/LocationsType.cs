﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Real_Estate_Context.Models
{
    [Table("locationsTypes")]
    public partial class LocationsType
    {
        public LocationsType()
        {
            Locations = new HashSet<Location>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("arType")]
        [StringLength(50)]
        public string ArType { get; set; }
        [Column("enType")]
        [StringLength(50)]
        public string EnType { get; set; }

        [InverseProperty("LocationType")]
        public virtual ICollection<Location> Locations { get; set; }
    }
}