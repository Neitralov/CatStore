module UserServiceTests

open System
open Microsoft.Extensions.Configuration
open Xunit
open Foq
open Domain.Data
open Domain.Services
open Domain.Interfaces
open Domain.ServiceErrors

[<Fact>]
let ``Учетная запись с уникальным email должна быть сохранена`` () =
    let user = User.Create("Example@gmail.com", "1234", "1234").Value
    let repository
        = Mock<IUserRepository>()
            .Setup(fun mock -> <@ mock.IsUserExists(any()) @>).Returns(false)
            .Create()
    let configuration = Mock.Of<IConfiguration>()
    let sut = UserService(repository, configuration)
    let result = sut.StoreUser(user).IsError
    Assert.False(result)
    verify <@ repository.AddUser(user) @> once
    verify <@ repository.SaveChanges() @> once
    
[<Fact>]
let ``Нельзя сохранить две учетные записи с одинаковым email`` () =
    let user = User.Create("Example@gmail.com", "1234", "1234").Value
    let repository
        = Mock<IUserRepository>()
            .Setup(fun mock -> <@ mock.IsUserExists(any()) @>).Returns(true)
            .Create()
    let configuration = Mock.Of<IConfiguration>()
    let sut = UserService(repository, configuration)
    let result = sut.StoreUser(user).FirstError
    Assert.Equal(Errors.User.AlreadyExists, result)
    verify <@ repository.AddUser(user) @> never
    verify <@ repository.SaveChanges() @> never
    
[<Fact>]
let ``Пароль существующего в БД пользователя обновится, при корректных данных`` () =
    let user = User.Create("Example@gmail.com", "1234", "1234").Value
    let repository
        = Mock<IUserRepository>()
            .Setup(fun mock -> <@ mock.FindUserById(any()) @>).Returns(user)
            .Create()
    let configuration = Mock.Of<IConfiguration>()
    let sut = UserService(repository, configuration)
    let result = sut.ChangeUserPassword(user.UserId, "1234", "123Q", "123Q").IsError
    Assert.False(result)
    verify <@ repository.SaveChanges() @> once
    
[<Fact>]
let ``Сервис вернет NotFound при попытке обновить пароль несуществующего пользователя`` () =
    let repository
        = Mock<IUserRepository>()
            .Setup(fun mock -> <@ mock.FindUserById(any()) @>).Returns(null)
            .Create()
    let configuration = Mock.Of<IConfiguration>()
    let sut = UserService(repository, configuration)
    let result = sut.ChangeUserPassword(Guid.NewGuid(), "1234", "123Q", "123Q").FirstError
    Assert.Equal(Errors.User.NotFound, result)
    verify <@ repository.SaveChanges() @> never
    
[<Fact>]
let ``Нельзя обновить пароль пользователя, если текущий пароль указан неверно`` () =
    let user = User.Create("Example@gmail.com", "1234", "1234").Value
    let repository
        = Mock<IUserRepository>()
            .Setup(fun mock -> <@ mock.FindUserById(any()) @>).Returns(user)
            .Create()
    let configuration = Mock.Of<IConfiguration>()
    let sut = UserService(repository, configuration)
    let result = sut.ChangeUserPassword(user.UserId, "wrongPassword", "123Q", "123Q").FirstError
    Assert.Equal(Errors.User.IncorrectOldPassword, result)
    verify <@ repository.SaveChanges() @> never
    
[<Fact>]
let ``Нельзя обновить пароль пользователя, если пароли в полях "новый пароль" и "повторите новый пароль" не совпадают`` () =
    let user = User.Create("Example@gmail.com", "1234", "1234").Value
    let repository
        = Mock<IUserRepository>()
            .Setup(fun mock -> <@ mock.FindUserById(any()) @>).Returns(user)
            .Create()
    let configuration = Mock.Of<IConfiguration>()
    let sut = UserService(repository, configuration)
    let result = sut.ChangeUserPassword(user.UserId, "1234", "123Q", "123F").FirstError
    Assert.Equal(Errors.User.PasswordsDontMatch, result)
    verify <@ repository.SaveChanges() @> never
    
