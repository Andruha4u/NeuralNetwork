using System;
using System.Collections.Generic;

namespace NeuralNetwork.Models
{
    public class BaseLayer<TInput, TOutput>
    {
        public BaseLayer(int neuronsCount)
        {
            Neurons = new List<BaseNeuron<TOutput>>(neuronsCount);
        }

        public Func<TInput, TOutput> ActivationFunction { get; set; }

        public List<BaseNeuron<TOutput>> Neurons { get; set; }
    }
}
