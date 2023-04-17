﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Real_Estate_Context.Models
{
    [Table("work_flow")]
    public partial class WorkFlow
    {
        public WorkFlow()
        {
            WorkFlowCycles = new HashSet<WorkFlowCycle>();
            WorkFlowFollowingUsers = new HashSet<WorkFlowFollowingUser>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("subject")]
        [StringLength(500)]
        public string Subject { get; set; }
        [Column("description")]
        [StringLength(500)]
        public string Description { get; set; }
        [Column("refCode")]
        [StringLength(50)]
        public string RefCode { get; set; }
        [Column("serialNo")]
        public int? SerialNo { get; set; }
        [Column("creationDate", TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }
        [Column("createdBy")]
        public int? CreatedBy { get; set; }
        [Column("deletedBy")]
        public int? DeletedBy { get; set; }
        [Column("deletedAt", TypeName = "datetime")]
        public DateTime? DeletedAt { get; set; }
        [Column("rejectOption")]
        public int? RejectOption { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("WorkFlowCreatedByNavigations")]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("WorkFlowDeletedByNavigations")]
        public virtual User DeletedByNavigation { get; set; }
        [InverseProperty("WorkFlow")]
        public virtual ICollection<WorkFlowCycle> WorkFlowCycles { get; set; }
        [InverseProperty("WorkFlow")]
        public virtual ICollection<WorkFlowFollowingUser> WorkFlowFollowingUsers { get; set; }
    }
}