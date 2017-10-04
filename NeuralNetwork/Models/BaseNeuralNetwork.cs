using System.Collections.Generic;

namespace NeuralNetwork.Models
{
    public abstract class BaseNeuralNetwork<TNeuralInptuParameter, TNeuralOutputParameter>
    {
        protected abstract List<BaseLayer<TNeuralInptuParameter, TNeuralOutputParameter>> Layers { get; set; }
    }
}