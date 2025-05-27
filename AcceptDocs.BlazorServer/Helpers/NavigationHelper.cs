using Microsoft.JSInterop;

namespace AcceptDocs.BlazorServer.Helpers
{
    public class NavigationHelper
    {
        private readonly IJSRuntime _js;

        public NavigationHelper(IJSRuntime js)
        {
            _js = js;
        }

        public async Task GoBackAsync()
        {
            await _js.InvokeVoidAsync("goBack");
        }
    }
}
