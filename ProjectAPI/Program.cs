using ProjectAPI.Mappers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
        builder.WithOrigins("http://127.0.0.1:5500")  // Đặt đúng origin của frontend
                       .AllowCredentials()  // Cho phép gửi credentials (cookies, headers...)
                       .AllowAnyHeader()
                       .AllowAnyMethod());
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;  // Cho phép gửi cookie từ các nguồn khác nhau
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MapperProduct));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin"); // CORS phải được gọi trước session middleware
app.UseSession();


app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
