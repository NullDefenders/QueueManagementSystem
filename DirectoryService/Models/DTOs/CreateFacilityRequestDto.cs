using System.ComponentModel.DataAnnotations;

namespace DirectoryService.Models.DTOs
{
    public class CreateFacilityRequestDto
    {
        [Required(ErrorMessage = "Код учреждения обязателен")]
        [StringLength(20, ErrorMessage = "Код не должен превышать 20 символов")]
        [Display(Name = "Код учреждения")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Название учреждения обязательно")]
        [StringLength(200, ErrorMessage = "Название не должно превышать 200 символов")]
        [Display(Name = "Название учреждения")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Адрес учреждения обязателен")]
        [StringLength(500, ErrorMessage = "Адрес не должен превышать 500 символов")]
        [Display(Name = "Адрес учреждения")]
        public string Address { get; set; } = string.Empty;
    }
}
