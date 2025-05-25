using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Models
{
    public class Set
    {
        public int Id { get; set; }
        public int Index { get; set; }    // sollte evtl. index oder so heißen
        public int Reps { get; set; }
        public float Weight { get; set; }
        public Exercise Exercise { get; set; }
    }
}
