using System.ComponentModel.DataAnnotations;

namespace DirectoryService.Models.DTOs
{
    /// <summary>
    /// DTO для ответа с информацией об активных учреждениях
    /// </summary>
    public class FacilitiesResponseDto
    {
        /// <summary>
        /// Список активных учреждений
        /// </summary>
        [Display(Name = "Список активных учреждений")]
        public List<FacilityDto> Facilities { get; }

        public FacilitiesResponseDto(List<FacilityDto> facilities)
        {
            Facilities = facilities;
        }
    }

    public class FacilityDto
    {
        [Display(Name = "ID учреждения")]
        public Guid Id { get; }

        [Display(Name = "Код учреждения")]
        public string Code { get; }

        [Display(Name = "Название учреждения")]
        public string Name { get; }

        [Display(Name = "Адрес учреждения")]
        public string Address { get; }

        public FacilityDto(
            Guid id,
            string code,
            string name,
            string address)
        {
            Id = id;
            Code = code;
            Name = name;
            Address = address;
        }
    }
}
