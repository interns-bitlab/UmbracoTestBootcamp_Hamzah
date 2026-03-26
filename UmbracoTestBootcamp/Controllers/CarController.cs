using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoTestBootcamp.Services;
using UmbracoTestBootcamp.Models.Car;

namespace UmbracoTestBootcamp.Controllers;

[Route("cars")]
public class CarController(UmbracoHelper umbracoHelper, CarService carService) : Controller
{

    // VIEWS
    [Route("viewcars")]
    public IActionResult GetCar()
    {
        IEnumerable<IPublishedContent> rootNodes = umbracoHelper
            .ContentAtRoot();

        if (rootNodes == null)
            return NotFound();

        var carsNodes = rootNodes
            .SelectMany(x => x.DescendantsOrSelf())
            .Where(x => x.ContentType.Alias == "car");

        var cars = carsNodes
            .Select(x => new Car
            {
                Id = x.Key,
                Name = x.Name ?? "",
                Make = x.Value<string>("make") ?? "",
                Model = x.Value<string>("model") ?? "",
                Year = x.Value<int>("year"),
                Price = x.Value<int>("price")
            })
            .ToList();

        return Ok(cars);
        //return View("CarContent/GetCar", cars);
    }

    // VIEW BY ID
    [Route("viewcar/{id}")]
    public IActionResult GetCarById(Guid Id)
    {
        var carNode = umbracoHelper
            .ContentAtRoot()
            .SelectMany(x => x.DescendantsOrSelf())
            .FirstOrDefault(x => x.ContentType.Alias == "car" && x.Key == Id);

        if (carNode == null)
            return NotFound();

        var cars = new Car
        {
            Id = carNode.Key,
            Name = carNode.Name ?? "",
            Make = carNode.Value<string>("make") ?? "",
            Model = carNode.Value<string>("model") ?? "",
            Year = carNode.Value<int>("year"),
            Price = carNode.Value<int>("price")
        };

        //return Ok(studentNode);
        return View("CarContent/GetCarById", cars);
    }

    // CREATE CAR
    [HttpPost]
    [Route("createcar")]
    public IActionResult CreateCar([FromBody] CarDto car)
    {
        carService.CreateCar(car);

        return Ok("Car created successfully");
    }

    // UPDATE CAR
    [HttpPut]
    [Route("updatecar/{id}")]
    public IActionResult UpdateCar(Guid Id, [FromBody] CarUpdateDto car)
    {
        carService.UpdateCar(Id, car);
        return Ok("Car updated successfully");
    }

    // PATCH CAR
    [HttpPatch]
    [Route("patchcar/{id}")]
    public IActionResult PatchCar(Guid Id, [FromBody] CarUpdateDto car)
    {
        carService.PatchCar(Id, car);
        return Ok("Car patched successfully");
    }

    // DELETE CAR
    [HttpDelete]
    [Route("deletecar/{id}")]
    public IActionResult DeleteCar(Guid Id) {
        carService.DeleteCar(Id);
        return Ok("Car deleted successfully");
    }

}
