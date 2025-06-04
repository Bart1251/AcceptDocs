using Microsoft.JSInterop;

namespace AcceptDocs.BlazorClient.Helpers
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
