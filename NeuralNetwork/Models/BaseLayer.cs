using System;
using System.Collections.Generic;

namespace NeuralNetwork.Models
{
    public class BaseLayer<TInput, TOutput>
    {
        public Func<TInput, TOutput> ActivationFunction { get; set; }

        public List<BaseNeuron<TOutput>> Neurons { get; set; }

        public TOutput Bias;
    }
}
