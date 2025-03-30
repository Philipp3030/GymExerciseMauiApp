using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymExerciseClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddedWeightToReps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "Repetitions",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Repetitions");
        }
    }
}
