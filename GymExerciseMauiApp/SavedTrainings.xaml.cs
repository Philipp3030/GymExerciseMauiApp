using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;

namespace GymExerciseMauiApp;

public partial class SavedTrainings : ContentPage
{
    private readonly ApplicationDbContext _context;
    private readonly TrainingViewModel _trainingViewModel;

    public SavedTrainings(ApplicationDbContext context)
	{
		InitializeComponent();
        _context = context;
        _trainingViewModel = new TrainingViewModel(context);
        BindingContext = _trainingViewModel;
    }

    private async void OnLabelTapped(object sender, EventArgs e)
    {
        var frame = sender as Frame;
        var selectedTraining = frame?.BindingContext as TrainingViewModel;

        if (selectedTraining != null)
        {
            await Navigation.PushAsync(new ExercisesOfTraining(selectedTraining));
        }
    }
}