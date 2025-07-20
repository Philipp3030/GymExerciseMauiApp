using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Mappings;
using GymExerciseClassLibrary.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.ViewModels
{
    public partial class AddMusclegroupViewModel : ObservableValidator
    {
        private readonly ApplicationDbContext _context;
        private readonly MusclegroupService _mgService;
        [ObservableProperty]
        private MusclegroupViewModel _musclegroup = new();

        public AddMusclegroupViewModel(ApplicationDbContext context)
        {
            _context = context;
            _mgService = new MusclegroupService(context);
        }

        [RelayCommand]
        private async Task SaveNewMusclegroup()
        {
            await _mgService.SaveNewMusclegroup(Musclegroup);
        }
    }
}
