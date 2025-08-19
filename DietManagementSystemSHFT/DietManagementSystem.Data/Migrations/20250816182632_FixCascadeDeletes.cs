using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeDeletes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientProgressRecords_Dietitians_DietitianId",
                table: "ClientProgressRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_DietPlans_Dietitians_DietitianId",
                table: "DietPlans");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProgressRecords_Dietitians_DietitianId",
                table: "ClientProgressRecords",
                column: "DietitianId",
                principalTable: "Dietitians",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DietPlans_Dietitians_DietitianId",
                table: "DietPlans",
                column: "DietitianId",
                principalTable: "Dietitians",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientProgressRecords_Dietitians_DietitianId",
                table: "ClientProgressRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_DietPlans_Dietitians_DietitianId",
                table: "DietPlans");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProgressRecords_Dietitians_DietitianId",
                table: "ClientProgressRecords",
                column: "DietitianId",
                principalTable: "Dietitians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DietPlans_Dietitians_DietitianId",
                table: "DietPlans",
                column: "DietitianId",
                principalTable: "Dietitians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
