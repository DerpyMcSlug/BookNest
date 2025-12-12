using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using BookNest.Models;
using System.Threading.Tasks;

public static class SeedAdmin
{
	public static async Task SeedAdminAsync(IServiceProvider services)
	{
		var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
		var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

		// Ensure Admin role exists
		if (!await roleManager.RoleExistsAsync("Admin"))
		{
			await roleManager.CreateAsync(new IdentityRole("Admin"));
		}

		// Check if admin user already exists
		var existingAdmin = await userManager.FindByEmailAsync("admin@booknest.com");

		if (existingAdmin == null)
		{
			var adminUser = new ApplicationUser
			{
				UserName = "mahuy2005@gmail.com",
				Email = "mahuy2005@gmail.com",
				NormalizedEmail = "MAHUY2005@GMAIL.COM",
				NormalizedUserName = "MAHUY2005@GMAIL.COM",
				EmailConfirmed = true,

				// REQUIRED FIELD
				Name = "Admin User",

				// OPTIONAL FIELDS
				StreetAddress = null,
				City = null,
				State = null,
				PostalCode = null,
				CompanyId = null,
				LastOtpSentAt = null,
				TwoFactorCode = null,
				TwoFactorExpiry = null
			};

			// Create admin with password
			var result = await userManager.CreateAsync(adminUser, "Admin123!");

			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(adminUser, "Admin");
			}
			else
			{
				// Log errors if needed
				foreach (var error in result.Errors)
				{
					Console.WriteLine($"Admin creation error: {error.Description}");
				}
			}
		}
	}
}