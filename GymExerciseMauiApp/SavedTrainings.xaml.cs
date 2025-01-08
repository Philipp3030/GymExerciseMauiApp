using GymExerciseClassLibrary;
using GymExerciseClassLibrary.ViewModels;

namespace GymExerciseMauiApp;

public partial class SavedTrainings : ContentPage
{
    //private readonly ApplicationDbContext _context;
    private readonly TrainingViewModel _trainingViewModel;

    public SavedTrainings(ApplicationDbContext context)
	{
		InitializeComponent();
        //_context = context;
        _trainingViewModel = new TrainingViewModel(context);
        BindingContext = _trainingViewModel;
    }
}