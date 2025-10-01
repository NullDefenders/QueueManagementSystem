using DirectoryService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Controllers
{
    /// <summary>
    /// Контроллер для получения активных подразделений учреждения
    /// </summary>
    [Route("api/facility/departments")]
    [ApiController]
    public class FacilityDepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IFacilityService _facilityService;

        public FacilityDepartmentsController(
            IDepartmentService departmentService,
            IFacilityService facilityService)
        {
            _departmentService = departmentService;
            _facilityService = facilityService;
        }

        /// <summary>
        /// Получить подразделения по идентификатору учреждения
        /// </summary>
        /// <param name="facilityId">Идентификатор учреждения</param>
        /// <returns>Список подразделений учреждения</returns>
        /// <response code="200">Возвращает список подразделений учреждения</response>
        /// <response code="400">Некорректный идентификатор учреждения</response>
        /// <response code="404">Учреждение с указанным ID не найдено или не имеет подразделений</response>
        [HttpGet("{facilityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFacilllityDepartments(Guid facilityId)
        {
            if (facilityId == Guid.Empty)
            {
                return BadRequest("Идентификатор учреждения не может быть пустым.");
            }

            var departments = await _departmentService.GetFacilityDepartmentsAsync(facilityId);

            if (departments == null)
            {
                return NotFound($"Учреждение с ID {facilityId} не найдено.");
            }

            return Ok(departments);
        }
    }
}
