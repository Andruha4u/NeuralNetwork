using System.Collections.Generic;

namespace NeuralNetwork.API.Models
{
    public class Layer
    {
        public List<Neuron> Neurons { get; set; }
        public double Bias { get; set; }
        public ActivationFunctionType ActivationFunctionType { get; set; }
    }
}
