﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Real_Estate_Context.Models
{
    [Table("work_flow_following_user")]
    public partial class WorkFlowFollowingUser
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("work_flow_id")]
        public int? WorkFlowId { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("WorkFlowFollowingUsers")]
        public virtual User User { get; set; }
        [ForeignKey("WorkFlowId")]
        [InverseProperty("WorkFlowFollowingUsers")]
        public virtual WorkFlow WorkFlow { get; set; }
    }
}