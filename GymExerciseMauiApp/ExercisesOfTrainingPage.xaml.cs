using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;

namespace GymExerciseMauiApp;

public partial class ExercisesOfTrainingPage : ContentPage
{
	private readonly TrainingViewModel _trainingViewModel;
    public ExercisesOfTrainingPage(TrainingViewModel trainingViewModel)
	{
		InitializeComponent();
        _trainingViewModel = trainingViewModel;
		BindingContext = _trainingViewModel;
	}
}