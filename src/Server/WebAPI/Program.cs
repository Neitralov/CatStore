var loggerConfiguration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(loggerConfiguration)
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);
    {
        builder.Host.UseSerilog((_, _, configuration) => configuration
            .ReadFrom.Configuration(loggerConfiguration));

        builder.Services.AddCors(
            options => options.AddPolicy("AllowCatStoreApp", policy =>
                policy.WithOrigins(builder.Configuration["FrontendUrl"] ?? throw new NullReferenceException("env variable FrontendUrl is not defined"))
                    .AllowAnyHeader()
                    .AllowAnyMethod()));

        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddControllers();
        builder.Services.AddHealthChecks();
        
        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddHttpClient("kassa", (client) =>
        {
            client.BaseAddress = new Uri("https://api.yookassa.ru");

            var yooKassaId = builder.Configuration["Yookassa:Id"] ?? throw new NullReferenceException("env variable Yookassa:Id is not defined");
            var yooKassaToken = builder.Configuration["Yookassa:Token"] ?? throw new NullReferenceException("env variable Yookassa:Token is not defined");
            var byteArray = new UTF8Encoding().GetBytes($"{yooKassaId}:{yooKassaToken}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        });
        
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
                        .GetBytes(builder.Configuration["AppSettings:Token"] ?? throw new NullReferenceException("env variable AppSettings:Token is not defined"))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "CatStore.WebAPI", Version = "1.0" } );

            var xmlDocPaths = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
            xmlDocPaths.ForEach(xmlDocPath => options.IncludeXmlComments(xmlDocPath));

            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter JWT token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = JwtBearerDefaults.AuthenticationScheme
            };

            options.AddSecurityDefinition(jwtSecurityScheme.Scheme, jwtSecurityScheme);
            options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            options.OperationFilter<SecurityRequirementsOperationFilter>(true, JwtBearerDefaults.AuthenticationScheme);
        });

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(
                name: nameof(User.CanEditCats), 
                configurePolicy: policyBuilder => policyBuilder.RequireClaim(nameof(User.CanEditCats), true.ToString()));
    }

    var app = builder.Build();
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/error");
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowCatStoreApp");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHealthChecks("/health");
        app.ApplyTypeAdapterConfigs();
        await app.MigrateDatabaseAsync();
        await app.RunAsync();
    }
}
catch (Exception exception)
{
    Log.Fatal(exception, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}