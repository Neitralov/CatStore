module CatServiceTests

open System
open Xunit
open Foq
open Domain.Data
open Domain.Services
open Domain.Interfaces
open Domain.ServiceErrors

[<Fact>]
let ``Коты с разными названиями должны сохраняться`` () =
    let cat = Cat.Create("Абрикос", "#ffffff", "#ffffff", "#ffffff", true, 10, 0).Value
    let repository
        = Mock<ICatRepository>()
            .Setup(fun mock -> <@ mock.IsCatExists("Персик") @>).Returns(true)
            .Create()
    let sut = CatService(repository)
    
    let result = sut.StoreCat(cat).IsError
    
    Assert.False(result)
    verify <@ repository.AddCat(cat) @> once
    verify <@ repository.SaveChanges() @> once
    
[<Fact>]
let ``Нельзя сохранить двух котов с одинаковыми названиями`` () =
    let cat = Cat.Create("Персик", "#ffffff", "#ffffff", "#ffffff", true, 10, 0).Value
    let repository
        = Mock<ICatRepository>()
            .Setup(fun mock -> <@ mock.IsCatExists("Персик") @>).Returns(true)
            .Create()
    let sut = CatService(repository)
    
    let result = sut.StoreCat(cat).FirstError
    
    Assert.Equal(Errors.Cat.AlreadyExists, result)
    verify <@ repository.AddCat(cat) @> never
    verify <@ repository.SaveChanges() @> never
    
[<Fact>]
let ``Сервис найдет кота по ID, если он находится в БД`` () =
    let cat = Cat.Create("Персик", "#ffffff", "#ffffff", "#ffffff", true, 10, 0).Value
    let behaviour (repository: ICatRepository) = <@ repository.GetCat(cat.CatId) --> cat @>
    let sut = CatService(Mock.With(behaviour))
    
    let result = sut.GetCat(cat.CatId).IsError
    
    Assert.False(result)
    
[<Fact>]
let ``Сервис вернет NotFound при попытке получить кота по ID, если его нет в БД`` () =
    let behaviour (repository: ICatRepository) = <@ repository.GetCat(any()) --> null @>
    let sut = CatService(Mock.With(behaviour))
    
    let result = sut.GetCat(Guid.NewGuid()).FirstError
    
    Assert.Equal(Errors.Cat.NotFound, result)
    
[<Fact>]
let ``Сервис вернет список всех котов из БД по запросу`` () =
    let behaviour (repository: ICatRepository) = <@ repository.GetCats() --> [] @>
    let sut = CatService(Mock.With(behaviour))
    
    let result = sut.GetCats().IsError
    
    Assert.False(result)
    
[<Fact>]
let ``Сервис обновит цену кота, если он есть в БД`` () =
    let cat = Cat.Create("Персик", "#ffffff", "#ffffff", "#ffffff", true, 10, 0).Value
    let repository
        = Mock<ICatRepository>()
            .Setup(fun mock -> <@ mock.FindCatById(cat.CatId) @>).Returns(cat)
            .Create()
    let sut = CatService(repository)
    
    let result = sut.UpdateCatPrice(cat.CatId, 15, 5).IsError
    
    Assert.False(result)
    Assert.Equal<decimal>(15m, cat.Cost)
    Assert.Equal<decimal>(5m, cat.Discount)
    verify <@ repository.SaveChanges() @> once
    
[<Fact>]
let ``Сервис вернет NotFound при попытке обновить цену кота, которого нет в БД`` () =
    let repository
        = Mock<ICatRepository>()
            .Setup(fun mock -> <@ mock.FindCatById(any()) @>).Returns(null)
            .Create()
    let sut = CatService(repository)
    
    let result = sut.UpdateCatPrice(Guid.NewGuid(), 15, 5).FirstError
    
    Assert.Equal(Errors.Cat.NotFound, result)
    verify <@ repository.SaveChanges() @> never
    
[<Fact>]
let ``Сервис удалит кота с соответствующим ID, если он есть в БД`` () =
    let repository
        = Mock<ICatRepository>()
            .Setup(fun mock -> <@ mock.RemoveCat(any()) @>).Returns(true)
            .Create()
    let sut = CatService(repository)
    
    let result = sut.DeleteCat(Guid.NewGuid()).IsError
    
    Assert.False(result)
    verify <@ repository.SaveChanges() @> once
    
[<Fact>]
let ``Сервис вернет NotFound при попытке удалить кота, которого нет в БД`` () =
    let repository
        = Mock<ICatRepository>()
            .Setup(fun mock -> <@ mock.RemoveCat(any()) @>).Returns(false)
            .Create()
    let sut = CatService(repository)
    
    let result = sut.DeleteCat(Guid.NewGuid()).FirstError
    
    Assert.Equal(Errors.Cat.NotFound, result)