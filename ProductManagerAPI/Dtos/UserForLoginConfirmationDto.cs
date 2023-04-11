using Microsoft.AspNetCore.Identity;

namespace ProductManagerAPI.Dtos
{
    public partial class UserForLoginConfirmationDto
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public UserForLoginConfirmationDto()
        {
            if(PasswordHash == null)
            {
                PasswordHash = new byte[0];
                // create empty byte array
            }
            if(PasswordSalt == null)
            {
                PasswordSalt = new byte[0];
            }
        }
    }
}
