using GymExerciseClassLibrary.Data;

namespace GymExerciseMauiApp
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Apply migrations and seed data
                dbContext.SeedData();
            }

        }
        protected override Window CreateWindow(IActivationState activationState)
        {
            var shell = new AppShell();

            return new Window(shell);
        }
    }
}
