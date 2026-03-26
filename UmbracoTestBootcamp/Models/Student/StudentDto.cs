namespace UmbracoTestBootcamp.Models.Student;

public class StudentDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public List<Guid>? StudentsCarId { get; set; }
}