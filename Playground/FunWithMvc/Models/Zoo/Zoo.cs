using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FunWithMvc.Models.Zoo
{
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
