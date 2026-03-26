namespace UmbracoTestBootcamp.Models.Student;

public class StudentUpdateDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public int? Age { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public List<Guid>? StudentsCarId { get; set; }
}