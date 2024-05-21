using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public partial class CabFaresProcessingContext : DbContext
{
    private readonly string ConnectionString;

    public CabFaresProcessingContext(string connectionString)
    {
        this.ConnectionString = connectionString;
    }

    public CabFaresProcessingContext(DbContextOptions<CabFaresProcessingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TaxiTrip> TaxiTrips { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(this.ConnectionString);
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaxiTrip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaxiTrip__3213E83FC94B8655");

            entity.HasIndex(e => e.PulocationId, "IX_PULocationId");

            entity.HasIndex(e => new { e.PulocationId, e.TipAmount }, "IX_PULocationId_tip_amount");

            entity.HasIndex(e => e.TripDistance, "IX_trip_distance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DolocationId).HasColumnName("DOLocationID");
            entity.Property(e => e.FareAmount)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("fare_amount");
            entity.Property(e => e.PassengerCount).HasColumnName("passenger_count");
            entity.Property(e => e.PulocationId).HasColumnName("PULocationID");
            entity.Property(e => e.StoreAndFwdFlag).HasColumnName("store_and_fwd_flag");
            entity.Property(e => e.TipAmount)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("tip_amount");
            entity.Property(e => e.TpepDropoffDatetime)
                .HasColumnType("datetime")
                .HasColumnName("tpep_dropoff_datetime");
            entity.Property(e => e.TpepPickupDatetime)
                .HasColumnType("datetime")
                .HasColumnName("tpep_pickup_datetime");
            entity.Property(e => e.TripDistance).HasColumnName("trip_distance");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
