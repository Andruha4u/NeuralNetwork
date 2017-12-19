using NeuralNetwork.Interfaces;
using NeuralNetwork.Models;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.API
{
    public class ApproximationNeuralNetworkExecutor : IExecutable<BaseNeuralParameter<double>, BaseNeuralParameter<double>>
    {
        private readonly List<BaseLayer<double, double>> Layers;

        public ApproximationNeuralNetworkExecutor(ref List<BaseLayer<double, double>> layers)
        {
            Layers = layers;

            BaseLayer<double, double> firstLayer = Layers.First();

            firstLayer.Neurons.ForEach(neuron =>
            {
                neuron.Akson = (collection) =>
                {
                    double soma = 0;
                    // Calculating sum of output value of previous neurons multiplied by weight of current dendrite
                    for (int i = 0; i < collection.Count; i++)
                    {
                        soma += collection[i] * neuron.Dendrites[i].Weight;
                    }
                    // Applying activation function of prev sum and bias for particular layer
                    return firstLayer.ActivationFunction(soma + firstLayer.Bias);
                };
            });

            // Going thru layers from second
            Layers.Skip(1).ToList().ForEach(layer =>
            {
                layer.Neurons.ForEach(neuron =>
                {
                    neuron.Akson = (collection) =>
                    {
                        // Calculating sum of output value of previous neurons multiplied by weight of current dendrite
                        double soma = neuron.Dendrites.Select(c => c.PreviousNeuron.Akson(collection) * c.Weight).Sum();
                        // Applying activation function of prev sum and bias for particular layer
                        return layer.ActivationFunction(soma + layer.Bias);
                    };
                });
            });
        }

        public BaseNeuralParameter<double> Execute(BaseNeuralParameter<double> input)
        {
            return new BaseNeuralParameter<double>(Layers.Last().Neurons.Select(neuron => neuron.Akson(input.Collection)).ToArray());
        }
    }
}
