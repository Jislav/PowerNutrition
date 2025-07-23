using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerNutrition.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorApplicatonUserAndHowCartItemsWorkAndRemovedCartEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartsItems_Carts_CartId",
                table: "CartsItems");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartsItems",
                table: "CartsItems");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "CartsItems");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CartsItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartsItems",
                table: "CartsItems",
                columns: new[] { "UserId", "SupplementId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CartsItems_AspNetUsers_UserId",
                table: "CartsItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartsItems_AspNetUsers_UserId",
                table: "CartsItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartsItems",
                table: "CartsItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CartsItems");

            migrationBuilder.AddColumn<Guid>(
                name: "CartId",
                table: "CartsItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartsItems",
                table: "CartsItems",
                columns: new[] { "CartId", "SupplementId" });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CartsItems_Carts_CartId",
                table: "CartsItems",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
