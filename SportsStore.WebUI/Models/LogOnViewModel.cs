using System.ComponentModel.DataAnnotations;

namespace SportsStore.WebUI.Models {
    public class LogOnViewModel {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}