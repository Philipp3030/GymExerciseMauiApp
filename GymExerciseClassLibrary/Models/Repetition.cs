using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Models
{
    public class Repetition
    {
        public int Id { get; set; }
        public int Set { get; set; }
        public int Count { get; set; }
        public float Weight { get; set; }
    }
}
