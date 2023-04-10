namespace ProductManagerAPI.Dtos
{
    partial class UserForLoginDto
    {
        string Email { get; set; }
        string Password { get; set; }
        UserForLoginDto()
        {
            Email ??= "";
            Password ??= "";
        }
    }
}
