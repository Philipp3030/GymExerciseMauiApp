using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymExerciseClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class FixedTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseTraining_Trainings_TrainigsId",
                table: "ExerciseTraining");

            migrationBuilder.RenameColumn(
                name: "TrainigsId",
                table: "ExerciseTraining",
                newName: "TrainingsId");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseTraining_TrainigsId",
                table: "ExerciseTraining",
                newName: "IX_ExerciseTraining_TrainingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseTraining_Trainings_TrainingsId",
                table: "ExerciseTraining",
                column: "TrainingsId",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseTraining_Trainings_TrainingsId",
                table: "ExerciseTraining");

            migrationBuilder.RenameColumn(
                name: "TrainingsId",
                table: "ExerciseTraining",
                newName: "TrainigsId");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseTraining_TrainingsId",
                table: "ExerciseTraining",
                newName: "IX_ExerciseTraining_TrainigsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseTraining_Trainings_TrainigsId",
                table: "ExerciseTraining",
                column: "TrainigsId",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
