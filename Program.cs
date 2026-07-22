var builder = WebApplication.CreateBuilder(args);

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

// 使用 MapGroup 組織路由
var cardsApi = app.MapGroup("/api/cards");

// 模擬資料庫
var cards = new List<PrepaidCard>
{
    new(1, "中國移動", "2026-08-15"),
    new(2, "3香港", "2026-09-01")
};

// 取得所有預付卡
cardsApi.MapGet("/", () => cards);

// 取得單張預付卡
cardsApi.MapGet("/{id}", (int id) =>
{
    var card = cards.FirstOrDefault(c => c.Id == id);
    return card is not null ? Results.Ok(card) : Results.NotFound();
});

// 新增預付卡
cardsApi.MapPost("/", (PrepaidCard card) =>
{
    var newCard = card with { Id = cards.Count > 0 ? cards.Max(c => c.Id) + 1 : 1 };
    cards.Add(newCard);
    return Results.Created($"/api/cards/{newCard.Id}", newCard);
});

// 更新預付卡
cardsApi.MapPut("/{id}", (int id, PrepaidCard updated) =>
{
    var index = cards.FindIndex(c => c.Id == id);
    if (index == -1) return Results.NotFound();
    
    cards[index] = updated with { Id = id };
    return Results.Ok(cards[index]);
});

// 刪除預付卡
cardsApi.MapDelete("/{id}", (int id) =>
{
    var index = cards.FindIndex(c => c.Id == id);
    if (index == -1) return Results.NotFound();
    
    cards.RemoveAt(index);
    return Results.NoContent();
});

app.Run();

public record PrepaidCard(int Id, string Name, string ExpiryDate);