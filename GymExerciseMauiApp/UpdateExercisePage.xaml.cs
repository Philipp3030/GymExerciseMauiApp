using GymExerciseClassLibrary.Data;
using GymExerciseClassLibrary.ViewModels;

namespace GymExerciseMauiApp;

public partial class UpdateExercisePage : ContentPage
{
    private readonly ApplicationDbContext _context;
    private readonly NavigationDataService _navigationDataService;
    private readonly UpdateExerciseViewModel _updateExerciseViewModel;


    public UpdateExercisePage(ApplicationDbContext context, NavigationDataService navigationDataService)
    {
        InitializeComponent();
        _context = context;
        _navigationDataService = navigationDataService;
        _updateExerciseViewModel = new UpdateExerciseViewModel(_context, _navigationDataService.Exercise); 
        BindingContext = _updateExerciseViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _updateExerciseViewModel.InitializeAsync();
    }

    private void TriggerCheckForErrorsCommand(object sender, EventArgs e)
    {
        _updateExerciseViewModel.Exercise.CheckForErrorsCommand?.Execute(null);
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
        }
    }
}