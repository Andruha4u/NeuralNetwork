namespace NeuralNetwork.Interfaces
{
    public interface ITrainable<TInput, TOutput>
    {
        void Train(TInput input, TOutput output);
    }
}
