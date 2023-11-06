using burgerBurger.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


    public class Program
    {
        public static async Task Main(string[] args)
        {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        // changed to false because we don't want required email confirmation
        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddControllersWithViews();

        builder.Services.AddSession();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "Owner", "Manager", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string email = "admin@burgerburger.com";
            string password = "Burger1234!";

            if(await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser();
                user.UserName = email;
                user.Email = email;

                // Workaround if we want to enable email verification
                //user.EmailConfirmed = true;

                await userManager.CreateAsync(user, password);

                await userManager.AddToRoleAsync(user, "Admin");
            }
        }

        /*
         * Authorization is at class or method level
         * [Authorize(Roles = "Admin")] -- Require admin
         * [Authorize] -- Require just being logged in
         * 
         * 
         * Required in any razor page (at the top)
         *   @using Microsoft.AspNetCore.Identity
         *   @inject SignInManager<IdentityUser> SignInManager
         *   @inject UserManager<IdentityUser> UserManager
         *   
         *   Logic for hiding/showing items in razor page (see above for required injections)
         *
         *   @if (User.IsInRole("Admin"))
         *   {
         *       <h1>BIG BOSS ADMIN HAS ENTERED THE BUILDING</h1>
         *   }
         */

        app.UseSession();
        app.Run();

    }
}

