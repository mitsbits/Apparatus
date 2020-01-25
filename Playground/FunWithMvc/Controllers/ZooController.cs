using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FunWithMvc.Models.Zoo;
namespace FunWithMvc.Controllers
{
    public class ZooController : Controller
    {
        public IActionResult Index()
        {

            return View("ViewModel", new Dog() { Id = Guid.NewGuid() });
        }

        [HttpPost]
        public IActionResult Index(Dog model)
        {
            if (ModelState.IsValid)
            {
                return View("Success", model);
            }
            return View("ViewModel", model);
        }
    }



}
