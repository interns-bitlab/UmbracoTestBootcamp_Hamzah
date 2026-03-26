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
    public void CreateStudent(StudentDto studentCreateDetails)
    {
        if (string.IsNullOrEmpty(studentCreateDetails.Name) || string.IsNullOrEmpty(studentCreateDetails.Email) || studentCreateDetails.Age <= 0)
        {
            throw new ArgumentException("Invalid student data");
        }

        var parentId = Guid.Parse("a66d8900-ba78-44ba-981a-e54a91b9877b");

        var student = contentService.Create(studentCreateDetails.Name, parentId, "student");
        student.SetValue("email", studentCreateDetails.Email);
        student.SetValue("age", studentCreateDetails.Age);

        if (studentCreateDetails.DateOfBirth.HasValue)
        {
            //DateOnly dateOnly = DateOnly.FromDateTime(studentDto.DateOfBirth.Value);
            DateTimeOffset dateTimeOffset = studentCreateDetails.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue);

            var dateObject = new DateOnlyValue { Date = dateTimeOffset };

            string jsonValue = jsonSerializer.Serialize(dateObject);
            student.SetValue("dateOfBirth", jsonValue);
        }

        var carUdi = BuildCarUDIs(studentCreateDetails.StudentsCarId);
 
        student.SetValue("studentsCar", carUdi);
     

        contentService.Save(student);

        contentService.Publish(student, ["*"]);
    }

    // UPDATE PUT
    public void UpdateStudent(Guid id, StudentUpdateDto studentUpdateDetails)
    {
        var student = contentService.GetById(id) ?? throw new ArgumentException("Student not found");
        
        if (!string.IsNullOrEmpty(studentUpdateDetails.Name))
        {
            student.Name = studentUpdateDetails.Name;
        }

        if (!string.IsNullOrEmpty(studentUpdateDetails.Email))
        {
            student.SetValue("email", studentUpdateDetails.Email);
        }

        if (studentUpdateDetails.Age.HasValue && studentUpdateDetails.Age.Value > 0)
        {
            student.SetValue("age", studentUpdateDetails.Age);
        }

        if (studentUpdateDetails.DateOfBirth.HasValue)
        {
            //DateOnly dateOnly = DateOnly.FromDateTime(studentDto.DateOfBirth.Value);
            DateTimeOffset dateTimeOffset = studentUpdateDetails.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue);

            var dateObject = new DateOnlyValue { Date = dateTimeOffset };

            string jsonValue = jsonSerializer.Serialize(dateObject);
            student.SetValue("dateOfBirth", jsonValue);
        }

        var carUdi = BuildCarUDIs(studentUpdateDetails.StudentsCarId);
        if (carUdi != null)
        {
            student.SetValue("studentsCar", carUdi);
        }
        else
        {
            student.SetValue("studentsCar", null);
        }

        contentService.Save(student);

        contentService.Publish(student, ["*"]);
    }

    // UPDATE PATCH
    public void PatchStudent(Guid id, StudentUpdateDto studentPatchDetails)
        {
            var student = contentService.GetById(id) ?? throw new ArgumentException("Student not found");
            
            if (!string.IsNullOrEmpty(studentPatchDetails.Name))
            {
                student.Name = studentPatchDetails.Name;
            }

            if (!string.IsNullOrEmpty(studentPatchDetails.Email))
            {
                student.SetValue("email", studentPatchDetails.Email);
            }

            if (studentPatchDetails.Age.HasValue && studentPatchDetails.Age.Value > 0)
            {
                student.SetValue("age", studentPatchDetails.Age.Value);
            }

            if (studentPatchDetails.DateOfBirth.HasValue)
            {
            //DateOnly dateOnly = DateOnly.FromDateTime(studentDto.DateOfBirth.Value);
            DateTimeOffset dateTimeOffset = studentPatchDetails.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue);

            var dateObject = new DateOnlyValue { Date = dateTimeOffset };

            string jsonValue = jsonSerializer.Serialize(dateObject);
            student.SetValue("dateOfBirth", jsonValue);
            }
            if (studentPatchDetails.StudentsCarId != null)
            {
                var carUdi = BuildCarUDIs(studentPatchDetails.StudentsCarId);
                student.SetValue("studentsCar", carUdi);
            }
           
            contentService.Save(student);
    
            //var userId = -1;
            contentService.Publish(student, ["*"]);
    }

    // DELETE
    public void DeleteStudent(Guid id)
    {
        var studentDelete = contentService.GetById(id) ?? throw new ArgumentException("Student not found");

        contentService.Delete(studentDelete);
    }
}
