using Borg.Infrastructure.Core.Collections;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Samples.TagHelpers.Controllers
{
    public class PaginationTagHelperController : Controller
    {
        readonly IList<Person> data;
        public PaginationTagHelperController()
        {

             data = Builder<Person>.CreateListOfSize(100).Build();
     ;
        }

        public IActionResult Index(int p = 1)
        {
            var local = data.Skip((p - 1) * 10).Take(10);
            var model = PagedResult<Person>.Create(local, 10, p, data.Count()) ;
            return View(model);
        }
    }


    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public bool IsBlocked { get; set; }
    }
}
