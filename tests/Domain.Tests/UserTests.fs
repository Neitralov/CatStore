module UserTests

open Xunit
open Domain.Data
open Domain.ServiceErrors

[<Theory>]
[<InlineData("admin@gmail.com", "admin", "admin")>]
[<InlineData("example@mail.ru", "1234", "1234")>]
let ``Пользователь должен быть создан при корректных значениях`` email password confirmPassword =
    let sut = User.Create(email, password, confirmPassword)
    let result = sut.IsError
    Assert.False(result)
    
[<Fact>]
let ``Нельзя создать пользователя с некорректным email`` () =
    let sut = User.Create("NotAnEmail", "1234", "1234")
    let result = sut.FirstError
    Assert.Equal(Errors.User.InvalidEmail, result)
    
[<Fact>]
let ``Нельзя создать пользователя с коротким паролем`` () =
    let sut = User.Create("example@gmail.com", "123", "123")
    let result = sut.FirstError
    Assert.Equal(Errors.User.InvalidPassword, result)
    
[<Fact>]
let ``Нельзя создать пользователя если пароли в полях не совпадают`` () =
    let sut = User.Create("example@gmail.com", "1234", "123Q")
    let result = sut.FirstError
    Assert.Equal(Errors.User.PasswordsDontMatch, result)
    
[<Fact>]
let ``Корректный пароль пользователя нельзя сменить на короткий`` () =
    let sut = User.Create("example@gmail.com", "1234", "1234")
    let result = sut.Value.ChangePassword("123").FirstError
    Assert.Equal(Errors.User.InvalidPassword, result)