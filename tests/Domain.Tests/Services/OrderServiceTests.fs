module OrderServiceTests

open System
open System.Collections.Generic
open Xunit
open Foq
open Domain.Data
open Domain.Services
open Domain.Interfaces
open Domain.ServiceErrors

[<Fact>]
let ``Успешное оформление заказа очищает корзину пользователя`` () =
    let cat = Cat.Create("Абрикос", "#ffffff", "#ffffff", "#ffffff", true, 10, 0).Value
    let orderItems = List<OrderItem>()
    orderItems.Add(OrderItem.Create(cat, 1).Value)
    let order = Order.Create(Guid.NewGuid(), orderItems).Value
    let orderRepository = Mock.Of<IOrderRepository>()
    let catRepository = Mock.Of<ICatRepository>()
    let cartRepository
        = Mock<ICartRepository>()
            .Setup(fun stub -> <@ stub.GetCartItems(any()) @>).Returns(List<CartItem>())
            .Setup(fun mock -> <@ mock.RemoveCartItems(any()) @>).Returns(true)
            .Create()
    let sut = OrderService(orderRepository, cartRepository, catRepository)
    
    let result = sut.StoreOrder(order).IsError
    
    Assert.False(result)
    verify <@ orderRepository.AddOrder(any()) @> once
    verify <@ orderRepository.SaveChanges() @> once
    verify <@ cartRepository.SaveChanges() @> once
    
[<Fact>]
let ``Пользователь получит данные о заказе, если он оформлял его`` () =
    let userId = Guid.NewGuid()
    let orderItems = List<OrderItem>()
    let cat = Cat.Create("Абрикос", "#ffffff", "#ffffff", "#ffffff", true, 10, 0).Value
    orderItems.Add(OrderItem.Create(cat, 2).Value)
    let order = Order.Create(userId, orderItems).Value
    let cartRepository = Mock.Of<ICartRepository>()
    let catRepository = Mock.Of<ICatRepository>()
    let behaviour (repository: IOrderRepository) = <@ repository.GetOrder(any(), userId) --> order @>
    let sut = OrderService(Mock.With(behaviour), cartRepository, catRepository)
    
    let result = sut.GetOrder(any(), userId).IsError
    
    Assert.False(result)
    
[<Fact>]
let ``Пользователь не получит данные о заказе, если заказ не принадлежит ему`` () =
    let userId = Guid.NewGuid()
    let orderItems = List<OrderItem>()
    let cat = Cat.Create("Абрикос", "#ffffff", "#ffffff", "#ffffff", true, 10, 0).Value
    orderItems.Add(OrderItem.Create(cat, 2).Value)
    let order = Order.Create(userId, orderItems).Value
    let cartRepository = Mock.Of<ICartRepository>()
    let catRepository = Mock.Of<ICatRepository>()
    let behaviour (repository: IOrderRepository) = <@ repository.GetOrder(any(), userId) --> order @>
    let sut = OrderService(Mock.With(behaviour), cartRepository, catRepository)
    
    let result = sut.GetOrder(any(), Guid.NewGuid()).FirstError
    
    Assert.Equal(Errors.Order.NotFound, result)
    
[<Fact>]
let ``Пользователь не получит данные о заказе, если такого заказа не существует`` () =
    let cartRepository = Mock.Of<ICartRepository>()
    let catRepository = Mock.Of<ICatRepository>()
    let behaviour (repository: IOrderRepository) = <@ repository.GetOrder(any(), any()) --> null @>
    let sut = OrderService(Mock.With(behaviour), cartRepository, catRepository)
    
    let result = sut.GetOrder(any(), any()).FirstError
    
    Assert.Equal(Errors.Order.NotFound, result)
    
[<Fact>]
let ``Сервис предоставит список всех заказов при запросе`` () =
    let cartRepository = Mock.Of<ICartRepository>()
    let catRepository = Mock.Of<ICatRepository>()
    let behaviour (repository: IOrderRepository) = <@ repository.GetOrders(any()) --> List<Order>() @>
    let sut = OrderService(Mock.With(behaviour), cartRepository, catRepository)
    
    let result = sut.GetOrders(any())
    
    Assert.True(result <> null)