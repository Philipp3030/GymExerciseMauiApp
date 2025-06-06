namespace GymExerciseMauiApp;

public partial class TestPage : ContentPage
{
	public TestPage()
	{
		InitializeComponent();
	}
    private void OnToggleTapped(object sender, EventArgs e)
    {
        // Toggle visibility
        ExpandableArea.IsVisible = !ExpandableArea.IsVisible;
    }
}