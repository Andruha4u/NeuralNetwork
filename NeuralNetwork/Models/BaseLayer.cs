using System;
using System.Collections.Generic;

namespace NeuralNetwork.Models
{
    public class BaseLayer<TInput, TOutput>
    {
        public BaseLayer(int neuronsCount, Func<TInput, TOutput> activationFunction, TOutput bias)
        {
            Neurons = new List<BaseNeuron<TOutput>>(neuronsCount);
            ActivationFunction = activationFunction;
            Bias = bias;
        }

        public Func<TInput, TOutput> ActivationFunction { get; set; }

        public List<BaseNeuron<TOutput>> Neurons { get; set; }

        public TOutput Bias;
    }
}
