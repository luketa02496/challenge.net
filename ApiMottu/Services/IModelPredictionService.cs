namespace ApiMottu.Services
{
    public record ProdutoFeatures(float Preco, float Estoque);
    public record ProdutoPrediction(float Score);

    public interface IModelPredictionService
    {
        ProdutoPrediction Predict(ProdutoFeatures features);
        void TrainSampleModel(); 
    }
}
