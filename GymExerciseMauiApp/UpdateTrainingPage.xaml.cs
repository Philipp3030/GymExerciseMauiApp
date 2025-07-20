using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;

namespace GymExerciseMauiApp;

public partial class UpdateTrainingPage : ContentPage
{
    private readonly UpdateTrainingViewModel _updateTrainingViewModel;
    private readonly ApplicationDbContext _context;
    private readonly NavigationDataService _navigationDataService;

    // Updated for DI
    public UpdateTrainingPage(ApplicationDbContext context, NavigationDataService navigationDataService)
	{
		InitializeComponent();
        _context = context;
        _navigationDataService = navigationDataService;
        _updateTrainingViewModel = new UpdateTrainingViewModel(_context, _navigationDataService.Training);
        BindingContext = _updateTrainingViewModel;
    }

    private void OnExerciseCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;
        var exercise = (ExerciseViewModel)checkbox.BindingContext;
        // Call the ToggleSelection method to update SelectedExercises
        _updateTrainingViewModel.ToggleSelection(exercise);
    }

    private async void NavigateToLastPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}