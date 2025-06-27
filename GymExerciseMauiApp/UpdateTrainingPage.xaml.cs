using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;

namespace GymExerciseMauiApp;

public partial class UpdateTrainingPage : ContentPage
{
    private readonly UpdateTrainingViewModel _trainingUpdateViewModel;
    private readonly ApplicationDbContext _context;
    private readonly NavigationDataService _navigationDataService;

    // Updated for DI
    public UpdateTrainingPage(ApplicationDbContext context, NavigationDataService navigationDataService)
	{
		InitializeComponent();
        _context = context;
        _navigationDataService = navigationDataService;
        _trainingUpdateViewModel = new UpdateTrainingViewModel(_context, _navigationDataService.Training);
        BindingContext = _trainingUpdateViewModel;
    }

    private void OnExerciseCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;
        var exercise = (ExerciseViewModel)checkbox.BindingContext;
        // Call the ToggleSelection method to update SelectedExercises
        _trainingUpdateViewModel.ToggleSelection(exercise);
    }
}