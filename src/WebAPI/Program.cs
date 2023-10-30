var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    
    builder.Services.AddDbContextFactory<DatabaseContext>(options =>
        options.UseSqlite(builder.Configuration["ConnectionString"]));
    builder.Services.AddTransient<ICatRepository, CatRepository>();
    builder.Services.AddTransient<CatService>();
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();    
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();    
}