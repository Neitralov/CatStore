namespace Database;

public class CatRepository : ICatRepository
{
    public void AddCat(Cat cat)
    {
        using var db = new LiteDatabase("mydb.db");
        var cats = db.GetCollection<Cat>("cats");
        cats.Insert(cat);
    }

    public Cat? GetCat(Guid catId)
    {
        using var db = new LiteDatabase("mydb.db");
        var cats = db.GetCollection<Cat>("cats");
        return cats.FindById(catId);
    }

    public bool UpdateCat(Cat cat)
    {
        using var db = new LiteDatabase("mydb.db");
        var cats = db.GetCollection<Cat>("cats");
        return cats.Update(cat);
    }

    public bool DeleteCat(Guid id)
    {
        using var db = new LiteDatabase("mydb.db");
        var cats = db.GetCollection<Cat>("cats");
        return cats.Delete(id);
    }
}