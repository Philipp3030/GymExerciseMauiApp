#if ANDROID
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui.Platform;
using Entry = Microsoft.Maui.Controls.Entry;

namespace GymExerciseMauiApp.Custom
{
    public class CustomScrollListener : RecyclerView.OnScrollListener
    {
        private readonly Entry _focusStealer;
        private bool _userInitiatedScroll = false;

        public CustomScrollListener(Entry focusStealer)
        {
            _focusStealer = focusStealer;
        }

        public override void OnScrollStateChanged(RecyclerView recyclerView, int newState)
        {
            base.OnScrollStateChanged(recyclerView, newState);

            // Track only actual user-initiated scroll
            _userInitiatedScroll = newState == RecyclerView.ScrollStateDragging;
        }

        public async override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            if (_userInitiatedScroll && dy != 0)
            {
                _userInitiatedScroll = false;

                var current = Platform.CurrentActivity?.CurrentFocus;
                current?.ClearFocus();
                await _focusStealer.HideSoftInputAsync(CancellationToken.None);
            }
        }
    }
}
#endif
