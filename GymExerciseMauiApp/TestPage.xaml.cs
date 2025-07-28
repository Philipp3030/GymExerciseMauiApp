using System.Collections.ObjectModel;

namespace GymExerciseMauiApp;

public partial class TestPage : ContentPage
{
	public TestPage()
	{
		InitializeComponent();

    }

    void OnFullySwiped(object sender, EventArgs e)
    {
        // Trigger your logic here
        Console.WriteLine("Full swipe completed!");
    }




}