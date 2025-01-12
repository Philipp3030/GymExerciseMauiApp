using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GymExerciseMauiApp;

public partial class SavedExercises : ContentPage
{
    //private readonly ApplicationDbContext _context;
    private readonly ExerciseViewModel _exerciseViewModel;

    public SavedExercises(ApplicationDbContext context)
	{
        InitializeComponent();
		//_context = context;
        _exerciseViewModel = new ExerciseViewModel(context);
        BindingContext = _exerciseViewModel;
    }
}