﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Garcon.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GarconEntities : DbContext
    {
        public GarconEntities()
            : base("name=GarconEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Table> Tables { get; set; }
        public virtual DbSet<UserCard> UserCards { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserTipPercent> UserTipPercents { get; set; }
        public virtual DbSet<Merchant> Merchants { get; set; }
    }
}
