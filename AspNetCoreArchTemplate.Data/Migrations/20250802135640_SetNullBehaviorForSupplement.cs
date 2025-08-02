using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerNutrition.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetNullBehaviorForSupplement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supplements_Categories_CategoryId",
                table: "Supplements");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Supplements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Supplements_Categories_CategoryId",
                table: "Supplements",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supplements_Categories_CategoryId",
                table: "Supplements");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Supplements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Supplements_Categories_CategoryId",
                table: "Supplements",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
