module CartServiceTests

open System
open System.Collections.Generic
open Xunit
open Foq
open Domain.Data
open Domain.Services
open Domain.Interfaces
open Domain.ServiceErrors

[<Fact>]
let ``При добавлении кота в корзину, если в корзине нет такого же кота, то он будет добавлен в корзину`` () =
    let cartItem = CartItem.Create(Guid.NewGuid(), Guid.NewGuid()).Value
    let cartRepository
        = Mock<ICartRepository>()
            .Setup(fun mock -> <@ mock.FindCartItem(any()) @>).Returns(null)
            .Create()
    let catBehaviour (repository: ICatRepository) = <@ repository.IsCatExists(any() : Guid) --> true @>
    let sut = CartService(cartRepository, Mock.With(catBehaviour))
    
    let result = sut.StoreCartItem(cartItem).IsError
    
    Assert.False(result)
    verify <@ cartRepository.AddCartItem(any()) @> once
    verify <@ cartRepository.SaveChanges() @> once
    
[<Fact>]
let ``При добавлении кота в корзину, если в корзине есть такой же кот, то количество товара у позиции в корзине увеличится`` () =
    let cartItem = CartItem.Create(Guid.NewGuid(), Guid.NewGuid()).Value
    let cartRepository
        = Mock<ICartRepository>()
            .Setup(fun mock -> <@ mock.FindCartItem(any()) @>).Returns(cartItem)
            .Create()
    let catBehaviour (repository: ICatRepository) = <@ repository.IsCatExists(any() : Guid) --> true @>
    let sut = CartService(cartRepository, Mock.With(catBehaviour))
    
    let result = sut.StoreCartItem(cartItem).IsError
    
    Assert.False(result)
    Assert.Equal(2, cartItem.Quantity)
    verify <@ cartRepository.AddCartItem(any()) @> never
    verify <@ cartRepository.SaveChanges() @> once
    
[<Fact>]
let ``В корзину нельзя положить кота, которого нет в каталоге`` () =
    let cartItem = CartItem.Create(Guid.NewGuid(), Guid.NewGuid()).Value
    let cartRepository
        = Mock<ICartRepository>()
            .Setup(fun mock -> <@ mock.FindCartItem(any()) @>).Returns(null)
            .Create()
    let catBehaviour (repository: ICatRepository) = <@ repository.IsCatExists(any() : Guid) --> false @>
    let sut = CartService(cartRepository, Mock.With(catBehaviour))
    
    let result = sut.StoreCartItem(cartItem).FirstError
    
    Assert.Equal(Errors.Cat.NotFound, result)
    verify <@ cartRepository.AddCartItem(any()) @> never
    verify <@ cartRepository.SaveChanges() @> never
    
[<Fact>]
let ``Если пользователь добавил кота в корзину и хочет получить связанную с ним запись корзины, то он получит соответствующую запись элемента корзины`` () =
    let cartItem = CartItem.Create(Guid.NewGuid(), Guid.NewGuid()).Value
    let cartBehaviour (repository: ICartRepository) = <@ repository.GetCartItem(cartItem.UserId, cartItem.CatId) --> cartItem @>
    let catRepository = Mock.Of<ICatRepository>()
    let sut = CartService(Mock.With(cartBehaviour), catRepository)
    
    let result = sut.GetCartItem(cartItem.UserId, cartItem.CatId).IsError
    
    Assert.False(result)
    
[<Fact>]
let ``Если пользователь не добавил кота в корзину и хочет получить связанную с ним запись корзины, то он получит NotFound`` () =
    let cartBehaviour (repository: ICartRepository) = <@ repository.GetCartItem(any(), any()) --> null @>
    let catRepository = Mock.Of<ICatRepository>()
    let sut = CartService(Mock.With(cartBehaviour), catRepository)
    
    let result = sut.GetCartItem(any(), any()).FirstError
    
    Assert.Equal(Errors.CartItem.NotFound, result)
    
[<Fact>]
let ``Сервис вернет элементы корзины покупок при запросе`` () =
    let cartBehaviour (repository: ICartRepository) = <@ repository.GetCartItems(any()) --> List<CartItem>() @>
    let catRepository = Mock.Of<ICatRepository>()
    let sut = CartService(Mock.With(cartBehaviour), catRepository)
    
    let result = sut.GetCartItems(any()).IsError
    
    Assert.False(result)
    
[<Fact>]
let ``При удалении кота из корзины, он будет удален, если действительно находится в корзине`` () =
    let cartRepository
        = Mock<ICartRepository>()
            .Setup(fun mock -> <@ mock.RemoveCartItem(any(), any()) @>).Returns(true)
            .Create()
    let catRepository = Mock.Of<ICatRepository>()
    let sut = CartService(cartRepository, catRepository)
    
    let result = sut.DeleteCartItem(any(), any()).IsError
    
    Assert.False(result)
    verify <@ cartRepository.SaveChanges() @> once
    
[<Fact>]
let ``При удалении кота из корзины, сервис вернет NotFound, если записи о товаре в корзине с такими данными не существует`` () =
    let cartRepository
        = Mock<ICartRepository>()
            .Setup(fun mock -> <@ mock.RemoveCartItem(any(), any()) @>).Returns(false)
            .Create()
    let catRepository = Mock.Of<ICatRepository>()
    let sut = CartService(cartRepository, catRepository)
    
    let result = sut.DeleteCartItem(any(), any()).FirstError
    
    Assert.Equal(Errors.CartItem.NotFound, result)
    verify <@ cartRepository.SaveChanges() @> once
    
[<Fact>]
let ``Количество котов в корзине будет обновлено, при запросе обновления существующего кота из корзины`` () =
    let cartItem = CartItem.Create(Guid.NewGuid(), Guid.NewGuid()).Value
    let cartRepository
        = Mock<ICartRepository>()
            .Setup(fun mock -> <@ mock.FindCartItem(any(), any()) @>).Returns(cartItem)
            .Create()
    let catRepository = Mock.Of<ICatRepository>()
    let sut = CartService(cartRepository, catRepository)
    
    let result = sut.UpdateQuantity(cartItem.CatId, cartItem.UserId, 2).IsError
    
    Assert.False(result)
    Assert.Equal(2, cartItem.Quantity)
    verify <@ cartRepository.SaveChanges() @> once
    
[<Fact>]
let ``Количество котов в корзине не будет обновлено, при запросе обновления несуществующего в корзине кота`` () =
    let cartItem = CartItem.Create(Guid.NewGuid(), Guid.NewGuid()).Value
    let newCartItem = CartItem.Create(cartItem.UserId, cartItem.CatId, 2).Value
    let cartRepository
        = Mock<ICartRepository>()
            .Setup(fun mock -> <@ mock.FindCartItem(newCartItem) @>).Returns(null)
            .Create()
    let catRepository = Mock.Of<ICatRepository>()
    let sut = CartService(cartRepository, catRepository)
    
    let result = sut.UpdateQuantity(Guid.NewGuid(), cartItem.UserId, 2).FirstError
    
    Assert.Equal(Errors.CartItem.NotFound, result)
    verify <@ cartRepository.SaveChanges() @> never
    
[<Fact>]
let ``Сервис передаст пользователю общее количество его товаров в корзине при запросе`` () =
    let behaviour (repository: ICartRepository) = <@ repository.GetUserCartItemsCount(any()) --> 4 @>
    let catRepository = Mock.Of<ICatRepository>()
    let sut = CartService(Mock.With(behaviour), catRepository)
    
    let result = sut.GetUserCartItemsCount(any()).Value
    
    Assert.Equal(4, result)