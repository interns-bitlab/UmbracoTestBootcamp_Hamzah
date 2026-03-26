namespace UmbracoTestBootcamp.Models.Student;
using UmbracoTestBootcamp.Models.Car;

public class Student
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public List<Car>? StudentsCar { get; set; } = [];

}



