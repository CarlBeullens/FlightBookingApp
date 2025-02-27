using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlightService.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FlightNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    DepartureCity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ArrivalCity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AvailableSeats = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "Id", "ArrivalCity", "ArrivalTime", "AvailableSeats", "DepartureCity", "DepartureTime", "FlightNumber", "Price" },
                values: new object[,]
                {
                    { new Guid("049f481f-445c-46ac-aa6b-04c0190a9b84"), "London", new DateTime(2025, 3, 1, 8, 45, 0, 0, DateTimeKind.Utc), 0, "Frankfurt", new DateTime(2025, 3, 1, 7, 30, 0, 0, DateTimeKind.Utc), "LH450", 149.99m },
                    { new Guid("098a2d48-f8a3-4a57-8bad-4c6ac068cf87"), "New York", new DateTime(2025, 3, 1, 13, 15, 0, 0, DateTimeKind.Utc), 0, "Paris", new DateTime(2025, 3, 1, 10, 45, 0, 0, DateTimeKind.Utc), "AA190", 549.99m },
                    { new Guid("0cc1a24a-b398-4107-b325-eee4ab2c8045"), "Berlin", new DateTime(2025, 3, 1, 13, 15, 0, 0, DateTimeKind.Utc), 0, "Stockholm", new DateTime(2025, 3, 1, 11, 30, 0, 0, DateTimeKind.Utc), "SK110", 142.99m },
                    { new Guid("1416d844-e03a-4459-9b31-0dae6d34bcce"), "Istanbul", new DateTime(2025, 3, 1, 20, 0, 0, 0, DateTimeKind.Utc), 0, "Amsterdam", new DateTime(2025, 3, 1, 15, 30, 0, 0, DateTimeKind.Utc), "TK780", 229.99m },
                    { new Guid("48f5ad70-9172-4406-8a5a-1ca4c2a27f04"), "Rome", new DateTime(2025, 3, 1, 11, 0, 0, 0, DateTimeKind.Utc), 0, "Paris", new DateTime(2025, 3, 1, 9, 0, 0, 0, DateTimeKind.Utc), "AF220", 179.99m },
                    { new Guid("5c419887-3bba-407e-9900-f442e67083d8"), "Dubai", new DateTime(2025, 3, 2, 8, 45, 0, 0, DateTimeKind.Utc), 0, "London", new DateTime(2025, 3, 1, 22, 15, 0, 0, DateTimeKind.Utc), "EK050", 459.99m },
                    { new Guid("7b0bb559-f90b-4b95-bbf7-f9f07e2349c1"), "Singapore", new DateTime(2025, 3, 2, 16, 45, 0, 0, DateTimeKind.Utc), 0, "Frankfurt", new DateTime(2025, 3, 1, 21, 30, 0, 0, DateTimeKind.Utc), "SQ333", 689.99m },
                    { new Guid("9056aef7-4ae0-4c27-b054-d021f3c84b6f"), "Doha", new DateTime(2025, 3, 2, 6, 30, 0, 0, DateTimeKind.Utc), 0, "Rome", new DateTime(2025, 3, 1, 23, 0, 0, 0, DateTimeKind.Utc), "QR445", 419.99m },
                    { new Guid("9376543c-a9d5-4a5a-a4da-bbd985f64753"), "Amsterdam", new DateTime(2025, 3, 1, 12, 45, 0, 0, DateTimeKind.Utc), 0, "Madrid", new DateTime(2025, 3, 1, 10, 15, 0, 0, DateTimeKind.Utc), "IB340", 165.50m },
                    { new Guid("962f2bbf-c0e0-4888-b6df-bbdbe2f2e34b"), "Vienna", new DateTime(2025, 3, 1, 15, 30, 0, 0, DateTimeKind.Utc), 0, "Zurich", new DateTime(2025, 3, 1, 14, 0, 0, 0, DateTimeKind.Utc), "LX225", 155.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
