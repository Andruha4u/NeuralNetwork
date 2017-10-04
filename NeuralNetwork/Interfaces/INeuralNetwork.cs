namespace NeuralNetwork.Interfaces
{
    public interface INeuralNetwork<TNeuralInptuParameter, TNeuralOutputParameter> : IExecutable<TNeuralInptuParameter, TNeuralOutputParameter>,
                                                                                     ITrainable<TNeuralInptuParameter, TNeuralOutputParameter>
    {
    }
}
