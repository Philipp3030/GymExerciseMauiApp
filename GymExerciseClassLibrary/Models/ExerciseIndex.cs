using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Models
{
    public class ExerciseIndex
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public int ExerciseId { get; set; }
        public int TrainingId { get; set; }
    }
}
