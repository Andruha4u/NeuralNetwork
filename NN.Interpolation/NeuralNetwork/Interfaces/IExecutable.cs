namespace NeuralNetwork.Interfaces
{
    public interface IExecutable<TInput, TOutput>
    {
        TOutput Execute(TInput input);     
    }
}
