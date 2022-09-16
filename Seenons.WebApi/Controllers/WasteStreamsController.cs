using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seenons.WasteStreams;
using Seenons.WebApi.Models;
using Seenons.WebApi.Models.WasteStreams;
using Swashbuckle.AspNetCore.Annotations;

namespace Seenons.WebApi.Controllers
{
    [ApiController]
    public class WasteStreamsController : ControllerBase
    {
        private readonly IGetWasteStreamsUseCase _getWasteStreamsUseCase;

        public WasteStreamsController(IGetWasteStreamsUseCase getWasteStreamsUseCase)
        {
            _getWasteStreamsUseCase = getWasteStreamsUseCase;
        }

        [HttpGet("api/wastestreams")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(ErrorResponse))]
        public async Task<IActionResult> GetWasteStreams([FromQuery] GetWasteStreamsRequest request)
        {
            try
            {
                var wasteStreams = (await _getWasteStreamsUseCase.GetWasteStreamsAsync(request.PostalCode, request.Weekdays)).ToArray();

                var result = new
                {
                    items = wasteStreams.Select(WasteStreamResponse.From).ToArray(),
                    totalItems = wasteStreams.Length
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    _ => StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Message = ex?.Message })
                };
            }
        }
    }
}
