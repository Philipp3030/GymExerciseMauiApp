using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Data
{
    public class NavigationDataService
    {
        public ExerciseViewModel Exercise { get; set; }
        public TrainingViewModel Training { get; set; }
        public string PreviousPageRoute { get; set; }
    }
}
