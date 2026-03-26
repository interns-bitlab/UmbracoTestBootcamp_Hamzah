using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using UmbracoTestBootcamp.Models.Student;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Serialization;
using UmbracoTestBootcamp.Models;

namespace UmbracoTestBootcamp.Services;
public class StudentService(IContentService contentService, IJsonSerializer jsonSerializer)
{
    // BUILD CAR UDI
    // SINGLE CAR
    //private static string? BuildCarUDI(Guid? carId)

    // MULTIPLE CARS
    private static string? BuildCarUDIs(List<Guid>? carId)
    {
        if (carId == null)
            return null;

        // HANDLE SINGLE CAR ONLY
        //var carUdi = Udi.Create(Constants.UdiEntityType.Document, carId.Value);

        //HANDLE MULTIPLE CARS
        var udis = carId.Select(id => Udi.Create(Constants.UdiEntityType.Document, id).ToString());

        return string.Join(",", udis);

        // RETURN SINGLE CAR
        //return carUdi.ToString();
    }

    // CREATE
    public void CreateStudent(StudentDto studentDto)
    {
        if (string.IsNullOrEmpty(studentDto.Name) || string.IsNullOrEmpty(studentDto.Email) || studentDto.Age <= 0)
        {
            throw new ArgumentException("Invalid student data");
        }

        var parentId = Guid.Parse("a66d8900-ba78-44ba-981a-e54a91b9877b");

        var newStudent = contentService.Create(studentDto.Name, parentId, "student");
        newStudent.SetValue("email", studentDto.Email);
        newStudent.SetValue("age", studentDto.Age);

        if (studentDto.DateOfBirth.HasValue)
        {
            //DateOnly dateOnly = DateOnly.FromDateTime(studentDto.DateOfBirth.Value);
            DateTimeOffset dateTimeOffset = studentDto.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue);

            var dateObject = new DateOnlyValue { Date = dateTimeOffset };

            string jsonValue = jsonSerializer.Serialize(dateObject);
            newStudent.SetValue("dateOfBirth", jsonValue);
        }

        var carUdi = BuildCarUDIs(studentDto.StudentsCarId);
 
        newStudent.SetValue("studentSCar", carUdi);
     

        contentService.Save(newStudent);

        contentService.Publish(newStudent, ["*"]);
    }

    // UPDATE PUT
    public void UpdateStudent(Guid id, StudentUpdateDto student)
    {
        var updateStudent = contentService.GetById(id) ?? throw new ArgumentException("Student not found");
        
        updateStudent.Name = student.Name;
        updateStudent.SetValue("email", student.Email);
        updateStudent.SetValue("age", student.Age);

        if (student.DateOfBirth.HasValue)
        {
            //DateOnly dateOnly = DateOnly.FromDateTime(studentDto.DateOfBirth.Value);
            DateTimeOffset dateTimeOffset = student.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue);

            var dateObject = new DateOnlyValue { Date = dateTimeOffset };

            string jsonValue = jsonSerializer.Serialize(dateObject);
            updateStudent.SetValue("dateOfBirth", jsonValue);
        }

        var carUdi = BuildCarUDIs(student.StudentsCarId);
        if (carUdi != null)
        {
            updateStudent.SetValue("studentSCar", carUdi);
        }
        else
        {
            updateStudent.SetValue("studentSCar", null);
        }

        contentService.Save(updateStudent);

        contentService.Publish(updateStudent, ["*"]);
    }

    // UPDATE PATCH
    public void PatchStudent(Guid id, StudentUpdateDto student)
        {
            var patchStudent = contentService.GetById(id) ?? throw new ArgumentException("Student not found");
            
            if (!string.IsNullOrEmpty(student.Name))
            {
                patchStudent.Name = student.Name;
            }
            if (!string.IsNullOrEmpty(student.Email))
            {
                patchStudent.SetValue("email", student.Email);
            }
            if (student.Age.HasValue && student.Age.Value > 0)
            {
                patchStudent.SetValue("age", student.Age.Value);
            }
            if (student.DateOfBirth.HasValue)
            {
            //DateOnly dateOnly = DateOnly.FromDateTime(studentDto.DateOfBirth.Value);
            DateTimeOffset dateTimeOffset = student.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue);

            var dateObject = new DateOnlyValue { Date = dateTimeOffset };

            string jsonValue = jsonSerializer.Serialize(dateObject);
            patchStudent.SetValue("dateOfBirth", jsonValue);
            }
            if (student.StudentsCarId != null)
            {
                var carUdi = BuildCarUDIs(student.StudentsCarId);
                patchStudent.SetValue("studentSCar", carUdi);
            }
           
            contentService.Save(patchStudent);
    
            //var userId = -1;
            contentService.Publish(patchStudent, ["*"]);
    }

    // DELETE
    public void DeleteStudent(Guid id)
    {
        var studentDelete = contentService.GetById(id) ?? throw new ArgumentException("Student not found");

        contentService.Delete(studentDelete);
    }
}
