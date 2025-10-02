using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Models
{
    public class Training
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
        public List<ExerciseIndex> ExerciseIndices { get; set; } = new List<ExerciseIndex>();   // Change_0210
    }
}
