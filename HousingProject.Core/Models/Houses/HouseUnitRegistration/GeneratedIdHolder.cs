using System;
using System.ComponentModel.DataAnnotations;

namespace HousingProject.Core.Models.Houses.HouseUnitRegistration
{
    public   class GeneratedIdHolder
    {
        [Key]
        public int GeneratorId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int GeneratorHolder { get; set; }
    }
}
