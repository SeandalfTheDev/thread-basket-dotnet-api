namespace ThreadBasket.Domain.Entities;

public class DmcThread
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Floss { get; set; }
    public string WebColor { get; set; } = "#FFFFFF";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}