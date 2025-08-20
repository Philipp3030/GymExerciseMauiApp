using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GymExerciseClassLibrary.Mappings
{
    public partial class Mapper
    {
        #region To Model
        private async Task<Musclegroup> MapMusclegroupVmToModel_Musclegroup(MusclegroupViewModel musclegroupVm)
        {
            var existingMusclegroup = await _context.Musclegroups.FirstOrDefaultAsync(m => musclegroupVm.Id != 0 && m.Id == musclegroupVm.Id);

            if (existingMusclegroup == null)
            {
                return new Musclegroup
                {
                    Name = musclegroupVm.Name
                };
            }
            else
            {
                existingMusclegroup.Name = musclegroupVm.Name;
                return existingMusclegroup;
            }
        }
        #endregion

        #region To ViewModel
        private MusclegroupViewModel MapMusclegroupToViewModel_Musclegroup(Musclegroup musclegroup, MusclegroupViewModel? musclegroupVm)
        {
            if (musclegroupVm == null)
            {
                return new MusclegroupViewModel
                {
                    Id = musclegroup.Id,
                    Name = musclegroup.Name
                };
            }
            else
            {
                musclegroupVm.Id = musclegroup.Id;
                musclegroupVm.Name = musclegroup.Name;
                return musclegroupVm;
            }
        }
        #endregion
    }
}
