using System.ComponentModel.DataAnnotations;

namespace DikkeTennisLijst.WebAPI.Models.Request
{
    public class PlayerResetPasswordRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "The confirmation password entered does not match the new password.")]
        public string ConfirmationPassword { get; set; }
    }
}