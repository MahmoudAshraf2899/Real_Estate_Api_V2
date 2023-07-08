﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Microsoft.EntityFrameworkCore;
using Real_Estate_Context.Models;

namespace Real_Estate_Context.Context
{
    public partial class ecommerce_real_estateContext : DbContext
    {
        public ecommerce_real_estateContext()
        {
        }

        public ecommerce_real_estateContext(DbContextOptions<ecommerce_real_estateContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerService> CustomerServices { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationImage> LocationImages { get; set; }
        public virtual DbSet<LocationsType> LocationsTypes { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectsFeature> ProjectsFeatures { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolesPermission> RolesPermissions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Visitor> Visitors { get; set; }
        public virtual DbSet<WorkFlow> WorkFlows { get; set; }
        public virtual DbSet<WorkFlowContact> WorkFlowContacts { get; set; }
        public virtual DbSet<WorkFlowCycle> WorkFlowCycles { get; set; }
        public virtual DbSet<WorkFlowCycleAssessment> WorkFlowCycleAssessments { get; set; }
        public virtual DbSet<WorkFlowFollowingUser> WorkFlowFollowingUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.

                optionsBuilder.UseSqlServer("Data Source=192.168.1.6; Database=ecommerce_real_estate; User ID=sa;Password=Nabwy12345; Integrated Security=False; Encrypt=false; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_customer_admins_createdBy");
            });

            modelBuilder.Entity<CustomerService>(entity =>
            {
                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CustomerServiceCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_customerServices_admins");

                entity.HasOne(d => d.EditedByNavigation)
                    .WithMany(p => p.CustomerServiceEditedByNavigations)
                    .HasForeignKey(d => d.EditedBy)
                    .HasConstraintName("FK_customerServices_admins1");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasOne(d => d.AddedByNavigation)
                    .WithMany(p => p.LocationAddedByNavigations)
                    .HasForeignKey(d => d.AddedBy)
                    .HasConstraintName("FK_locations_admins");

                entity.HasOne(d => d.DeletedyByNavigation)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.DeletedyBy)
                    .HasConstraintName("FK_locations_customerServices");

                entity.HasOne(d => d.EditedByNavigation)
                    .WithMany(p => p.LocationEditedByNavigations)
                    .HasForeignKey(d => d.EditedBy)
                    .HasConstraintName("FK_locations_admins1");

                entity.HasOne(d => d.LocationType)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.LocationTypeId)
                    .HasConstraintName("FK_locations_locationsTypes");

                entity.HasOne(d => d.PaymentType)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.PaymentTypeId)
                    .HasConstraintName("FK_locations_paymentTypes");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_locations_projects");
            });

            modelBuilder.Entity<LocationImage>(entity =>
            {
                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationImages)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_locationImages_locations");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.PermissionCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_permissions_admins_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.PermissionUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_permissions_admins_updatedBy");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.NoUnits).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.AddedByNavigation)
                    .WithMany(p => p.ProjectAddedByNavigations)
                    .HasForeignKey(d => d.AddedBy)
                    .HasConstraintName("FK_projects_admins1");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.ProjectDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_projects_admins");

                entity.HasOne(d => d.EditedByNavigation)
                    .WithMany(p => p.ProjectEditedByNavigations)
                    .HasForeignKey(d => d.EditedBy)
                    .HasConstraintName("FK_projects_admins2");
            });

            modelBuilder.Entity<ProjectsFeature>(entity =>
            {
                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectsFeatures)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_projects_features_projects");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_reservation_admins_createdBy");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_reservation_location_locationId");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RoleCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_roles_admins_createdBy");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.RoleUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_roles_admins_updatedBy");
            });

            modelBuilder.Entity<RolesPermission>(entity =>
            {
                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.RolesPermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_roles_Permissions_permissions");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RolesPermissions)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_roles_Permissions_roles");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_admins_roles");

                entity.HasOne(d => d.Supervisor)
                    .WithMany(p => p.InverseSupervisor)
                    .HasForeignKey(d => d.SupervisorId)
                    .HasConstraintName("FK_admins_supervisor");
            });

            modelBuilder.Entity<Visitor>(entity =>
            {
                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Visitors)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_visitors_customerServices");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.Visitors)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_visitors_admins");
            });

            modelBuilder.Entity<WorkFlow>(entity =>
            {
                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.WorkFlowCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_work_flow_admins_createdBy");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.WorkFlowDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_work_flow_admins_deletedBy");
            });

            modelBuilder.Entity<WorkFlowContact>(entity =>
            {
                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.WorkFlowContactDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_work_flow_contacts_admins_deletedBy");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WorkFlowContactUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_work_flow_contacts_admins_userId");
            });

            modelBuilder.Entity<WorkFlowCycle>(entity =>
            {
                entity.HasOne(d => d.WorkFlow)
                    .WithMany(p => p.WorkFlowCycles)
                    .HasForeignKey(d => d.WorkFlowId)
                    .HasConstraintName("FK_work_flow_cycle_work_flow");
            });

            modelBuilder.Entity<WorkFlowCycleAssessment>(entity =>
            {
                entity.HasOne(d => d.ActionByAcc)
                    .WithMany(p => p.WorkFlowCycleAssessmentActionByAccs)
                    .HasForeignKey(d => d.ActionByAccId)
                    .HasConstraintName("FK_work_flow_cycle_assessment_admins_actionByAccId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WorkFlowCycleAssessmentUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_work_flow_cycle_assessment_admins_userId");

                entity.HasOne(d => d.WorkFlowContacts)
                    .WithMany(p => p.WorkFlowCycleAssessments)
                    .HasForeignKey(d => d.WorkFlowContactsId)
                    .HasConstraintName("FK_work_flow_cycle_assessment_work_flow_contacts_wf_ContactsId");
            });

            modelBuilder.Entity<WorkFlowFollowingUser>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.WorkFlowFollowingUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_work_flow_following_user_admins_user_id");

                entity.HasOne(d => d.WorkFlow)
                    .WithMany(p => p.WorkFlowFollowingUsers)
                    .HasForeignKey(d => d.WorkFlowId)
                    .HasConstraintName("FK_work_flow_following_user_work_flow_workFlowId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}