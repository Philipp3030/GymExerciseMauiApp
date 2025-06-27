using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;

namespace GymExerciseMauiApp;

public partial class DeleteTrainingPage : ContentPage
{
    private readonly ApplicationDbContext _context;
    private readonly NavigationDataService _navigationDataService;

    public DeleteTrainingPage(ApplicationDbContext context, NavigationDataService navigationDataService)
    {
        InitializeComponent();
        _context = context;
        _navigationDataService = navigationDataService;
        //_updateExerciseViewModel = new UpdateExerciseViewModel(_context, _navigationDataService.Exercise);
        //BindingContext = _updateExerciseViewModel;
    }
}