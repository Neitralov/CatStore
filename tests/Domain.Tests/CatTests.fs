module CatTests

open Xunit
open Domain.Data
open Domain.ServiceErrors

[<Theory>]
[<InlineData("Персик", "#ffffff", "#ffffff", "#ffffff", true, 10, 5)>]
[<InlineData("Ком", "#000000", "#000000", "#000000", true, 1, 0)>]
[<InlineData("Пятнадцать букв", "#afe3e3", "#c0aa14", "#1b1b4b", false, 50, 49)>]
let ``Кот должен быть создан при корректных значениях`` (name, skinColor, eyeColor, earColor, isMale, cost, discount) =
    let sut = Cat.Create(name, skinColor, eyeColor, earColor, isMale, cost, discount)
    let result = sut.IsError
    Assert.False(result)

[<Theory>]
[<InlineData("")>]
[<InlineData("      ")>]
[<InlineData("Аа")>]
[<InlineData("Aaaaaaaaaaaaaaaa")>]
let ``Нельзя создать кота с пустым, коротким или слишком длинным именем`` (value) =
    let sut = Cat.Create(value, "#ffffff", "#ffffff", "#ffffff", true, 10, 0)
    let result = sut.FirstError
    Assert.Equal(Errors.Cat.InvalidName, result)

[<Fact>]
let ``Нельзя создать кота с некорректным цветом шерсти`` () =
    let sut = Cat.Create("Персик", "invalidHEX", "#ffffff", "#ffffff", true, 10, 0)
    let result = sut.FirstError
    Assert.Equal(Errors.Cat.InvalidSkinColor, result)

[<Fact>]
let ``Нельзя создать кота с некорректным цветом глаз`` () =
    let sut = Cat.Create("Персик", "#ffffff", "invalidHEX", "#ffffff", true, 10, 0)
    let result = sut.FirstError
    Assert.Equal(Errors.Cat.InvalidEyeColor, result)
    
[<Fact>]
let ``Нельзя создать кота с некорректным цветом ушей`` () =
    let sut = Cat.Create("Персик", "#ffffff", "#ffffff", "invalidHEX", true, 10, 0)
    let result = sut.FirstError
    Assert.Equal(Errors.Cat.InvalidEarColor, result)
    
[<Fact>]
let ``Нельзя создать бесплатного кота`` () =
    let sut = Cat.Create("Персик", "#ffffff", "#ffffff", "#ffffff", true, 0, 0)
    let result = sut.FirstError
    Assert.Equal(Errors.Cat.InvalidCost, result)
    
[<Fact>]
let ``Скидка не может составлять 100% и более`` () =
    let sut = Cat.Create("Персик", "#ffffff", "#ffffff", "#ffffff", true, 10, 10)
    let result = sut.FirstError
    Assert.Equal(Errors.Cat.InvalidCost, result)
    
[<Fact>]
let ``Скидка не может быть отрицательной`` () =
    let sut = Cat.Create("Персик", "#ffffff", "#ffffff", "#ffffff", true, 10, -10)
    let result = sut.FirstError
    Assert.Equal(Errors.Cat.InvalidDiscount, result)