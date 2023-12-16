var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddRouting(options => options.LowercaseUrls = true);
    builder.Services.AddControllers();
    
    builder.Services.AddDbContextFactory<DatabaseContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddTransient<ICatRepository, CatRepository>();
    builder.Services.AddTransient<CatService>();
    builder.Services.AddTransient<IUserRepository, UserRepository>();
    builder.Services.AddTransient<UserService>();
    builder.Services.AddTransient<ICartRepository, CartRepository>();
    builder.Services.AddTransient<CartService>();
    builder.Services.AddTransient<IOrderRepository, OrderRepository>();
    builder.Services.AddTransient<OrderService>();

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
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "CatStore.WebAPI", Version = "0.1.0" } );

        var xmlDocPath = Path.Combine(AppContext.BaseDirectory, "WebAPI.xml");
        options.IncludeXmlComments(xmlDocPath);

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
        options.OperationFilter<SecurityRequirementsOperationFilter>(true, JwtBearerDefaults.AuthenticationScheme);
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