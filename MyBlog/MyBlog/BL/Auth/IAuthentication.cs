using System;
using System.ComponentModel.DataAnnotations;
using MyBlog.DAL.Models;

namespace MyBlog.BL.Auth
{
	public interface IAuthentication
	{
		Task<int> CreateUser(UserModel user);

		Task<ValidationResult?> ValidateEmail(string? email);

		Task<bool> Authenticate(String email, String password, string ip, bool rememberme);

		Task<bool> IsAccountLocked(String email);

		Task<int> UpdatePassword(int userid, string salt, string password);
	}
}

