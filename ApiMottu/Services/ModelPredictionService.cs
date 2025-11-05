using Microsoft.ML;
using Microsoft.ML.Data;

namespace ApiMottu.Services
{
    public class ModelPredictionService : IModelPredictionService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;
        private readonly PredictionEngine<ModelInput, ModelOutput> _predEngine;

        public ModelPredictionService()
        {
            _mlContext = new MLContext(seed: 1);

            
            var samples = new List<ModelInput>
            {
                new ModelInput { Preco = 50, Estoque = 10, Label = false }, 
                new ModelInput { Preco = 300, Estoque = 2, Label = true }, 
                new ModelInput { Preco = 200, Estoque = 1, Label = true },
                new ModelInput { Preco = 100, Estoque = 20, Label = false },
                new ModelInput { Preco = 150, Estoque = 5, Label = true }
            };

            var data = _mlContext.Data.LoadFromEnumerable(samples);

            var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(ModelInput.Preco), nameof(ModelInput.Estoque))
                .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

            _model = pipeline.Fit(data);
            _predEngine = _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(_model);
        }

        public ProdutoPrediction Predict(ProdutoFeatures features)
        {
            var input = new ModelInput { Preco = features.Preco, Estoque = features.Estoque };
            var pred = _predEngine.Predict(input);
            return new ProdutoPrediction(pred.Probability);
        }

        public void TrainSampleModel() { }

        private class ModelInput
        {
            [ColumnName("Preco")] public float Preco { get; set; }
            [ColumnName("Estoque")] public float Estoque { get; set; }
            [ColumnName("Label")] public bool Label { get; set; }
        }
        private class ModelOutput
        {
            public bool PredictedLabel { get; set; }
            public float Score { get; set; }
            public float Probability { get; set; }
        }
    }
}
