module OrderItemTests

open System
open Xunit
open Domain.Data
open Domain.ServiceErrors

[<Theory>]
[<InlineData("Персик", 1, 25)>]
[<InlineData("Снежинка", 4, 120)>]
let ``Элемент заказа должен быть создан при корректных значениях`` name quantity totalPrice =
    let sut = OrderItem.Create(Guid.NewGuid(), name, quantity, totalPrice)
    let result = sut.IsError
    Assert.False(result)
    
[<Fact>]
let ``Нельзя создать элемент заказа с количеством товара меньше одного`` () =
    let sut = OrderItem.Create(Guid.NewGuid(), "Персик", 0, 25)
    let result = sut.FirstError
    Assert.Equal(Errors.OrderItem.InvalidQuantity, result)
    
[<Fact>]
let ``Нельзя создать бесплатный элемент заказа`` () =
    let sut = OrderItem.Create(Guid.NewGuid(), "Персик", 1, 0)
    let result = sut.FirstError
    Assert.Equal(Errors.OrderItem.InvalidPrice, result)