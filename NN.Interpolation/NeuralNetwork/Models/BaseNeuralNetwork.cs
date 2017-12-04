using System.Collections.Generic;

namespace NeuralNetwork.Models
{
    public abstract class BaseNeuralNetwork<TNeuralInptuParameter, TNeuralOutputParameter>
    {
        public abstract List<BaseLayer<TNeuralInptuParameter, TNeuralOutputParameter>> Layers { get; set; }
    }
}