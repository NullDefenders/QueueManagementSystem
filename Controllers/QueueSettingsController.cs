using DirectoryService.Models.DTOs;
using DirectoryService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Controllers
{
    /// <summary>
    /// Контроллер для работы с настройками электронной очереди
    /// </summary>
    [Route("api/queues/{queueId}/settings")]
    [ApiController]
    [Tags("Настройки очередей")]
    public class QueueSettingsController : ControllerBase
    {
        private readonly IQueueSettingsService _queueSettingsService;

        public QueueSettingsController(IQueueSettingsService queueSettingsService)
        {
            _queueSettingsService = queueSettingsService;
        }

        /// <summary>
        /// Получить настройки электронной очереди по идентификатору
        /// </summary>
        /// <param name="queueId">Идентификатор электронной очереди (GUID)</param>
        /// <returns>Настройки электронной очереди</returns>
        /// <response code="200">Успешно возвращены настройки очереди</response>
        /// <response code="400">Неверный формат идентификатора</response>
        /// <response code="404">Очередь не найдена</response>
        [HttpGet]
        [ProducesResponseType(typeof(QueueSettingsResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetQueueSettings(Guid queueId)
        {
            if (queueId == Guid.Empty)
                return BadRequest("Идентификатор электронной очереди не может быть пустым.");

            var queueSettings = await _queueSettingsService.GetQueueSettingsAsync(queueId);
            if (queueSettings == null)
                return NotFound($"Электронная очередь с ID {queueId} не найдена.");

            return Ok(queueSettings);
        }

        /// <summary>
        /// Обновить настройки электронной очереди
        /// </summary>
        /// <param name="queueId">Идентификатор электронной очереди (GUID)</param>
        /// <param name="requestDto">Данные для обновления настроек</param>
        /// <returns>Обновленные настройки электронной очереди</returns>
        /// <response code="200">Настройки успешно обновлены</response>
        /// <response code="400">Неверные входные данные</response>
        /// <response code="404">Очередь не найдена</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPut]
        [ProducesResponseType(typeof(QueueSettingsResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateQueueSettings(
            Guid queueId,
            [FromBody] UpdateQueueSettingsRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (queueId == Guid.Empty)
                return BadRequest("Идентификатор электронной очереди не может быть пустым.");

            try
            {
                var updatedSettings = await _queueSettingsService.UpdateQueueSettingsAsync(
                    queueId,
                    requestDto.MinutesBeforePending,
                    requestDto.MinutesAfterPending,
                    requestDto.PendingCount);

                return Ok(updatedSettings);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}