using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.Mappings
{
    public partial class Mapper
    {
        #region To ViewModel
        private SetViewModel MapSetToViewModel_Set(Set set, SetViewModel? setVm)
        {
            // if no entity exists, create a new instance
            if (setVm == null)
            {
                return new SetViewModel
                {
                    Id = set.Id,
                    Index = set.Index,
                    Reps = set.Reps.ToString(),
                    Weight = set.Weight.ToString(),
                    ExerciseId = set.Exercise.Id
                };
            }
            // if entity exists, update properties of existing entity
            else
            {
                setVm.Id = set.Id;
                setVm.Index = set.Index;
                setVm.Reps = set.Reps.ToString();
                setVm.Weight = set.Weight.ToString();
                setVm.ExerciseId = set.Exercise.Id;
                return setVm;
            }
        }
        #endregion
    }
}
