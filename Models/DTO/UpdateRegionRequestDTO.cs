using System.ComponentModel.DataAnnotations;

namespace WebApiNZwalks.Models.DTO
{
    public class UpdateRegionRequestDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code should be a minimum of 3 characters")]
        [MaxLength(3, ErrorMessage = "Code should be a maximum of 3 characters")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "The name can be atmost 100 characters long")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
