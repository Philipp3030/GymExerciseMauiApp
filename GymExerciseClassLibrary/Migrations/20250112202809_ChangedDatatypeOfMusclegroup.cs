using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymExerciseClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDatatypeOfMusclegroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Musclegroup",
                table: "Exercises");

            migrationBuilder.AddColumn<int>(
                name: "MusclegroupId",
                table: "Exercises",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_MusclegroupId",
                table: "Exercises",
                column: "MusclegroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Musclegroups_MusclegroupId",
                table: "Exercises",
                column: "MusclegroupId",
                principalTable: "Musclegroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Musclegroups_MusclegroupId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_MusclegroupId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "MusclegroupId",
                table: "Exercises");

            migrationBuilder.AddColumn<string>(
                name: "Musclegroup",
                table: "Exercises",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
