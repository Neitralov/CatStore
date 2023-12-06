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

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                    .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };
        });

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "CatStore.WebAPI", Version = "v1" } );

        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter JWT token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = JwtBearerDefaults.AuthenticationScheme,
        };

        options.AddSecurityDefinition(jwtSecurityScheme.Scheme, jwtSecurityScheme);
        options.OperationFilter<SecurityRequirementsOperationFilter>(true, "Bearer");
    });
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