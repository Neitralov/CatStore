namespace Client;

public class CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient httpClient) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();
        
        var accessToken = await localStorage.GetItemAsStringAsync("AccessToken");
        var refreshToken = await localStorage.GetItemAsStringAsync("RefreshToken");
        
        if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
        {
            var accessTokenData = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            
            if (IsAccessTokenValid(accessTokenData.ValidTo))
            {
                identity = new ClaimsIdentity(accessTokenData.Claims, "jwt");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
            else
            {
                if (await TryRefreshTokens(accessToken, refreshToken))
                {
                    identity = new ClaimsIdentity(accessTokenData.Claims, "jwt");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
                else
                {
                    await localStorage.RemoveItemAsync("AccessToken");
                    await localStorage.RemoveItemAsync("RefreshToken");
                }
            }
        }
        
        var state = new AuthenticationState(new ClaimsPrincipal(identity));

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }
    
    private async Task<bool> TryRefreshTokens(string accessToken, string refreshToken)
    {
        var request = new RefreshUserTokensRequest(accessToken, refreshToken);
        var response = await httpClient.PostAsJsonAsync("/api/users/refresh-tokens", request);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginUserResponse>();
        
            await localStorage.SetItemAsStringAsync("AccessToken", result?.AccessToken!);
            await localStorage.SetItemAsStringAsync("RefreshToken", result?.RefreshToken!);
        }
        
        return response.IsSuccessStatusCode;
    }
    
    private bool IsAccessTokenValid(DateTime validTo) => validTo > DateTime.UtcNow;
}