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
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public int MusclegroupId { get; set; }
        public Musclegroup Musclegroup { get; set; }
        public string? MachineName { get; set; }
        public string? Description { get; set; }
        public int? Sets { get; set; }
        //public int? Reps { get; set; }
        public List<Repetition> Reps { get; set; } = new List<Repetition>();
        public int? RepsGoal { get; set; }
        public List<Training> Trainings { get; set; } = new List<Training>();
    }
}
