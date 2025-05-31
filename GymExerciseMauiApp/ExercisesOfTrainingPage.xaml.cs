using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;
using System.Threading.Tasks;

namespace GymExerciseMauiApp;

public partial class ExercisesOfTrainingPage : ContentPage
{
	private readonly ExercisesOfTrainingViewModel _exercisesOfTrainingViewModel;

    public ExercisesOfTrainingPage(ExercisesOfTrainingViewModel exercisesOfTrainingViewModel)
	{
		InitializeComponent();
        _exercisesOfTrainingViewModel = exercisesOfTrainingViewModel;
        BindingContext = _exercisesOfTrainingViewModel;
	}

    private async void TriggerOnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry setEntry && setEntry.BindingContext is SetViewModel set)
        {
            var exerciseOfSet = _exercisesOfTrainingViewModel.Training.ExercisesOfTraining.FirstOrDefault(e => e.Id == set.ExerciseId);
            if (exerciseOfSet == null)
            {
                return;
            }
            if (!exerciseOfSet.CheckForErrorsCommand.CanExecute(null))
            {
                return;
            }
            exerciseOfSet.CheckForErrorsCommand.Execute(null);
            if (exerciseOfSet.HasErrors == true)
            {
                return;
            }

            // check all sets for errors
            foreach (var s in exerciseOfSet.Sets)
            {
                if (!s.CheckForErrorsCommand.CanExecute(null))
                {
                    return;
                }
                s.CheckForErrorsCommand.Execute(null);
                if (s.HasErrors == true)
                {
                    return;
                }
            }
            await _exercisesOfTrainingViewModel.UpdateExercise(exerciseOfSet);
        }

        if (sender is Entry exerciseEntry && exerciseEntry.BindingContext is ExerciseViewModel exercise)
        {
            if (!exercise.CheckForErrorsCommand.CanExecute(null))
            {
                return;
            }
            exercise.CheckForErrorsCommand.Execute(null);
            if (exercise.HasErrors == true)
            {
                return;
            }

            // check all sets for errors
            foreach (var s in exercise.Sets)
            {
                if (!s.CheckForErrorsCommand.CanExecute(null))
                {
                    return;
                }
                s.CheckForErrorsCommand.Execute(null);
                if (s.HasErrors == true)
                {
                    return;
                }
            }
            await _exercisesOfTrainingViewModel.UpdateExercise(exercise);
        }
    }
}