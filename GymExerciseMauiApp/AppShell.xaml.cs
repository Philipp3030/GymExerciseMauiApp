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
            Routing.RegisterRoute(nameof(ExercisesOfTrainingPage), typeof(ExercisesOfTrainingPage));
            Routing.RegisterRoute(nameof(ExerciseUpdatePage), typeof(ExerciseUpdatePage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(SavedExercisesPage), typeof(SavedExercisesPage));
            Routing.RegisterRoute(nameof(TrainingUpdatePage), typeof(TrainingUpdatePage));
            
            Routing.RegisterRoute(nameof(TestPage), typeof(TestPage));
        }
    }
}
