using Microsoft.ML;
using Microsoft.ML.Data;

namespace ApiMottu.Services
{
    // Estruturas usadas no modelo
    public class ModelInput
    {
        public float Preco { get; set; }
        public float Estoque { get; set; }
        public bool Label { get; set; } // variável alvo (true/false)
    }

    public class ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; }

        public float Score { get; set; }
    }

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
                .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", maximumNumberOfIterations: 100));

           
            _model = pipeline.Fit(data);

            
            _predEngine = _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(_model);
        }

        public ProdutoPrediction Predict(ProdutoFeatures features)
        {
            var input = new ModelInput
            {
                Preco = features.Preco,
                Estoque = features.Estoque
            };

            var output = _predEngine.Predict(input);
            return new ProdutoPrediction(output.Score);
        }

        public void TrainSampleModel()
        {
            
        }
    }
}
