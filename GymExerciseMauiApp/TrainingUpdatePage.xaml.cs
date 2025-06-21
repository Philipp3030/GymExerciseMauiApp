using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;

namespace GymExerciseMauiApp;

public partial class TrainingUpdatePage : ContentPage
{
    private readonly TrainingUpdateViewModel _trainingUpdateViewModel;
    private readonly ApplicationDbContext _context;
    private readonly NavigationDataService _navigationDataService;

    // Updated for DI
    public TrainingUpdatePage(ApplicationDbContext context, NavigationDataService navigationDataService)
	{
		InitializeComponent();
        _context = context;
        _navigationDataService = navigationDataService;
        _trainingUpdateViewModel = new TrainingUpdateViewModel(_context, _navigationDataService.Training);
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