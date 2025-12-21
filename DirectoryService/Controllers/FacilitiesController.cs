using DirectoryService.Models.DTOs;
using DirectoryService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Controllers
{
    /// <summary>
    /// Контроллер для работы с учреждениями и их подразделениями
    /// </summary>
    [Route("api/facilities")]
    [ApiController]
    [Tags("Учреждения")]
    public class FacilitiesController : ControllerBase
    {
        private readonly IFacilityService _facilityService;
        private readonly IDepartmentService _departmentService;

        public FacilitiesController(
            IFacilityService facilityService,
            IDepartmentService departmentService)
        {
            _facilityService = facilityService;
            _departmentService = departmentService;
        }

        /// <summary>
        /// Получить информацию обо всех активных учреждениях
        /// </summary>
        /// <returns>Список активных учреждений</returns>
        /// <response code="200">Успешно возвращен список учреждений</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet]
        [ProducesResponseType(typeof(FacilitiesResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFacilities()
        {
            var facilities = await _facilityService.GetFacilitiesAsync();
            if (facilities == null)
                return StatusCode(500, "Внутренняя ошибка сервера");

            return Ok(facilities);
        }

        /// <summary>
        /// Получить информацию об учреждении по ID
        /// </summary>
        /// <param name="facilityId">Идентификатор учреждения (GUID)</param>
        /// <returns>Информация об учреждении</returns>
        /// <response code="200">Успешно возвращена информация об учреждении</response>
        /// <response code="400">Неверный формат идентификатора</response>
        /// <response code="404">Учреждение не найдено</response>
        [HttpGet("{facilityId}")]
        [ProducesResponseType(typeof(FacilityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFacility(Guid facilityId)
        {
            if (facilityId == Guid.Empty)
                return BadRequest("Идентификатор учреждения не может быть пустым.");

            var facility = await _facilityService.GetFacilityByIdAsync(facilityId);
            if (facility == null)
                return NotFound($"Учреждение с ID {facilityId} не найдено.");

            return Ok(facility);
        }

        /// <summary>
        /// Создать новое учреждение
        /// </summary>
        /// <param name="request">Данные для создания учреждения</param>
        /// <returns>Созданное учреждение</returns>
        /// <response code="201">Учреждение успешно создано</response>
        /// <response code="400">Неверные входные данные</response>
        /// <response code="409">Конфликт (учреждение с таким кодом уже существует)</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost]
        [ProducesResponseType(typeof(FacilityDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateFacility([FromBody] CreateFacilityRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdFacility = await _facilityService.CreateFacilityAsync(
                    request.Code, request.Name, request.Address);

                return CreatedAtAction(
                    nameof(GetFacility),
                    new { facilityId = createdFacility.Id },
                    createdFacility);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Обновить информацию об учреждении
        /// </summary>
        /// <param name="facilityId">Идентификатор учреждения (GUID)</param>
        /// <param name="request">Данные для обновления учреждения</param>
        /// <response code="204">Учреждение успешно обновлено</response>
        /// <response code="400">Неверные входные данные</response>
        /// <response code="404">Учреждение не найдено</response>
        /// <response code="409">Конфликт (учреждение с таким кодом уже существует)</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPut("{facilityId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateFacility(
            Guid facilityId,
            [FromBody] UpdateFacilityRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (facilityId == Guid.Empty)
                return BadRequest("Идентификатор учреждения не может быть пустым.");

            try
            {
                await _facilityService.UpdateFacilityAsync(
                    facilityId, request.Code, request.Name, request.Address);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Получить подразделения по идентификатору учреждения
        /// </summary>
        /// <param name="facilityId">Идентификатор учреждения (GUID)</param>
        /// <returns>Список подразделений учреждения</returns>
        /// <response code="200">Успешно возвращен список подразделений</response>
        /// <response code="400">Неверный формат идентификатора</response>
        /// <response code="404">Учреждение не найдено</response>
        [HttpGet("{facilityId}/departments")]
        [ProducesResponseType(typeof(FacilityDepartmentsResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFacilityDepartments(Guid facilityId)
        {
            if (facilityId == Guid.Empty)
                return BadRequest("Идентификатор учреждения не может быть пустым.");

            var departments = await _departmentService.GetFacilityDepartmentsAsync(facilityId);
            if (departments == null)
                return NotFound($"Учреждение с ID {facilityId} не найдено.");

            return Ok(departments);
        }

        /// <summary>
        /// Удалить учреждение
        /// </summary>
        /// <param name="facilityId">Идентификатор учреждения (GUID)</param>
        /// <response code="204">Учреждение успешно удалено</response>
        /// <response code="400">Неверный формат идентификатора</response>
        /// <response code="404">Учреждение не найдено</response>
        /// <response code="409">Конфликт (нельзя удалить учреждение с подразделениями)</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete("{facilityId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFacility(Guid facilityId)
        {
            if (facilityId == Guid.Empty)
                return BadRequest("Идентификатор учреждения не может быть пустым.");

            try
            {
                var success = await _facilityService.DeleteFacilityAsync(facilityId);

                if (success)
                    return NoContent();
                else
                    return NotFound($"Учреждение с ID {facilityId} не найдено.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}