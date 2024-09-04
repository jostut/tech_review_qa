using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Business.Data
{
    [Table("Account")]
    public class Account
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int PersonId { get; set; }

        public virtual Person Person { get; set; }

    }

    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(z => z.Person).WithOne(z => z.Account).HasForeignKey<Account>(z => z.PersonId);
        }
    }
}
