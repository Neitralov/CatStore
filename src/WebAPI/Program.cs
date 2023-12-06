var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    
    builder.Services.AddDbContextFactory<DatabaseContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddTransient<ICatRepository, CatRepository>();
    builder.Services.AddTransient<CatService>();
    builder.Services.AddTransient<IAuthRepository, AuthRepository>();
    builder.Services.AddTransient<AuthService>();
    
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
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();    
}