using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GymExerciseMauiApp;

public partial class SavedExercisesPage : ContentPage
{
    private readonly ApplicationDbContext _context;
    private readonly SavedExercisesViewModel _savedExercisesViewModel;

    public SavedExercisesPage(ApplicationDbContext context)
	{
        InitializeComponent();
		_context = context;
        _savedExercisesViewModel = new SavedExercisesViewModel(_context);
        BindingContext = _savedExercisesViewModel;
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

    private async void NavigateToUpdateExercise(object sender, TappedEventArgs e)
    {
        var image = sender as Image;
        var selectedExercise = image?.BindingContext as ExerciseViewModel;
        
        if (selectedExercise != null)
        {
            await Navigation.PushAsync(new ExerciseUpdatePage(new ExerciseUpdateViewModel(_context, selectedExercise))); 
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _savedExercisesViewModel.InitializeAsync();
    }
}