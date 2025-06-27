using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.Enums;
using GymExerciseClassLibrary.ViewModels;

namespace GymExerciseMauiApp;

public partial class UpdateExercisePage : ContentPage
{
    private readonly ApplicationDbContext _context;
    private readonly NavigationDataService _navigationDataService;
    private readonly UpdateExerciseViewModel _exerciseUpdateViewModel;


    public UpdateExercisePage(ApplicationDbContext context, NavigationDataService navigationDataService)
    {
        InitializeComponent();
        _context = context;
        _navigationDataService = navigationDataService;
        _exerciseUpdateViewModel = new UpdateExerciseViewModel(_context, _navigationDataService.Exercise); 
        BindingContext = _exerciseUpdateViewModel;
    }

 //   public UpdateExercisePage(UpdateExerciseViewModel UpdateExerciseViewModel, ApplicationDbContext context)
	//{
	//	InitializeComponent();
 //       _exerciseUpdateViewModel = UpdateExerciseViewModel;
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
        if (sender is Button button && button.BindingContext is UpdateExerciseViewModel exerciseUpdate)
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}/{_navigationDataService.PreviousPageRoute}");

            //if (exerciseUpdate.Source == SourcePage.OverviewAllExercisesPage)
            //{
            //    await Navigation.PushAsync(new OverviewAllExercisesPage(_context));
            //}
            //else if (exerciseUpdate.Source == SourcePage.OverviewExercisesOfTrainingPage && exerciseUpdate.Training != null)
            //{
            //    await Navigation.PushAsync(new OverviewExercisesOfTrainingPage(_context, new OverviewExercisesOfTrainingViewModel(_context, exerciseUpdate.Training)));
            //}
        }
    }
}