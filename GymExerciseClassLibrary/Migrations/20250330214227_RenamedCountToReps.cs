using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymExerciseClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class RenamedCountToReps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Count",
                table: "Repetitions",
                newName: "Reps");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Reps",
                table: "Repetitions",
                newName: "Count");
        }
    }
}
