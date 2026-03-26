namespace UmbracoTestBootcamp.Models.Car;

public class Car
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Price { get; set; }
}