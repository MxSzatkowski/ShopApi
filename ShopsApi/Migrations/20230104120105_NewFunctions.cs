using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopsApi.Migrations
{
    public partial class NewFunctions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Shops",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shops_CreatedById",
                table: "Shops",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Users_CreatedById",
                table: "Shops",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Users_CreatedById",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Shops_CreatedById",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Shops");
        }
    }
}
