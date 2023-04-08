using HousingProject.Core.Models.CountiesModel;
using HousingProject.Core.Models.General;
using HousingProject.Core.Models.Houses;
using HousingProject.Core.Models.Houses.Flats.AdminContacts;
using HousingProject.Core.Models.Houses.Flats.House_Registration;
using HousingProject.Core.Models.Houses.Flats.uploadImage;
using HousingProject.Core.Models.Houses.HouseAggrement;
using HousingProject.Core.Models.Houses.HouseUnitRegistration;
using HousingProject.Core.Models.Houses.HouseUsers;
using HousingProject.Core.Models.ImagesModelsUsed;
using HousingProject.Core.Models.People;
using HousingProject.Core.Models.People.General;
using HousingProject.Core.Models.People.Landlord;
using HousingProject.Core.Models.Professionals;
using HousingProject.Core.Models.RentPayment;
using HousingProject.Core.Models.WorkIdModel;
using HousingProject.Infrastructure.ExtraFunctions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HousingProject.Architecture.Data
{
    public class HousingProjectContext : IdentityDbContext<RegistrationModel>
    {


        public HousingProjectContext(DbContextOptions<HousingProjectContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RegistrationModel>(entity =>
            {
                entity.ToTable(name: "Registration");
            });
        }

        public DbSet<RegistrationModel> RegistrationModel { get; set; }

        public DbSet<House_Registration> House_Registration { get; set; }

        public DbSet<TenantClass> TenantClass { get; set; }

        public DbSet<Landlordmodel> Landlordmodel { get; set; }

        public DbSet<TenantSummary> TenantSummary { get; set; }

        public DbSet<AdminContacts> AdminContacts { get; set; }

        public DbSet<ContactUs> ContactUs { get; set; }

        public DbSet<UploadImage> UploadImage { get; set; }

        public DbSet<Rentpayment> Rentpayment { get; set; }

        public DbSet<AddCounty> AddCounty { get; set; }

        public DbSet<AddCities> AddCities { get; set; }

        public DbSet<HouseUsers> HouseUsers { get; set; }
        public DbSet<ImaageUploadClass> ImaageUploadClass { get; set; }

        public DbSet<RentDebit> RentDebit { get; set; }
        public DbSet<RegisterProfessional> RegisterProfessional { get; set; }
        public DbSet<HouseUnit> HouseUnit { get; set; }
        public DbSet<GeneratedIdHolder> GeneratedIdHolder { get; set; }
        public DbSet<WorkIdModel> WorkIdModel { get; set; }
        public DbSet<Aggrement> Aggrement { get; set; }
        public DbSet<AggrementSections> AggrementSections { get; set; }
        public DbSet<HouseAggrementMapper> HouseAggrementMapper { get; set; }
        public DbSet<MapperofAggreement> MapperofAggreement { get; set; }
        public DbSet<Sections> Sections { get; set; }
        public DbSet<SectionMapper> SectionMapper { get; set; }


    }
}
