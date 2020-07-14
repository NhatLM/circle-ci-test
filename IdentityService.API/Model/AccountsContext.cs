using Microsoft.EntityFrameworkCore;

namespace IdentityService.API.Model
{
    public partial class AccountsContext : DbContext
    {
        public AccountsContext()
        {
        }

        public AccountsContext(DbContextOptions<AccountsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CustUser> CustUser { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        var connectionStr = System.Environment.GetEnvironmentVariable("AccountSQLConnection");
        //        optionsBuilder.UseMySql(connectionStr);
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustUser>(entity =>
            {
                entity.ToTable("cust_user");

                entity.HasIndex(e => e.CustId)
                    .HasName("cust");

                entity.HasIndex(e => e.DirectPhoneCountryCode)
                    .HasName("fk_cust_user_direct_phone_country_codes");

                entity.HasIndex(e => e.Hash)
                    .HasName("hash")
                    .IsUnique();

                entity.HasIndex(e => e.PhoneCountryCode)
                    .HasName("fk_cust_user_phone_country_codes");

                entity.HasIndex(e => e.UserType)
                    .HasName("fk_cust_user_customer_user_types");

                entity.HasIndex(e => new { e.CustId, e.StayOnHash })
                    .HasName("cust_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.AdminLocale)
                    .HasColumnName("admin_locale")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.AllowBidding).HasColumnName("allow_bidding");

                entity.Property(e => e.AllowBuyerBlock).HasColumnName("allow_buyer_block");

                entity.Property(e => e.AllowCreateUsers).HasColumnName("allow_create_users");

                entity.Property(e => e.AllowGuarenteedBid).HasColumnName("allow_guarenteed_bid");

                entity.Property(e => e.AllowSelling).HasColumnName("allow_selling");

                entity.Property(e => e.AssessmentUser).HasColumnName("assessment_user");

                entity.Property(e => e.BlockUsage).HasColumnName("block_usage");

                entity.Property(e => e.ConfirmFavoriteRemoval).HasColumnName("confirm_favorite_removal");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CurrencyCode)
                    .HasColumnName("currency_code")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CustId).HasColumnName("cust_id");

                entity.Property(e => e.CustTaskSubstitute).HasColumnName("cust_task_substitute");

                entity.Property(e => e.DirectPhone)
                    .HasColumnName("direct_phone")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DirectPhoneCountryCode).HasColumnName("direct_phone_country_code");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.EmailVerifiedAt).HasColumnName("email_verified_at");

                entity.Property(e => e.EmailplatformId).HasColumnName("emailplatform_id");

                entity.Property(e => e.ErpIdNumber)
                    .HasColumnName("erp_id_number")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.GroupEmail)
                    .HasColumnName("group_email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Hash)
                    .HasColumnName("hash")
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceContactName)
                    .HasColumnName("invoice_contact_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceFree)
                    .HasColumnName("invoice_free")
                    .HasColumnType("date");

                entity.Property(e => e.LastDailyAuction).HasColumnName("last_daily_auction");

                entity.Property(e => e.ListViewMode)
                    .IsRequired()
                    .HasColumnName("list_view_mode")
                    .HasColumnType("enum('list','gallery')")
                    .HasDefaultValueSql("'list'");

                entity.Property(e => e.Locale)
                    .IsRequired()
                    .HasColumnName("locale")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'da'")
                    .HasComment("ISO 639-1");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NewsRead)
                    .HasColumnName("news_read")
                    .HasColumnType("date");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneCountryCode).HasColumnName("phone_country_code");

                entity.Property(e => e.RememberToken)
                    .HasColumnName("remember_token")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StayOnHash)
                    .HasColumnName("stay_on_hash")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TermsApproval).HasColumnName("terms_approval");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UserLevel)
                    .HasColumnName("user_level")
                    .HasDefaultValueSql("'1000'");

                entity.Property(e => e.UserType)
                    .IsRequired()
                    .HasColumnName("user_type")
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'USER'");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
