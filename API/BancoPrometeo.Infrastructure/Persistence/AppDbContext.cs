using BancoPrometeo.Domain.Entities;
using BancoPrometeo.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BancoPrometeo.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // View-mapped DbSets (read-only)
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Transfer> Transfers => Set<Transfer>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<LoanInstallment> LoanInstallments => Set<LoanInstallment>();
    public DbSet<CreditCard> CreditCards => Set<CreditCard>();
    public DbSet<ServicePayment> ServicePayments => Set<ServicePayment>();
    public DbSet<Investment> Investments => Set<Investment>();
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Map Identity tables to Sec schema
        builder.Entity<ApplicationUser>().ToTable("Users", "Sec");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>().ToTable("Roles", "Sec");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>().ToTable("UserRoles", "Sec");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>().ToTable("UserClaims", "Sec");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>().ToTable("UserLogins", "Sec");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>().ToTable("RoleClaims", "Sec");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<string>>().ToTable("UserTokens", "Sec");

        // Customer
        builder.Entity<Customer>(e =>
        {
            e.ToTable("Customers", "Core");
            e.HasKey(x => x.CustomerId);
            e.Property(x => x.CustomerId).HasColumnName("CustomerId");
        });

        // Account
        builder.Entity<Account>(e =>
        {
            e.ToTable("Accounts", "Core");
            e.HasKey(x => x.AccountId);
            e.Property(x => x.Status).HasConversion<string>();
        });

        // Transaction
        builder.Entity<Transaction>(e =>
        {
            e.ToTable("Transactions", "Core");
            e.HasKey(x => x.TxnId);
        });

        // Transfer
        builder.Entity<Transfer>(e =>
        {
            e.ToTable("Transfers", "Core");
            e.HasKey(x => x.TransferId);
        });

        // Loan
        builder.Entity<Loan>(e =>
        {
            e.ToTable("Loans", "Core");
            e.HasKey(x => x.LoanId);
            e.Property(x => x.Status).HasConversion<string>();
        });

        // LoanInstallment
        builder.Entity<LoanInstallment>(e =>
        {
            e.ToTable("LoanInstallments", "Core");
            e.HasKey(x => x.InstallmentId);
        });

        // CreditCard
        builder.Entity<CreditCard>(e =>
        {
            e.ToTable("CreditCards", "Core");
            e.HasKey(x => x.CardId);
        });

        // ServicePayment
        builder.Entity<ServicePayment>(e =>
        {
            e.ToTable("ServicePayments", "Core");
            e.HasKey(x => x.PaymentId);
        });

        // Investment
        builder.Entity<Investment>(e =>
        {
            e.ToTable("Investments", "Core");
            e.HasKey(x => x.InvestmentId);
        });
    }
}
