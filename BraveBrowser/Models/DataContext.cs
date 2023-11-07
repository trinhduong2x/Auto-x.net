using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BraveBrowser.Models
{
	public partial class DataContext : DbContext
	{
		public DataContext()
			: base("name=DataContext")
		{
			this.Configuration.LazyLoadingEnabled = false;
			this.Configuration.ProxyCreationEnabled = false;
		}

		public virtual DbSet<data_comment> data_comment { get; set; }
		public virtual DbSet<data_group> data_group { get; set; }
		public virtual DbSet<data_post> data_post { get; set; }
		public virtual DbSet<user> users { get; set; }
		public virtual DbSet<user_action> user_action { get; set; }
		public virtual DbSet<user_action_log> user_action_log { get; set; }
		public virtual DbSet<user_group> user_group { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<data_group>()
				.HasMany(e => e.data_post)
				.WithRequired(e => e.data_group)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<data_post>()
				.HasMany(e => e.data_comment)
				.WithRequired(e => e.data_post)
				.WillCascadeOnDelete(false);
		}
	}
}
