using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoTestBootcamp.Services;
using UmbracoTestBootcamp.Models.Student;
using Umbraco.Cms.Core;
using UmbracoTestBootcamp.Models.Car;

namespace UmbracoTestBootcamp.Controllers;

[Route("students")]
public class StudentController(UmbracoHelper umbracoHelper, StudentService studentService) : Controller
{

    // VIEWS
    [Route("viewstudents")]
    public IActionResult GetStudent()
    {
        IEnumerable<IPublishedContent> rootNodes = umbracoHelper
            .ContentAtRoot();

        if (rootNodes == null)
            return NotFound();

        var studentsNodes = rootNodes
            .SelectMany(x => x.DescendantsOrSelf())
            .Where(x => x.ContentType.Alias == "student");

        var students = studentsNodes
            .Select(x =>
            {

                var carNodes = x.Value<IEnumerable<IPublishedContent>>("studentsCar");
                //var carNode = carNodes?.FirstOrDefault();

                return new Student
                {
                    Id = x.Key,
                    Name = x.Name ?? "",
                    Email = x.Value<string>("email") ?? "",
                    Age = x.Value<int>("age"),
                    DateOfBirth = x.Value<DateOnly?>("dateOfBirth"),
                    //StudentsCar = carNode != null ? new Car
                    StudentsCar = carNodes?.Select(carNode => new Car
                    {
                        Id = carNode.Key,
                        Name = carNode.Name ?? "",
                        Make = carNode.Value<string>("make") ?? "",
                        Model = carNode.Value<string>("model") ?? "",
                        Year = carNode.Value<int>("year"),
                        Price = carNode.Value<int>("price")
                    }).ToList() ?? []
                };
            })
            .ToList();

            //return Ok(students);
        return View("GetStudent", students);
    }

    // VIEW BY ID
    [Route("viewstudent/{id}")]
    public IActionResult GetStudentById(Guid Id)
    {
        var studentNode = umbracoHelper
            .ContentAtRoot()
            .SelectMany(x => x.DescendantsOrSelf())
            .FirstOrDefault(x => x.ContentType.Alias == "student" && x.Key == Id);

        if (studentNode == null)
            return NotFound();

        var carNodes = studentNode.Value<IEnumerable<IPublishedContent>>("studentsCar");
        //var carNode = carNodes?.FirstOrDefault();

        var students = new Student
        {
            Id = studentNode.Key,
            Name = studentNode.Name ?? "",
            Email = studentNode.Value<string>("email") ?? "",
            Age = studentNode.Value<int>("age"),
            DateOfBirth = studentNode.Value<DateOnly?>("dateOfBirth"),
            //StudentsCar = carNode != null ? new Car
            StudentsCar = carNodes?.Select(carNode => new Car
            {
                Id = carNode.Key,
                Name = carNode.Name ?? "",
                Make = carNode.Value<string>("make") ?? "",
                Model = carNode.Value<string>("model") ?? "",
                Year = carNode.Value<int>("year"),
                Price = carNode.Value<int>("price")
            }).ToList() ?? []
        };

        //return Ok(students);
        return View("GetStudentById", students);
    }


    // CREATE STUDENT
    [HttpPost]
    [Route("createstudent")]
    public IActionResult CreateStudent([FromBody] StudentDto studentCreateDetails)
    {
        studentService.CreateStudent(studentCreateDetails);

        return Ok("Student created successfully!");
    }

    // UPDATE STUDENT
    [HttpPut]
    [Route("updatestudent/{id}")]
    public IActionResult UpdateStudent(Guid Id, [FromBody] StudentUpdateDto studentUpdateDetails)
    {
        studentService.UpdateStudent(Id, studentUpdateDetails);

        return Ok("Student updated successfully!");
    }

    // PATCH STUDENT
    [HttpPatch]
    [Route("patchstudent/{id}")]
    public IActionResult PatchStudent(Guid Id, [FromBody] StudentUpdateDto studentPatchDetails)
    {
        studentService.PatchStudent(Id, studentPatchDetails);

        return Ok("Student patched successfully!");
    }

    // DELETE STUDENT
    [HttpDelete]
    [Route("deletestudent/{id}")]
    public IActionResult DeleteStudent(Guid Id)
    {
        studentService.DeleteStudent(Id);

        return Ok("Student deleted successfully!");
    }
}
