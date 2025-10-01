using DirectoryService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Controllers
{
    namespace DirectoryService.Controllers
    {
        /// <summary>
        /// Контроллер для получения информации об учреждениях
        /// </summary>
        [Route("api/facilities")]
        [ApiController]
        public class FacilitiesController : ControllerBase
        {
            private readonly IFacilityService _facilityService;

            public FacilitiesController(IFacilityService facilityService)
            {
                _facilityService = facilityService;
            }

            /// <summary>
            /// Получить информацию обо всех активных учреждениях и список подразделений в их составе
            /// </summary>
            /// <returns>Информация об учреждениях</returns>
            /// <response code="200">Возвращает информацию об учреждениях</response>
            [HttpGet]
            [ProducesResponseType(StatusCodes.Status200OK)]
            public async Task<IActionResult> GetFacilities()
            {
                var facilities = await _facilityService.GetFacilitiesAsync();
                return Ok(facilities);
            }
        }
    }
}
