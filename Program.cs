using Microsoft.EntityFrameworkCore;
using PrepaidCardApi.Data;
using PrepaidCardApi.Models;

// 禁用 FileSystemWatcher，避免 Linux 容器中的限制問題
Environment.SetEnvironmentVariable("DOTNET_USE_POLLING_FILE_WATCHER", "true");

var builder = WebApplication.CreateBuilder(args);

// 加入 PostgreSQL 資料庫
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PrepaidCardDb")!));

// 加入 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowVueApp");

// 自動建立資料庫
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// 使用 MapGroup 組織路由
var cardsApi = app.MapGroup("/api/cards");

// 取得所有預付卡
cardsApi.MapGet("/", async (AppDbContext db) =>
    await db.Cards.ToListAsync());

// 取得單張預付卡
cardsApi.MapGet("/{id}", async (int id, AppDbContext db) =>
    await db.Cards.FindAsync(id) is PrepaidCard card
        ? Results.Ok(card)
        : Results.NotFound());

// 新增預付卡
cardsApi.MapPost("/", async (PrepaidCard card, AppDbContext db) =>
{
    db.Cards.Add(card);
    await db.SaveChangesAsync();
    return Results.Created($"/api/cards/{card.Id}", card);
});

// 更新預付卡
cardsApi.MapPut("/{id}", async (int id, PrepaidCard updated, AppDbContext db) =>
{
    var card = await db.Cards.FindAsync(id);
    if (card is null) return Results.NotFound();

    card.Name = updated.Name;
    card.ExpiryDate = updated.ExpiryDate;
    await db.SaveChangesAsync();

    return Results.Ok(card);
});

// 刪除預付卡
cardsApi.MapDelete("/{id}", async (int id, AppDbContext db) =>
{
    var card = await db.Cards.FindAsync(id);
    if (card is null) return Results.NotFound();

    db.Cards.Remove(card);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();