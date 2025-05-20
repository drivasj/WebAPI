using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class register_villa_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Detail", "ImageUrl", "LastUpdate", "LastUpdateUser", "Name", "Occupants", "Price", "RegisterDate", "RegisterUser", "SquareMeters" },
                values: new object[,]
                {
                    { 1, "test detail", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Estandar", 5, 80.0, new DateTime(2025, 5, 20, 9, 23, 56, 806, DateTimeKind.Local).AddTicks(4237), "drivasj", 20.0 },
                    { 2, "test detail VIP", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "VIP", 8, 180.0, new DateTime(2025, 5, 20, 9, 23, 56, 806, DateTimeKind.Local).AddTicks(4258), "drivasj", 70.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
