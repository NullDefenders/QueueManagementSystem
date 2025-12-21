namespace DirectoryService.Models.DTOs
{
    /// <summary>
    /// DTO для удаления учреждения
    /// </summary>
    public class DeleteFacilityRequestDto
    {
        /// <summary>
        /// ID учреждения для удаления
        /// </summary>
        public Guid FacilityId { get; set; }
    }
}
