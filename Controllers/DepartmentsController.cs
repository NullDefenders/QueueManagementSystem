using DirectoryService.Models.DTOs;
using DirectoryService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Controllers
{
    /// <summary>
    /// Контроллер для управления подразделениями и их данными
    /// </summary>
    [Route("api/departments")]
    [ApiController]
    [Tags("Подразделения")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Получить расписания работы по идентификатору подразделения
        /// </summary>
        /// <param name="departmentId">Идентификатор подразделения (GUID)</param>
        /// <returns>Список расписаний работы подразделения</returns>
        /// <response code="200">Успешно возвращен список расписаний</response>
        /// <response code="400">Неверный формат идентификатора</response>
        /// <response code="404">Подразделение не найдено</response>
        [HttpGet("{departmentId}/schedules")]
        [ProducesResponseType(typeof(DepartmentSchedulesResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSchedules(Guid departmentId)
        {
            if (departmentId == Guid.Empty)
                return BadRequest("Идентификатор подразделения не может быть пустым.");

            var schedules = await _departmentService.GetDepartmentSchedulesAsync(departmentId);
            if (schedules == null)
                return NotFound($"Подразделение с ID {departmentId} не найдено.");

            return Ok(schedules);
        }

        /// <summary>
        /// Получить услуги по идентификатору подразделения
        /// </summary>
        /// <param name="departmentId">Идентификатор подразделения (GUID)</param>
        /// <returns>Список услуг подразделения</returns>
        /// <response code="200">Успешно возвращен список услуг</response>
        /// <response code="400">Неверный формат идентификатора</response>
        /// <response code="404">Подразделение не найдено</response>
        [HttpGet("{departmentId}/services")]
        [ProducesResponseType(typeof(DepartmentServicesResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetServices(Guid departmentId)
        {
            if (departmentId == Guid.Empty)
                return BadRequest("Идентификатор подразделения не может быть пустым.");

            var services = await _departmentService.GetDepartmentServicesAsync(departmentId);
            if (services == null)
                return NotFound($"Подразделение с ID {departmentId} не найдено.");

            return Ok(services);
        }

        /// <summary>
        /// Получить рабочие места по идентификатору подразделения
        /// </summary>
        /// <param name="departmentId">Идентификатор подразделения (GUID)</param>
        /// <returns>Список рабочих мест подразделения</returns>
        /// <response code="200">Успешно возвращен список рабочих мест</response>
        /// <response code="400">Неверный формат идентификатора</response>
        /// <response code="404">Подразделение не найдено</response>
        [HttpGet("{departmentId}/workplaces")]
        [ProducesResponseType(typeof(DepartmentWorkplacesResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWorkplaces(Guid departmentId)
        {
            if (departmentId == Guid.Empty)
                return BadRequest("Идентификатор подразделения не может быть пустым.");

            var workplaces = await _departmentService.GetDepartmentWorkplacesAsync(departmentId);
            if (workplaces == null)
                return NotFound($"Подразделение с ID {departmentId} не найдено.");

            return Ok(workplaces);
        }
    }
}