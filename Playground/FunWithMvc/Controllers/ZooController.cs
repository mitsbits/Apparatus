using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FunWithMvc.Controllers
{
    public class ZooController : Controller
    {
        public IActionResult Index()
        {

            return View(new Dog() { Id = Guid.NewGuid() });
        }

        [HttpPost]
        public IActionResult Index(Dog model)
        {
            if (ModelState.IsValid)
            {
                return View("Success", model);
            }
            return View( model);
        }
    }


    public class Dog : Animal
    {
        [Required]
        public string Name { get; set; }
    }

    public class Animal : DataRecord
    {
        public DateTime BirthDay { get; set; }
        public double Weight { get; set; }
        public Spieces Spieces { get; set; }
    }

    public enum Spieces
    {
        Fish,
        Amphibian,
        Reptile,
        Bird,
        Mamal
    }

    public class DataRecord
    {
        public Guid Id { get; set; } 
    }
}
