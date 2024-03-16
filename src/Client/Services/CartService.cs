using System.Net;

namespace Client.Services;

public class CartService(
    HttpClient httpClient,
    ILocalStorageService localStorage,
    UserService userService)
{
    public event Func<Task>? OnChange;
    
    public async Task AddToCart(CreateCartItemRequest request)
    {
        await (await userService.IsUserAuthenticated() ? AddToDbCart() : AddToLocalCart());
        await OnChange?.Invoke()!;

        async Task AddToDbCart() =>
            await httpClient.PostAsJsonAsync("api/cart-items", request);

        async Task AddToLocalCart()
        {
            var localCart = await localStorage.GetItemAsync<List<CartItemResponse>>("LocalCart") ?? new List<CartItemResponse>();

            var sameItemIndex = localCart.FindIndex(cartItem => cartItem.CatId == request.CatId);

            if (sameItemIndex == -1)
                localCart.Add(new CartItemResponse(request.CatId, 1));
            else
                localCart[sameItemIndex] = new CartItemResponse(request.CatId, localCart[sameItemIndex].Quantity + 1);

            await localStorage.SetItemAsync("LocalCart", localCart);
        }
    }

    public async Task<List<CartItemResponse>> GetCartItems()
    {
        return await (await userService.IsUserAuthenticated() ? GetDbCartItems() : GetLocalCartItems());

        async Task<List<CartItemResponse>> GetDbCartItems() =>
            await httpClient.GetFromJsonAsync<List<CartItemResponse>>("api/cart-items") ?? new List<CartItemResponse>();

        async Task<List<CartItemResponse>> GetLocalCartItems() =>
            await localStorage.GetItemAsync<List<CartItemResponse>>("LocalCart") ?? new List<CartItemResponse>();
    }
    
    public async Task<HttpResponseMessage> DeleteCartItem(Guid catId)
    {
        var response = await (await userService.IsUserAuthenticated() ? DeleteDbCartItem() : DeleteLocalCartItem());
        await OnChange?.Invoke()!;
        return response;

        async Task<HttpResponseMessage> DeleteDbCartItem() =>
            await httpClient.DeleteAsync($"/api/cart-items/{catId}");
        
        async Task<HttpResponseMessage> DeleteLocalCartItem()
        {
            var localCart = await localStorage.GetItemAsync<List<CartItemResponse>>("LocalCart") ?? new List<CartItemResponse>();

            var sameItemIndex = localCart.FindIndex(cartItem => cartItem.CatId == catId);

            if (sameItemIndex != -1)
                localCart.RemoveAt(sameItemIndex);

            await localStorage.SetItemAsync("LocalCart", localCart);

            return new HttpResponseMessage(statusCode: HttpStatusCode.NoContent);
        }
    }
    
    public async Task<HttpResponseMessage> UpdateCartItemQuantity(UpdateCartItemQuantityRequest request)
    {
        var response = await (await userService.IsUserAuthenticated() ? UpdateDbCartItemQuantity() : UpdateLocalCartItemQuantity());
        await OnChange?.Invoke()!;
        return response;
        
        async Task<HttpResponseMessage> UpdateDbCartItemQuantity() =>
            await httpClient.PatchAsJsonAsync("/api/cart-items/update-quantity", request);

        async Task<HttpResponseMessage> UpdateLocalCartItemQuantity()
        {
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
    }
    
    public async Task<int> GetCartItemsQuantity()
    {
        return await (await userService.IsUserAuthenticated() ? GetDbCartItemsQuantity() : GetLocalCartItemsQuantity());
        
        async Task<int> GetDbCartItemsQuantity()
        {
            var response = await httpClient.GetFromJsonAsync<TotalNumberOfCartItemsResponse>("/api/cart-items/count");
            return response?.Count ?? 0;
        }

        async Task<int> GetLocalCartItemsQuantity()
        {
            var localCart = await localStorage.GetItemAsync<List<CartItemResponse>>("LocalCart") ?? new List<CartItemResponse>();
            return localCart.Sum(item => item.Quantity);
        }
    }
    
    public async Task StoreCartItemsFromLocalCart()
    {
        var localCart = await localStorage.GetItemAsync<List<CartItemResponse>>("LocalCart");
        
        if (localCart is null)
            return;

        foreach (var cartItem in localCart)
        {
            //TODO: Расширить API, чтобы можно было указывать количество товара при создании записи в БД
            var createRequest = new CreateCartItemRequest(cartItem.CatId);
            await httpClient.PostAsJsonAsync("api/cart-items", createRequest);
            var updateRequest = new UpdateCartItemQuantityRequest(cartItem.CatId, cartItem.Quantity);
            await httpClient.PatchAsJsonAsync("/api/cart-items/update-quantity", updateRequest);
        }
        
        await localStorage.RemoveItemAsync("LocalCart");
    }
}