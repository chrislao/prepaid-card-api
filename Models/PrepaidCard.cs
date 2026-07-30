namespace PrepaidCardApi.Models;

public class PrepaidCard
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string ExpiryDate { get; set; } = string.Empty;
}