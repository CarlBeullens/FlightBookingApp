﻿// <auto-generated />
using System;
using FlightService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FlightService.Data.Migrations
{
    [DbContext(typeof(FlightDbContext))]
    partial class FlightDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FlightService.Models.Flight", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ArrivalCity")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("AvailableSeats")
                        .HasColumnType("integer");

                    b.Property<string>("DepartureCity")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FlightNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Flights");

                    b.HasData(
                        new
                        {
                            Id = new Guid("049f481f-445c-46ac-aa6b-04c0190a9b84"),
                            ArrivalCity = "London",
                            ArrivalTime = new DateTime(2025, 3, 1, 8, 45, 0, 0, DateTimeKind.Utc),
                            AvailableSeats = 0,
                            DepartureCity = "Frankfurt",
                            DepartureTime = new DateTime(2025, 3, 1, 7, 30, 0, 0, DateTimeKind.Utc),
                            FlightNumber = "LH450",
                            Price = 149.99m
                        },
                        new
                        {
                            Id = new Guid("48f5ad70-9172-4406-8a5a-1ca4c2a27f04"),
                            ArrivalCity = "Rome",
                            ArrivalTime = new DateTime(2025, 3, 1, 11, 0, 0, 0, DateTimeKind.Utc),
                            AvailableSeats = 0,
                            DepartureCity = "Paris",
                            DepartureTime = new DateTime(2025, 3, 1, 9, 0, 0, 0, DateTimeKind.Utc),
                            FlightNumber = "AF220",
                            Price = 179.99m
                        },
                        new
                        {
                            Id = new Guid("9376543c-a9d5-4a5a-a4da-bbd985f64753"),
                            ArrivalCity = "Amsterdam",
                            ArrivalTime = new DateTime(2025, 3, 1, 12, 45, 0, 0, DateTimeKind.Utc),
                            AvailableSeats = 0,
                            DepartureCity = "Madrid",
                            DepartureTime = new DateTime(2025, 3, 1, 10, 15, 0, 0, DateTimeKind.Utc),
                            FlightNumber = "IB340",
                            Price = 165.50m
                        },
                        new
                        {
                            Id = new Guid("0cc1a24a-b398-4107-b325-eee4ab2c8045"),
                            ArrivalCity = "Berlin",
                            ArrivalTime = new DateTime(2025, 3, 1, 13, 15, 0, 0, DateTimeKind.Utc),
                            AvailableSeats = 0,
                            DepartureCity = "Stockholm",
                            DepartureTime = new DateTime(2025, 3, 1, 11, 30, 0, 0, DateTimeKind.Utc),
                            FlightNumber = "SK110",
                            Price = 142.99m
                        },
                        new
                        {
                            Id = new Guid("962f2bbf-c0e0-4888-b6df-bbdbe2f2e34b"),
                            ArrivalCity = "Vienna",
                            ArrivalTime = new DateTime(2025, 3, 1, 15, 30, 0, 0, DateTimeKind.Utc),
                            AvailableSeats = 0,
                            DepartureCity = "Zurich",
                            DepartureTime = new DateTime(2025, 3, 1, 14, 0, 0, 0, DateTimeKind.Utc),
                            FlightNumber = "LX225",
                            Price = 155.99m
                        },
                        new
                        {
                            Id = new Guid("5c419887-3bba-407e-9900-f442e67083d8"),
                            ArrivalCity = "Dubai",
                            ArrivalTime = new DateTime(2025, 3, 2, 8, 45, 0, 0, DateTimeKind.Utc),
                            AvailableSeats = 0,
                            DepartureCity = "London",
                            DepartureTime = new DateTime(2025, 3, 1, 22, 15, 0, 0, DateTimeKind.Utc),
                            FlightNumber = "EK050",
                            Price = 459.99m
                        },
                        new
                        {
                            Id = new Guid("7b0bb559-f90b-4b95-bbf7-f9f07e2349c1"),
                            ArrivalCity = "Singapore",
                            ArrivalTime = new DateTime(2025, 3, 2, 16, 45, 0, 0, DateTimeKind.Utc),
                            AvailableSeats = 0,
                            DepartureCity = "Frankfurt",
                            DepartureTime = new DateTime(2025, 3, 1, 21, 30, 0, 0, DateTimeKind.Utc),
                            FlightNumber = "SQ333",
                            Price = 689.99m
                        },
                        new
                        {
                            Id = new Guid("098a2d48-f8a3-4a57-8bad-4c6ac068cf87"),
                            ArrivalCity = "New York",
                            ArrivalTime = new DateTime(2025, 3, 1, 13, 15, 0, 0, DateTimeKind.Utc),
                            AvailableSeats = 0,
                            DepartureCity = "Paris",
                            DepartureTime = new DateTime(2025, 3, 1, 10, 45, 0, 0, DateTimeKind.Utc),
                            FlightNumber = "AA190",
                            Price = 549.99m
                        },
                        new
                        {
                            Id = new Guid("1416d844-e03a-4459-9b31-0dae6d34bcce"),
                            ArrivalCity = "Istanbul",
                            ArrivalTime = new DateTime(2025, 3, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                            AvailableSeats = 0,
                            DepartureCity = "Amsterdam",
                            DepartureTime = new DateTime(2025, 3, 1, 15, 30, 0, 0, DateTimeKind.Utc),
                            FlightNumber = "TK780",
                            Price = 229.99m
                        },
                        new
                        {
                            Id = new Guid("9056aef7-4ae0-4c27-b054-d021f3c84b6f"),
                            ArrivalCity = "Doha",
                            ArrivalTime = new DateTime(2025, 3, 2, 6, 30, 0, 0, DateTimeKind.Utc),
                            AvailableSeats = 0,
                            DepartureCity = "Rome",
                            DepartureTime = new DateTime(2025, 3, 1, 23, 0, 0, 0, DateTimeKind.Utc),
                            FlightNumber = "QR445",
                            Price = 419.99m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
