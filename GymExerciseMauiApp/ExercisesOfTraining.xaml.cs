using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;

namespace GymExerciseMauiApp;

public partial class ExercisesOfTraining : ContentPage
{
	private readonly TrainingViewModel _trainingViewModel;
    public ExercisesOfTraining(TrainingViewModel trainingViewModel)
	{
		InitializeComponent();
        _trainingViewModel = trainingViewModel;
		BindingContext = _trainingViewModel;
	}
}