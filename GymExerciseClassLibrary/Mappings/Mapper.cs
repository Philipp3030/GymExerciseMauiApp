using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Models;
using GymExerciseClassLibrary.ViewModels;
using System.Collections.ObjectModel;

namespace GymExerciseClassLibrary.Mappings
{
    public partial class Mapper
    {
        private readonly ApplicationDbContext _context;

        public Mapper(ApplicationDbContext context)
        {
            _context = context;
        }

        private void UpdateCollection<T>(ObservableCollection<T> target, IEnumerable<T> updatedItems)
        {
            target.Clear();
            foreach (var item in updatedItems)
            {
                target.Add(item);
            }
        }

        #region To Model
        public async Task<Training> MapToModel(TrainingViewModel trainingVm)
        {
            return await MapTrainingVmToModel_Training(trainingVm);
        }

        public async Task<Exercise> MapToModel(ExerciseViewModel exerciseVm)
        {
            return await MapExerciseVmToModel_Exercise(exerciseVm);
        }

        public async Task<Musclegroup> MapToModel(MusclegroupViewModel musclegroupVm)
        {
            return await MapMusclegroupVmToModel_Musclegroup(musclegroupVm);
        }
        #endregion

        #region To ViewModel
        public TrainingViewModel MapToViewModel(Training training, TrainingViewModel? trainingVm)
        {
            return MapTrainingToViewModel_Training(training, trainingVm);
        }

        public ExerciseViewModel MapToViewModel(Exercise exercise, ExerciseViewModel? exerciseVm)
        {
            return MapExerciseToViewModel_Exercise(exercise, exerciseVm);
        }

        public SetViewModel MapToViewModel(Set set, SetViewModel? setVm)
        {
            return MapSetToViewModel_Set(set, setVm);
        }

        public MusclegroupViewModel MapToViewModel(Musclegroup musclegroup, MusclegroupViewModel? musclegroupVm)
        {
            return MapMusclegroupToViewModel_Musclegroup(musclegroup, musclegroupVm);
        }
        #endregion
    }
}
