using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Application.DTOs
{
    public class ProfileEditRequest
    {
        [Display(Name = "Display Name")]
        public string? DisplayName { get; set; }
    }
}
