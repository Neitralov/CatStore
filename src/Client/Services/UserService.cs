namespace Client.Services;

public class UserService(
    AuthenticationStateProvider authStateProvider)
{
    public async Task<bool> IsUserAuthenticated()
    {
        var authState = await authStateProvider.GetAuthenticationStateAsync();
        return authState.User.Identity!.IsAuthenticated;
    }
}