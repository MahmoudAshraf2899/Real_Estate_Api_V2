﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Real_Estate_Context.Models
{
    [Table("work_flow_contacts")]
    public partial class WorkFlowContact
    {
        public WorkFlowContact()
        {
            WorkFlowCycleAssessments = new HashSet<WorkFlowCycleAssessment>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("work_flow_id")]
        public int? WorkFlowId { get; set; }
        [Column("level")]
        public int? Level { get; set; }
        [Column("userId")]
        public int? UserId { get; set; }
        [Column("description")]
        [StringLength(500)]
        public string Description { get; set; }
        [Column("isMultiApproval")]
        public bool? IsMultiApproval { get; set; }
        [Column("isSingleApproval")]
        public bool? IsSingleApproval { get; set; }
        [Column("useSelection")]
        public bool? UseSelection { get; set; }
        [Column("deletedBy")]
        public int? DeletedBy { get; set; }
        [Column("deletedAt", TypeName = "datetime")]
        public DateTime? DeletedAt { get; set; }

        [ForeignKey("DeletedBy")]
        [InverseProperty("WorkFlowContactDeletedByNavigations")]
        public virtual User DeletedByNavigation { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("WorkFlowContactUsers")]
        public virtual User User { get; set; }
        [InverseProperty("WorkFlowContacts")]
        public virtual ICollection<WorkFlowCycleAssessment> WorkFlowCycleAssessments { get; set; }
    }
}