using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymExerciseClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertyInClassExercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Trainings_TrainingId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_TrainingId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "TrainingId",
                table: "Exercises");

            migrationBuilder.CreateTable(
                name: "ExerciseTraining",
                columns: table => new
                {
                    ExercisesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TrainigsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseTraining", x => new { x.ExercisesId, x.TrainigsId });
                    table.ForeignKey(
                        name: "FK_ExerciseTraining_Exercises_ExercisesId",
                        column: x => x.ExercisesId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseTraining_Trainings_TrainigsId",
                        column: x => x.TrainigsId,
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseTraining_TrainigsId",
                table: "ExerciseTraining",
                column: "TrainigsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseTraining");

            migrationBuilder.AddColumn<int>(
                name: "TrainingId",
                table: "Exercises",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_TrainingId",
                table: "Exercises",
                column: "TrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Trainings_TrainingId",
                table: "Exercises",
                column: "TrainingId",
                principalTable: "Trainings",
                principalColumn: "Id");
        }
    }
}
