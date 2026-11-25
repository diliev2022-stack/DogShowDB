
using Microsoft.AspNetCore.Mvc;
using WebDogApp.Core.Contracts;
using WebDogApp.Models.Dog;
using WebDogApp.Models.Breed;
using System.Linq;

namespace WebDogApp.Controllers
{
    public class DogController : Controller
    {
        private readonly IDogService _dogService;
        private readonly IBreedService _breedService;

        public DogController(IDogService dogService, IBreedService breedService)
        {
            _dogService = dogService;
            _breedService = breedService;
        }

        public IActionResult Index()
        {
            var dogs = _dogService.GetDogs();

            var model = dogs.Select(d => new IndexVM
            {
                Id = d.Id,
                Name = d.Name,
                Age = d.Age,
                BreedName = d.Breed.Name,
                Picture = d.Picture
            }).ToList();

            return View(model);
        }

        public IActionResult Create()
        {
            var model = new CreateVM
            {
                Breeds = _breedService.GetBreeds()
                    .Select(b => new BreedPairViewModel
                    {
                        Id = b.Id,
                        Name = b.Name
                    }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Breeds = _breedService.GetBreeds()
                    .Select(b => new BreedPairViewModel
                    {
                        Id = b.Id,
                        Name = b.Name
                    }).ToList();

                return View(model);
            }

            _dogService.Create(model.Name, model.Age, model.BreedId, model.Picture);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var dog = _dogService.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }

            var model = new DetailsVM
            {
                Id = dog.Id,
                Name = dog.Name,
                Age = dog.Age,
                BreedName = dog.Breed.Name,
                Picture = dog.Picture
            };

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var dog = _dogService.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }

            var model = new EditVM
            {
                Id = dog.Id,
                Name = dog.Name,
                Age = dog.Age,
                BreedId = dog.BreedId,
                Picture = dog.Picture,
                Breeds = _breedService.GetBreeds()
                    .Select(b => new BreedPairViewModel
                    {
                        Id = b.Id,
                        Name = b.Name
                    }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Breeds = _breedService.GetBreeds()
                    .Select(b => new BreedPairViewModel
                    {
                        Id = b.Id,
                        Name = b.Name
                    }).ToList();

                return View(model);
            }

            _dogService.UpdateDog(model.Id, model.Name, model.Age, model.BreedId, model.Picture);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var dog = _dogService.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }

            var model = new DeleteVM
            {
                Id = dog.Id,
                Name = dog.Name,
                Age = dog.Age,
                BreedName = dog.Breed.Name,
                Picture = dog.Picture
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _dogService.RemoveById(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
