using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR.NotificationPublishers;

namespace StargateAPI.Business.Data
{

    [Table("Person")]
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public virtual Account? Account { get; set; }

        public virtual ICollection<AstronautDuty> AstronautDuties { get; set; } = new HashSet<AstronautDuty>();

    }

    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasMany(z => z.AstronautDuties).WithOne(z => z.Person).HasForeignKey(z => z.PersonId);
        }
    }
}
