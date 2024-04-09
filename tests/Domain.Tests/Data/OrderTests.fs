module OrderTests

open System
open System.Collections.Generic
open Xunit
open Domain.Data
open Domain.ServiceErrors

[<Fact>]
let ``Создание заказа с корректными данными не вызывает ошибок`` () =
    let orderItems = List<OrderItem>()
    orderItems.Add(OrderItem.Create(Guid.NewGuid(), "Персик", 1, 25).Value)
    let sut = Order.Create(Guid.NewGuid(), orderItems)
    
    let result = sut.IsError
    
    Assert.False(result)
    
[<Fact>]
let ``Нельзя создать пустой заказ`` () =
    let orderItems = List<OrderItem>()
    let sut = Order.Create(Guid.NewGuid(), orderItems)
    
    let result = sut.FirstError
    
    Assert.Equal(Errors.Order.EmptyOrder, result)