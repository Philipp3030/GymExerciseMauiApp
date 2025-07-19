namespace GymExerciseMauiApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddExercisePage), typeof(AddExercisePage));
            Routing.RegisterRoute(nameof(AddMusclegroupPage), typeof(AddMusclegroupPage));
            Routing.RegisterRoute(nameof(AddMusclegroupPopup), typeof(AddMusclegroupPopup));
            Routing.RegisterRoute(nameof(AddTrainingPage), typeof(AddTrainingPage));
            Routing.RegisterRoute(nameof(OverviewExercisesOfTrainingPage), typeof(OverviewExercisesOfTrainingPage));
            Routing.RegisterRoute(nameof(UpdateExercisePage), typeof(UpdateExercisePage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(OverviewAllExercisesPage), typeof(OverviewAllExercisesPage));
            Routing.RegisterRoute(nameof(UpdateTrainingPage), typeof(UpdateTrainingPage));

            Routing.RegisterRoute(nameof(TestPage), typeof(TestPage));
        }
    }
}
