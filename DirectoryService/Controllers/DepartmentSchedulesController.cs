using DirectoryService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Controllers
{
    /// <summary>
    /// Контроллер для управления расписаниями подразделений
    /// </summary>
    [Route("api/department/schedules")]
    [ApiController]
    public class DepartmentSchedulesController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        
        public DepartmentSchedulesController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

            /// <summary>
            /// Получить расписания по идентификатору подразделения
            /// </summary>
            /// <param name="departmentId">Идентификатор подразделения</param>
            /// <returns>Расписания подразделения</returns>
            /// <response code="200">Возвращает расписания подразделения</response>
            /// <response code="400">Некорректный идентификатор подразделения</response>
            /// <response code="404">Подразделение с указанным ID не найдено</response>
            [HttpGet("{departmentId}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> GetSchedulesByDepartmentId(Guid departmentId)
            {
                if (departmentId == Guid.Empty)
                {
                    return BadRequest("Идентификатор подразделения не может быть пустым.");
                }

                var schedules = await _departmentService.GetDepartmentSchedulesAsync(departmentId);

                if (schedules == null)
                {
                    return NotFound($"Подразделение с ID {departmentId} не найдено.");
                }

                return Ok(schedules);
            }
        }
    }
