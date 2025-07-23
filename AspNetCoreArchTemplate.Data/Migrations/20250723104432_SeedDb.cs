using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PowerNutrition.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Protein" },
                    { 2, "Creatine" },
                    { 3, "Glutamine" },
                    { 4, "Vitamins" }
                });

            migrationBuilder.InsertData(
                table: "Supplements",
                columns: new[] { "Id", "Brand", "CategoryId", "Description", "ImageUrl", "Name", "Price", "Stock", "Weight" },
                values: new object[,]
                {
                    { new Guid("42b37657-90fe-4f7d-baed-5ae26d88dbe6"), "HealthCore", 4, "Daily multivitamin with essential nutrients.", "https://2.bgbong.xyz/newdir/article-img8/cvs-mens-gummy-vitamins-a-comprehensive-guide-to-supporting-mens-health-mfuyiqfz77xnr.png", "Multivitamin Complex", 19.99m, 100, 0.10000000000000001 },
                    { new Guid("4696209f-f3f3-47e7-a149-a2611ffc07c7"), "NutraWell", 4, "Supports bone health and immune system.", "https://m.media-amazon.com/images/I/51jjjqpi94L._AC_.jpg", "Vitamin D3", 12.99m, 50, 0.050000000000000003 },
                    { new Guid("d2e21316-1713-4523-b684-2d833716be3d"), "MuscleTech", 2, "Micronized creatine monohydrate for faster absorption.", "https://cdn.shopify.com/s/files/1/0944/0726/files/Creatine-Monohydrate-NCAA-Approved-Supplements-for-College-Students-with-No-Banned-Substances-Safe-for-Test_1800x1800_687a7f60-8727-4d77-a80b-0c522e3e9b1f_480x480.webp?v=1680728035", "Micronized Creatine", 25.99m, 40, 0.29999999999999999 },
                    { new Guid("ebe5d688-3dc6-4637-86d9-4139bcd1043d"), "PureStrength", 1, "High-quality whey protein for muscle recovery.", "https://au.atpscience.com/cdn/shop/articles/web04.jpg?v=1702341918&width=2048", "Whey Protein", 39.99m, 30, 2.0 },
                    { new Guid("effaafbc-ffec-4c8b-a1c8-750dd479ba9e"), "Optimum Nutrition", 3, "Supports muscle recovery and immune health.", "https://m.media-amazon.com/images/I/81WBLQck12L._UF1000,1000_QL80_.jpg", "L-Glutamine Powder", 21.50m, 35, 0.25 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Supplements",
                keyColumn: "Id",
                keyValue: new Guid("42b37657-90fe-4f7d-baed-5ae26d88dbe6"));

            migrationBuilder.DeleteData(
                table: "Supplements",
                keyColumn: "Id",
                keyValue: new Guid("4696209f-f3f3-47e7-a149-a2611ffc07c7"));

            migrationBuilder.DeleteData(
                table: "Supplements",
                keyColumn: "Id",
                keyValue: new Guid("d2e21316-1713-4523-b684-2d833716be3d"));

            migrationBuilder.DeleteData(
                table: "Supplements",
                keyColumn: "Id",
                keyValue: new Guid("ebe5d688-3dc6-4637-86d9-4139bcd1043d"));

            migrationBuilder.DeleteData(
                table: "Supplements",
                keyColumn: "Id",
                keyValue: new Guid("effaafbc-ffec-4c8b-a1c8-750dd479ba9e"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
