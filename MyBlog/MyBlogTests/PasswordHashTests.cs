using Microsoft.AspNetCore.DataProtection;
using MyBlog.BL.Auth;

namespace MyBlogTests;

public class PasswordHashTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Md5Test()
    {
        string password = "Test";
        string salt = Guid.NewGuid().ToString();

        IEncrypt encrypt = new Md5Encrypt();
        string hash = encrypt.HashPassword(password, salt);
        Console.WriteLine(salt +  ": " + hash);

        Assert.AreEqual(encrypt.HashPassword(password, salt), hash);
    }

    [Test]
    public void Pbkdf2Encrypt()
    {
        string password = "Test";
        string salt = Guid.NewGuid().ToString();

        var provider = DataProtectionProvider.Create("Test App");
        IEncrypt encrypt = new Pbkdf2Encrypt(provider);
        string hash = encrypt.HashPassword(password, salt);
        Console.WriteLine(salt + ": " + hash);

        Assert.AreEqual(encrypt.HashPassword(password, salt), hash);
    }
}

