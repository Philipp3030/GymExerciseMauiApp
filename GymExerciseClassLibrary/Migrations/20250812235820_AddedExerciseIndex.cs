using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymExerciseClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddedExerciseIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExerciseIndices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    ExerciseId = table.Column<int>(type: "INTEGER", nullable: false),
                    TrainingId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseIndices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseIndices_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseIndices_ExerciseId",
                table: "ExerciseIndices",
                column: "ExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseIndices");
        }
    }
}
