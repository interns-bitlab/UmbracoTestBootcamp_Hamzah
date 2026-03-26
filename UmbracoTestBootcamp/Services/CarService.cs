using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoTestBootcamp.Controllers;
using UmbracoTestBootcamp.Models.Car;
using Umbraco.Cms.Core.Services;


namespace UmbracoTestBootcamp.Services;
public class CarService(IContentService contentService) : Controller
{

    // CREATE
    public void CreateCar(CarDto car)
    {
        if (string.IsNullOrEmpty(car.Name) || string.IsNullOrEmpty(car.Make) || string.IsNullOrEmpty(car.Model) ||
            car.Year <= 0 || car.Price <= 0)
        {
            throw new ArgumentException("Invalid car data");
        }

        var parentId = Guid.Parse("9480592c-5c00-4d05-913d-212b3fbc95dd");

        var newCar = contentService.Create(car.Name, parentId, "car");

        newCar.SetValue("make", car.Make);
        newCar.SetValue("model", car.Model);
        newCar.SetValue("year", car.Year);
        newCar.SetValue("price", car.Price);

        contentService.Save(newCar);

        contentService.Publish(newCar, ["*"]);
    }

    // UPDATE PUT
    public void UpdateCar(Guid Id, CarUpdateDto car)
    {
        var updateCar = contentService.GetById(Id) ?? throw new ArgumentException("Car not found");

        updateCar.Name = car.Name;
        updateCar.SetValue("make", car.Make);
        updateCar.SetValue ("model", car.Model);
        updateCar.SetValue("year", car.Year);
        updateCar.SetValue("price", car.Price);

        contentService.Save(updateCar);

        contentService.Publish(updateCar, ["*"]);
    }

    // PATCH
    public void PatchCar(Guid Id, CarUpdateDto car)
    {
        var patchCar = contentService.GetById(Id) ?? throw new ArgumentException("Car not found");

        if (!string.IsNullOrEmpty(car.Name))
        {
            patchCar.Name = car.Name;
        }
        if (!string.IsNullOrEmpty(car.Make))
        {
            patchCar.SetValue("make", car.Make);
        }
        if (!string.IsNullOrEmpty(car.Model))
        {
            patchCar.SetValue("model", car.Model);
        }
        if (car.Year.HasValue && car.Year.Value > 0)
        {
            patchCar.SetValue("year", car.Year.Value);
        }
        if (car.Price.HasValue && car.Price.Value > 0)
        {
            patchCar.SetValue("price", car.Price.Value);
        }

        contentService.Save(patchCar);

        contentService.Publish(patchCar, ["*"]);
    }

    // DELETE
    public void DeleteCar(Guid Id)
    {
        var carDelete = contentService.GetById(Id) ?? throw new ArgumentException("Car not found");

        contentService.Delete(carDelete);
    }
}
