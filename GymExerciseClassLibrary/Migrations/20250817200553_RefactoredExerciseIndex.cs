using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymExerciseClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class RefactoredExerciseIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ExerciseIndices_TrainingId",
                table: "ExerciseIndices",
                column: "TrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseIndices_Trainings_TrainingId",
                table: "ExerciseIndices",
                column: "TrainingId",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseIndices_Trainings_TrainingId",
                table: "ExerciseIndices");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseIndices_TrainingId",
                table: "ExerciseIndices");
        }
    }
}
