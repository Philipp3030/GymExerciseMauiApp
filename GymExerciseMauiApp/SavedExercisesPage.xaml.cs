using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GymExerciseMauiApp;

public partial class SavedExercisesPage : ContentPage
{
    private readonly ApplicationDbContext _context;

    public SavedExercisesPage(ApplicationDbContext context)
	{
        InitializeComponent();
		_context = context;
        BindingContext = new SavedExercisesViewModel(_context);
    }

    //private async void OnLabelTapped(object sender, EventArgs e)
    //{
    //    var frame = sender as Frame;
    //    var selectedExercise = frame?.BindingContext as ExerciseViewModel;

    //    if (selectedExercise != null)
    //    {
    //        await Navigation.PushAsync(new ExerciseUpdatePage(new ExerciseUpdateViewModel(_context, selectedExercise)));
    //    }
    //}

    private async void NavigateToUpdateExercise(object sender, EventArgs e)
    {
        var button = sender as Button;
        var selectedExercise = button?.BindingContext as ExerciseViewModel;

        if (selectedExercise != null)
        {
            await Navigation.PushAsync(new ExerciseUpdatePage(new ExerciseUpdateViewModel(_context, selectedExercise))); 
        }
    }
}