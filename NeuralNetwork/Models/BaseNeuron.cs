using System;
using System.Collections.Generic;

namespace NeuralNetwork.Models
{
    public class BaseNeuron<T>
    {
        public List<BaseDendrite<T>> Dendrites { get; set; }

        public Func<T> Akson { get; set; } = () => default(T);

        public T Delta { get; set; }
    }
}
