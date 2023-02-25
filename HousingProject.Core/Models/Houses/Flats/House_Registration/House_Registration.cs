using HousingProject.Core.Models.People;
using HousingProject.Core.Models.People.General;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HousingProject.Core.Models.Houses.Flats.House_Registration
{
    public class House_Registration
    {
        [Key]
        public int HouseiD { get; set; }

        public string House_Location { get; set; }

        public int Total_Units { get; set; }

        public string Owner_Firstname { get; set; }

        public string  Owner_LastName { get; set; }

        public int Owner_id_Number { get; set; }

        public List<TenantClass> Tenant { get; set; }

        public string House_Name { get; set; }
       
        public string  UserId { get; set; }
        public RegistrationModel CreatedBy { get; set; }

        public int Estimated_Maximum_Capacity { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public bool EmailSent { get; set; }

        public string Country { get; set; }
        public string Area { get; set; }
        public string  CreatorNames { get; set; }
        public string  CreatorEmail { get; set; }




    }
    }
