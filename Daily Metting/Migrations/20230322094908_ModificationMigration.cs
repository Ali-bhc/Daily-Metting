using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Daily_Metting.Migrations
{
    public partial class ModificationMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubmissionID",
                table: "Attainements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "Attainement_min",
                table: "APUs",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Attainement_Max",
                table: "APUs",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Attainements_SubmissionID",
                table: "Attainements",
                column: "SubmissionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attainements_Submissions_SubmissionID",
                table: "Attainements",
                column: "SubmissionID",
                principalTable: "Submissions",
                principalColumn: "SubmissionID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attainements_Submissions_SubmissionID",
                table: "Attainements");

            migrationBuilder.DropIndex(
                name: "IX_Attainements_SubmissionID",
                table: "Attainements");

            migrationBuilder.DropColumn(
                name: "SubmissionID",
                table: "Attainements");

            migrationBuilder.AlterColumn<int>(
                name: "Attainement_min",
                table: "APUs",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Attainement_Max",
                table: "APUs",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
