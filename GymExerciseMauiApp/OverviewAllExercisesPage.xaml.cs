using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GymExerciseMauiApp;

public partial class OverviewAllExercisesPage : ContentPage
{
    private readonly ApplicationDbContext _context;
    private readonly NavigationDataService _navigationDataService;
    private readonly OverviewAllExercisesViewModel _overviewAllExercisesViewModel;

    public OverviewAllExercisesPage(ApplicationDbContext context, NavigationDataService navigationDataService)
	{
        InitializeComponent();
		_context = context;
        _navigationDataService = navigationDataService;
        _overviewAllExercisesViewModel = new OverviewAllExercisesViewModel(_context);
        BindingContext = _overviewAllExercisesViewModel;
    }

    //private async void OnLabelTapped(object sender, EventArgs e)
    //{
    //    var frame = sender as Frame;
    //    var selectedExercise = frame?.BindingContext as ExerciseViewModel;

    //    if (selectedExercise != null)
    //    {
    //        await Navigation.PushAsync(new UpdateExercisePage(new UpdateExerciseViewModel(_context, selectedExercise)));
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
                _navigationDataService.PreviousPageRoute = nameof(OverviewAllExercisesPage);
                await Shell.Current.GoToAsync(nameof(UpdateExercisePage));
            }
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _overviewAllExercisesViewModel.InitializeAsync();
    }
}