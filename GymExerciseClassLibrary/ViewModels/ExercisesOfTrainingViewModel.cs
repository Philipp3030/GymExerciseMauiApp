using CommunityToolkit.Mvvm.ComponentModel;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class ExercisesOfTrainingViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;

        [ObservableProperty]
        private ObservableCollection<ExerciseViewModel> _exerciseVMsOfTraining = new();
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private string? _description;
        [ObservableProperty]
        private TrainingViewModel? _selectedTrainingVM;
        [ObservableProperty]
        private string? _errorMessage;

        public ExercisesOfTrainingViewModel() { }
        public ExercisesOfTrainingViewModel(ApplicationDbContext context)
        {
            _context = context;
            LoadExercisesOfTraining();
        }


        private async void LoadExercisesOfTraining()
        {
            var training = await _context.Trainings.FirstOrDefaultAsync(t => t.Id == this.Id);
            foreach (var exercise in training.Exercises)
            {
                ExerciseVMsOfTraining.Add(Mapper.MapExerciseToViewModel(exercise));
            }
        }
    }
}