[<Fact>]
let ``Нельзя обновить пароль пользователя, если новый пароль совпадает со старым`` () =
    let user = User.Create("Example@gmail.com", "1234", "1234").Value
    let repository
        = Mock<IUserRepository>()
            .Setup(fun mock -> <@ mock.FindUserById(any()) @>).Returns(user)
            .Create()
    let configuration = Mock.Of<IConfiguration>()
    let sut = UserService(repository, configuration)
    let result = sut.ChangeUserPassword(user.UserId, "1234", "1234", "1234").FirstError
    Assert.Equal(Errors.User.NewAndOldPasswordAreTheSame, result)
    verify <@ repository.SaveChanges() @> never
    
[<Fact>]
let ``Пользователь может войти в свой аккаунт, введя данные своей учетной записи`` () =
    let user = User.Create("Example@gmail.com", "1234", "1234").Value
    let repository
        = Mock<IUserRepository>()
            .Setup(fun mock -> <@ mock.FindUserByEmail(any()) @>).Returns(user)
            .Create()
    let behaviour (config: IConfiguration) = <@ config["AppSettings:Token"] --> "My favorite really secret key. 512 bit at least. (64 characters)."  @>
    let sut = UserService(repository, Mock.With(behaviour))
    let result = sut.Login(user.Email, "1234").IsError
    Assert.False(result)
    verify <@ repository.AddRefreshTokenSession(any()) @> once
    verify <@ repository.SaveChanges() @> once
    
[<Fact>]
let ``Пользователь не может войти в свой аккаунт, если укажет неправильный email`` () =
    let repository
        = Mock<IUserRepository>()
            .Setup(fun mock -> <@ mock.FindUserByEmail(any()) @>).Returns(null)
            .Create()
    let configuration = Mock.Of<IConfiguration>()
    let sut = UserService(repository, configuration)
    let result = sut.Login("wrongEmail", "password").FirstError
    Assert.Equal(Errors.Login.IncorrectEmailOrPassword, result)
    verify <@ repository.AddRefreshTokenSession(any()) @> never
    verify <@ repository.SaveChanges() @> never
    
[<Fact>]
let ``Пользователь не может войти в свой аккаунт, если укажет неправильный пароль`` () =
    let user = User.Create("Example@gmail.com", "1234", "1234").Value
    let repository
        = Mock<IUserRepository>()
            .Setup(fun mock -> <@ mock.FindUserByEmail(any()) @>).Returns(user)
            .Create()
    let configuration = Mock.Of<IConfiguration>()
    let sut = UserService(repository, configuration)
    let result = sut.Login("Example@gmail.com", "wrongPassword").FirstError
    Assert.Equal(Errors.Login.IncorrectEmailOrPassword, result)
    verify <@ repository.AddRefreshTokenSession(any()) @> never
    verify <@ repository.SaveChanges() @> never
    
[<Fact>]
let ``Нельзя обновить токены, если у пользователя нет AccessToken`` () =
    let repository = Mock.Of<IUserRepository>()
    let configuration = Mock.Of<IConfiguration>()
    let sut = UserService(repository, configuration)
    let result = sut.RefreshTokens(null, Guid.NewGuid().ToString()).FirstError
    Assert.Equal(Errors.AccessToken.NotFound, result)
    verify <@ repository.SaveChanges() @> never
    
[<Fact>]
let ``Нельзя обновить токены, если у пользователя нет RefreshToken`` () =
    let repository = Mock.Of<IUserRepository>()
    let configuration = Mock.Of<IConfiguration>()
    let sut = UserService(repository, configuration)
    let result = sut.RefreshTokens("AccessToken", null).FirstError
    Assert.Equal(Errors.RefreshToken.NotFound, result)
    verify <@ repository.SaveChanges() @> never