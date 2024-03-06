using Microsoft.AspNetCore.Identity;

namespace BrowserTravel.Solutions.Api.Utilities;

// Clase utilitaria para crear roles y usuarios predeterminados
public static class DefaultUserAndRoles
{
    // Método para sembrar roles en la base de datos
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        // Nombres de roles predeterminados
        string[] roleNames = { "admin", "invitado" };

        // Verificar y crear roles si no existen
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    // Método para sembrar un usuario administrador en la base de datos
    public static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager)
    {
        // Nombres de usuarios, contraseñas y roles predeterminados
        string[] users = { "invitado1", "administrador1" };
        string[] passwords = { "Invitado1!", "Administrador1!" };
        string[] roles = { "invitado", "admin" };
        int position = 0;

        // Verificar y crear usuarios administradores si no existen
        foreach (string user in users)
        {
            var adminUser = await userManager.FindByNameAsync(user);

            if (adminUser == null)
            {
                var identityUser = new IdentityUser
                {
                    UserName = user,
                    Email = $"{user}@example.com"
                };

                // Crear usuario, establecer contraseña y asignar el rol correspondiente
                await userManager.CreateAsync(identityUser, passwords[position]);
                await userManager.AddToRoleAsync(identityUser, roles[position]);
                position++;
            }
        }
    }
}
