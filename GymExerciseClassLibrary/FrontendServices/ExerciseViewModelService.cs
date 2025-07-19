using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.FrontendServices
{
    public class ExerciseViewModelService
    {
        public ExerciseViewModelService()
        {
        }

        public bool VerifyExercise(ExerciseViewModel exercise)
        {           
            if (!exercise.CheckForErrorsCommand.CanExecute(null))
            {
                return false;
            }
            exercise.CheckForErrorsCommand.Execute(null);
            if (exercise.HasErrors == true)
            {
                return false;
            }

            // check all sets for errors
            foreach (var s in exercise.Sets)
            {
                if (!s.CheckForErrorsCommand.CanExecute(null))
                {
                    return false;
                }
                s.CheckForErrorsCommand.Execute(null);
                if (s.HasErrors == true)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
