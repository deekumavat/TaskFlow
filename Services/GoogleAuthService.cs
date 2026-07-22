using Microsoft.JSInterop;
namespace TaskFlow.Wasm.Services;
public class GoogleAuthService {
 private readonly IJSRuntime _js; public GoogleAuthService(IJSRuntime js)=>_js=js;
 public ValueTask<string?> GetTokenAsync()=>_js.InvokeAsync<string?>("taskFlowAuth.getToken");
 public ValueTask<string?> SignInAsync()=>_js.InvokeAsync<string?>("taskFlowAuth.signIn");
 public ValueTask SignOutAsync()=>_js.InvokeVoidAsync("taskFlowAuth.signOut");
 public ValueTask<bool> IsSignedInAsync()=>_js.InvokeAsync<bool>("taskFlowAuth.isSignedIn");
}