using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; } = true;
        public int MusclegroupId { get; set; }
        public Musclegroup? Musclegroup { get; set; }
        public string? MachineName { get; set; }
        public string? Description { get; set; }  
        public int? AmountOfSets { get; set; }
        public List<Set> Sets { get; set; } = new List<Set>();
        public int? RepsGoal { get; set; }
        public List<Training> Trainings { get; set; } = new List<Training>();
        public List<ExerciseIndex> ExerciseIndices { get; set; } = new List<ExerciseIndex>(); // Change_0210
    }
}
