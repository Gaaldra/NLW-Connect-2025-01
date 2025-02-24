namespace TechLibrary.Api.Infraestructure.Security.Criptography;

public static class BCryptAlgorithm
{
    public static string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public static bool VerifyPassword(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
}
