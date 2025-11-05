using Xunit;
using ApiMottu.Services;

namespace ApiMottu.Tests
{
    public class ModelPredictionServiceTests
    {
        private readonly ModelPredictionService _service;

        public ModelPredictionServiceTests()
        {
            _service = new ModelPredictionService();
        }

        [Fact]
        public void Predict_ShouldReturnValidProbability()
        {
            var result = _service.Predict(new ProdutoFeatures(100, 10));
            Assert.InRange(result.Score, 0, 1);
        }
    }
}
