module OrderItemTests

open System
open Xunit
open Domain.Data
open Domain.ServiceErrors

[<Fact>]
let ``Элемент заказа будет создан при корректных значениях`` =
    let cat = Cat.Create("Абрикос", "#ffffff", "#ffffff", "#ffffff", true, 10, 0).Value
    let sut = OrderItem.Create(cat, 1)
    
    let result = sut.IsError
    
    Assert.False(result)
    
[<Fact>]
let ``Нельзя создать элемент заказа с количеством товара меньше одного`` () =
    let cat = Cat.Create("Абрикос", "#ffffff", "#ffffff", "#ffffff", true, 10, 0).Value
    let sut = OrderItem.Create(cat, 0)
    
    let result = sut.FirstError
    
    Assert.Equal(Errors.OrderItem.InvalidQuantity, result)