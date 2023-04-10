namespace ProductManagerAPI.Dtos
{
    partial class UserForRegistrationDto
    {
        string Username { get; set; }
        string Email { get; set; }
        string PasswordHash { get; set; }
        string PasswordSalt { get; set; }
        UserForRegistrationDto()
        {
            Username ??= "";
            Email ??= "";
            PasswordHash ??= "";
            PasswordSalt ??= "";
        }
    }
}
