using System.ComponentModel.DataAnnotations;

namespace Webhallen.Services
{
    public class WebhallenConfiguration
    {
        [Required]
        public string BaseUrl { get; set; } = "";

        [Required]
        public string Username { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [Required]
        public string SupplyDropSelector { get; set; } = "";
    }
}
