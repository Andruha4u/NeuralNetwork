using NeuralNetwork.Interfaces;
using NeuralNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.API
{
    public class ApproximationNeuralNetwork : BaseNeuralNetwork<double, double>,
                                              INeuralNetwork<BaseNeuralParameter<double>, BaseNeuralParameter<double>>
    {
        private int _inputsCount { get; set; }

        public override List<BaseLayer<double, double>> Layers { get; set; }

        public int InputsCount
        {
            get
            {
                return _inputsCount;
            }
            set
            {
                if (Layers.First().Neurons.Select(n => n.Dendrites.Count).Distinct().Single() != value)
                {
                    throw new OperationCanceledException("Could not set inputs count because it didnt match to first layer neurons definition.");
                }
                _inputsCount = value;
            }
        }

        public ApproximationNeuralNetwork(List<BaseLayer<double, double>> layers, int inputsCount)
        {
            Layers = layers;
            InputsCount = inputsCount;
        }

        public BaseNeuralParameter<double> Execute(BaseNeuralParameter<double> input)
        {

            if (InputsCount != input.Collection.Count)
            {
                throw new OperationCanceledException("Could run thru all neural network because of wrong inputs parameters." +
                    $" Current inputs count {input.Collection.Count}, expected inputs count {InputsCount}");
            }

            BaseLayer<double, double> firstLayer = Layers.First();

            firstLayer.Neurons.ForEach(neuron =>
            {
                neuron.Akson = () =>
                {
                    double soma = 0;
                    // Calculating sum of output value of previous neurons multiplied by weight of current dendrite
                    for (int i = 0; i < InputsCount; i++)
                    {
                        soma += input.Collection[i] * neuron.Dendrites[i].Weight;
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
                    neuron.Akson = () =>
                    {
                        // Calculating sum of output value of previous neurons multiplied by weight of current dendrite
                        double soma = neuron.Dendrites.Select(c => c.PreviousNeuron.Akson() * c.Weight).Sum();
                        // Applying activation function of prev sum and bias for particular layer
                        return layer.ActivationFunction(soma + layer.Bias);
                    };
                });
            });

            return new BaseNeuralParameter<double>(Layers.Last().Neurons.Select(neuron => neuron.Akson()).ToList());
        }

        public void Train(BaseNeuralParameter<double> input, BaseNeuralParameter<double> output)
        {
            BaseNeuralParameter<double> actualResult = Execute(input);

            // TODO: change weights to make smaller error

            throw new NotImplementedException();
        }
    }
}
