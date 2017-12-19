using System;
using System.Collections.Generic;

namespace NeuralNetwork.Models
{
    public class BaseNeuron<T>
    {
        public List<BaseDendrite<T>> Dendrites { get; set; }

        public Func<List<T>, T> Akson { get; set; } 

        public T Delta { get; set; }
    }
}
