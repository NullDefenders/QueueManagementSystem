using DirectoryService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Controllers
{
    /// <summary>
    /// Контроллер для управления рабочими местами
    /// </summary>
    [Route("api/department/workplaces")]
    [ApiController]
    public class DepartmentWokplacesController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentWokplacesController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Получить список рабочих мест по идентификатору подразделения
        /// </summary>
        /// <param name="departmentId">Идентификатор подразделения</param>
        /// <returns>Список рабочих мест</returns>
        /// <response code="200">Возвращает список рабочих мест (может быть пустым)</response>
        /// <response code="400">Некорректный идентификатор подразделения</response>
        /// <response code="404">Подразделение с указанным ID не найдено</response>
        [HttpGet("{departmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWorkplacesByDepartmentId(Guid departmentId)
        {
            if (departmentId == Guid.Empty)
            {
                return BadRequest("Идентификатор подразделения не может быть пустым.");
            }
            
            var workplaces = await _departmentService.GetDepartmentWorkplacesAsync(departmentId);

            if (workplaces == null)
            {
                return NotFound($"Подразделение с ID {departmentId} не найдено.");
            }

            return Ok(workplaces);
        }
    }
}
