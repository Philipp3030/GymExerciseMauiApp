using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Models
{
    public class Repetition
    {

        // die Klasse sollte Set heißen, oder?
        public int Id { get; set; }
        public int Set { get; set; }    // sollte evtl. index oder so heißen
        public int Reps { get; set; }
        public float Weight { get; set; }
        public Exercise Exercise { get; set; }
    }
}
