using System.Net;

namespace Client.Services;

public class CartService(
    HttpClient httpClient,
    ILocalStorageService localStorage,
    AuthenticationStateProvider authStateProvider)
{
    public event Func<Task>? OnChange;
    
    public async Task AddToCart(CreateCartItemRequest request)
    {
        var authState = await authStateProvider.GetAuthenticationStateAsync();

        if (authState.User.Identity!.IsAuthenticated)
            await httpClient.PostAsJsonAsync("api/cart-items", request);
        else
        {
            var localCart = await localStorage.GetItemAsync<List<CartItemResponse>>("LocalCart") ?? new List<CartItemResponse>();

            var sameItemIndex = localCart.FindIndex(cartItem => cartItem.CatId == request.CatId);

            if (sameItemIndex == -1)
                localCart.Add(new CartItemResponse(request.CatId, 1));
            else
                localCart[sameItemIndex] = new CartItemResponse(request.CatId, localCart[sameItemIndex].Quantity + 1);

            await localStorage.SetItemAsync("LocalCart", localCart);
        }
        
        await OnChange?.Invoke()!;
    }
    
    public async Task<List<CartItemResponse>> GetCartItems()
    {
        var authState = await authStateProvider.GetAuthenticationStateAsync();

        if (authState.User.Identity!.IsAuthenticated)
            return await httpClient.GetFromJsonAsync<List<CartItemResponse>>("api/cart-items") ?? new List<CartItemResponse>();
        
        return await localStorage.GetItemAsync<List<CartItemResponse>>("LocalCart") ?? new List<CartItemResponse>();
    }
    
    public async Task<HttpResponseMessage> DeleteCartItem(Guid catId)
    {
        var authState = await authStateProvider.GetAuthenticationStateAsync();
        
        if (authState.User.Identity!.IsAuthenticated)
        {
            var response = await httpClient.DeleteAsync($"/api/cart-items/{catId}");
            await OnChange?.Invoke()!;
            return response;
        }
        
        var localCart = await localStorage.GetItemAsync<List<CartItemResponse>>("LocalCart") ?? new List<CartItemResponse>();

        var sameItemIndex = localCart.FindIndex(cartItem => cartItem.CatId == catId);

        if (sameItemIndex != -1)
            localCart.RemoveAt(sameItemIndex);

        await localStorage.SetItemAsync("LocalCart", localCart);
        await OnChange?.Invoke()!;
        
        return new HttpResponseMessage(statusCode: HttpStatusCode.NoContent);
    }
    
    public async Task<HttpResponseMessage> UpdateCartItemsQuantity(UpdateCartItemQuantityRequest request)
    {
        var authState = await authStateProvider.GetAuthenticationStateAsync();
        
        if (authState.User.Identity!.IsAuthenticated)
        {
            var response = await httpClient.PatchAsJsonAsync("/api/cart-items/update-quantity", request);
            await OnChange?.Invoke()!;
            return response;
        }
        
        var localCart = await localStorage.GetItemAsync<List<CartItemResponse>>("LocalCart") ?? new List<CartItemResponse>();

        var sameItemIndex = localCart.FindIndex(cartItem => cartItem.CatId == request.CatId);

        if (request.Quantity < 1)
            return new HttpResponseMessage(statusCode: HttpStatusCode.BadRequest);
        
        if (sameItemIndex != -1)
            localCart[sameItemIndex] = new CartItemResponse(request.CatId, request.Quantity);

        await localStorage.SetItemAsync("LocalCart", localCart);
        await OnChange?.Invoke()!;
        
        return new HttpResponseMessage(statusCode: HttpStatusCode.NoContent);
    }
    
    public async Task<int> GetCartItemsCount()
    {
        var authState = await authStateProvider.GetAuthenticationStateAsync();
        
        if (authState.User.Identity!.IsAuthenticated)
        {
            var response = await httpClient.GetFromJsonAsync<TotalNumberOfCartItemsResponse>("/api/cart-items/count");
            return response?.Count ?? 0;
        }
        
        var localCart = await localStorage.GetItemAsync<List<CartItemResponse>>("LocalCart") ?? new List<CartItemResponse>();
        return localCart.Sum(item => item.Quantity);
    }
    
    public async Task StoreCartItemsFromLocalCart()
    {
        var localCart = await localStorage.GetItemAsync<List<CartItemResponse>>("LocalCart");
        
        if (localCart is null)
            return;

        foreach (var cartItem in localCart)
        {
            var createRequest = new CreateCartItemRequest(cartItem.CatId);
            await httpClient.PostAsJsonAsync("api/cart-items", createRequest);
            var updateRequest = new UpdateCartItemQuantityRequest(cartItem.CatId, cartItem.Quantity);
            await httpClient.PatchAsJsonAsync("/api/cart-items/update-quantity", updateRequest);
        }
        
        await localStorage.RemoveItemAsync("LocalCart");
    }
}