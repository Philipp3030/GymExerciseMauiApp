using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Enums;
using GymExerciseClassLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GymExerciseMauiApp;

public partial class SavedExercisesPage : ContentPage
{
    private readonly ApplicationDbContext _context;
    private readonly NavigationDataService _navigationDataService;
    private readonly SavedExercisesViewModel _savedExercisesViewModel;

    public SavedExercisesPage(ApplicationDbContext context, NavigationDataService navigationDataService)
	{
        InitializeComponent();
		_context = context;
        _navigationDataService = navigationDataService;
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
            _navigationDataService.Exercise = selectedExercise;
            if (_navigationDataService.Exercise != null)
            {
                _navigationDataService.PreviousPageRoute = nameof(SavedExercisesPage);
                await Shell.Current.GoToAsync(nameof(ExerciseUpdatePage));
            }
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _savedExercisesViewModel.InitializeAsync();
    }
}