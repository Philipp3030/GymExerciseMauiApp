using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GymExerciseMauiApp;

public partial class SavedExercisesPage : ContentPage
{
    //private readonly ApplicationDbContext _context;
    private readonly ExerciseViewModel _exerciseViewModel;

    public SavedExercisesPage(ApplicationDbContext context)
	{
        InitializeComponent();
		//_context = context;
        //_exerciseViewModel = new ExerciseViewModel(context);
        BindingContext = _exerciseViewModel;
    }
}