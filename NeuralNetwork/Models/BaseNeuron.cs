using System.Collections.Generic;

namespace NeuralNetwork.Models
{
    public class BaseNeuron<T>
    {
        public List<double> Weights { get; set; }

        public T Value { get; set; }
    }
}
