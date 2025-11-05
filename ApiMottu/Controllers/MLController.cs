using ApiMottu.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiMottu.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/ml")]
    [ApiVersion("1.0")]
    public class MLController : ControllerBase
    {
        private readonly IModelPredictionService _svc;

        public MLController(IModelPredictionService svc)
        {
            _svc = svc;
        }

        // POST api/v1/ml/predict
        [HttpPost("predict")]
        public IActionResult Predict([FromBody] ProdutoFeatures features)
        {
            var result = _svc.Predict(features);
            return Ok(new { Probability = result.Score });
        }
    }
}
