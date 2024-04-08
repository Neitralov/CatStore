module CartItemTests

open System
open Xunit
open Domain.Data
open Domain.ServiceErrors

[<Theory>]
[<InlineData(1)>]
[<InlineData(10)>]
let ``Элемент корзины должен быть создан при корректных значениях`` value =
    let sut = CartItem.Create(Guid.NewGuid(), Guid.NewGuid(), value)
    
    let result = sut.IsError
    
    Assert.False(result)

[<Fact>]
let ``Нельзя создать элемент корзины с количеством товара меньше одного`` () =
    let sut = CartItem.Create(Guid.NewGuid(), Guid.NewGuid(), 0)
    
    let result = sut.FirstError
    
    Assert.Equal(Errors.CartItem.InvalidQuantity, result)
    
[<Fact>]
let ``Нельзя обновить количество товаров в элементе корзины так, чтобы количество товара оказалось меньше единицы`` () =
    let sut = CartItem.Create(Guid.NewGuid(), Guid.NewGuid())
    
    let result = sut.Value.UpdateQuantity(0).FirstError
    
    Assert.Equal(Errors.CartItem.InvalidQuantity, result)
    
[<Fact>]
let ``Количество товара в элементе корзины можно изменить на произвольное число не меньше единицы`` () =
    let sut = CartItem.Create(Guid.NewGuid(), Guid.NewGuid())
    
    sut.Value.UpdateQuantity(4) |> ignore
    let result = sut.Value.Quantity
    
    Assert.Equal(4, result)
    
[<Fact>]
let ``Количество товара в элементе корзины можно увеличить на единицу`` () =
    let sut = CartItem.Create(Guid.NewGuid(), Guid.NewGuid())
    
    sut.Value.IncreaseQuantity()
    let result = sut.Value.Quantity
    
    Assert.Equal(2, result)