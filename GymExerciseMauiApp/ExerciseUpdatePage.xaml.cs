using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Enums;
using GymExerciseClassLibrary.ViewModels;

namespace GymExerciseMauiApp;

public partial class ExerciseUpdatePage : ContentPage
{
    private readonly ApplicationDbContext _context;
    private readonly NavigationDataService _navigationDataService;
    private readonly ExerciseUpdateViewModel _exerciseUpdateViewModel;


    public ExerciseUpdatePage(ApplicationDbContext context, NavigationDataService navigationDataService)
    {
        InitializeComponent();
        _context = context;
        _navigationDataService = navigationDataService;
        _exerciseUpdateViewModel = new ExerciseUpdateViewModel(_context, _navigationDataService.Exercise); 
        BindingContext = _exerciseUpdateViewModel;
    }

 //   public ExerciseUpdatePage(ExerciseUpdateViewModel exerciseUpdateViewModel, ApplicationDbContext context)
	//{
	//	InitializeComponent();
 //       _exerciseUpdateViewModel = exerciseUpdateViewModel;
 //       _context = context;
 //       BindingContext = _exerciseUpdateViewModel;
	//}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _exerciseUpdateViewModel.InitializeAsync();
    }

    private void TriggerCheckForErrorsCommand(object sender, EventArgs e)
    {
        _exerciseUpdateViewModel.Exercise.CheckForErrorsCommand?.Execute(null);
    }

    private void TriggerCheckForErrorsSetCommand(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry && entry.BindingContext is SetViewModel set)
        {
            // Call the command manually
            if (set.CheckForErrorsCommand.CanExecute(null))
            {
                set.CheckForErrorsCommand.Execute(null);
            }
        }
    }

    private async void NavigateToLastPage(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ExerciseUpdateViewModel exerciseUpdate)
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}/{_navigationDataService.PreviousPageRoute}");

            //if (exerciseUpdate.Source == SourcePage.SavedExercisesPage)
            //{
            //    await Navigation.PushAsync(new SavedExercisesPage(_context));
            //}
            //else if (exerciseUpdate.Source == SourcePage.ExercisesOfTrainingPage && exerciseUpdate.Training != null)
            //{
            //    await Navigation.PushAsync(new ExercisesOfTrainingPage(_context, new ExercisesOfTrainingViewModel(_context, exerciseUpdate.Training)));
            //}
        }
    }
}