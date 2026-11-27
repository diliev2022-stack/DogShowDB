
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

        // GET: Dog
        public IActionResult Index(string searchStringBreed, string searchStringName)
        {
            var dogs = _dogService.GetDogs(searchStringBreed, searchStringName)
                .Select(item => new IndexVM
                {
                    Id = item.Id,
                    Name = item.Name,
                    Age = item.Age,
                    BreedName = item.Breed.Name,
                    Picture = item.Picture
                }).ToList();

            return View(dogs);
        }

        // GET: Dog/Details/5
        public IActionResult Details(int id)
        {
            var item = _dogService.GetDogById(id);

            if (item == null)
            {
                return NotFound();
            }

            DetailsVM dog = new DetailsVM
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                BreedName = item.Breed.Name,
                Picture = item.Picture
            };

            return View(dog);
        }

        // GET: Dog/Create
        public IActionResult Create()
        {
            var dog = new CreateVM();

            dog.Breeds = _breedService.GetBreeds()
                .Select(c => new BreedPairViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();

            return View(dog);
        }

        // POST: Dog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateVM dog)
        {
            if (ModelState.IsValid)
            {
                var created = _dogService.Create(
                    dog.Name,
                    dog.Age,
                    dog.BreedId,
                    dog.Picture
                );

                if (created)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            dog.Breeds = _breedService.GetBreeds()
                .Select(c => new BreedPairViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();

            return View(dog);
        }

        // GET: Dog/Edit/5
        public IActionResult Edit(int id)
        {
            var item = _dogService.GetDogById(id);

            if (item == null)
            {
                return NotFound();
            }

            EditVM dog = new EditVM
            {
                Id = item.Id,
                Name = item.Name,
                BreedId = item.BreedId,
                Age = item.Age,
                Picture = item.Picture
            };

            dog.Breeds = _breedService.GetBreeds()
                .Select(c => new BreedPairViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();

            return View(dog);
        }

        // POST: Dog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EditVM bindingModel)
        {
            if (ModelState.IsValid)
            {
                var updated = _dogService.UpdateDog(
                    id,
                    bindingModel.Name,
                    bindingModel.Age,
                    bindingModel.BreedId,
                    bindingModel.Picture
                );

                if (updated)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            bindingModel.Breeds = _breedService.GetBreeds()
                .Select(c => new BreedPairViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();

            return View(bindingModel);
        }

        // GET: Dog/Delete/5
        public IActionResult Delete(int id)
        {
            var item = _dogService.GetDogById(id);

            if (item == null)
            {
                return NotFound();
            }

            DeleteVM dog = new DeleteVM
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                BreedName = item.Breed.Name,
                Picture = item.Picture
            };

            return View(dog);
        }

        // POST: Dog/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            var deleted = _dogService.RemoveById(id);

            if (deleted)
            {
                return RedirectToAction(nameof(Index), "Dog");
            }
            else
            {
                return View();
            }
        }
    }
}
