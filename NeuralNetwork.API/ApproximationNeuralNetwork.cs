using NeuralNetwork.API.Extentions;
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
        private double _learningRate { get; set; }

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
            _learningRate = 0.8;
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

            return new BaseNeuralParameter<double>(Layers.Last().Neurons.Select(neuron => neuron.Akson()).ToArray());
        }

        public void Train(List<BaseNeuralParameter<double>> inputs, List<BaseNeuralParameter<double>> outputs)
        {
            //double startApproximation = 0.03507;
            double startApproximation = 0.0370651;
            for (int inputInd = 0; inputInd < inputs.Count; inputInd++)
            {
                BaseNeuralParameter<double> actualResult = Execute(inputs[inputInd]);

                Layers.ForEach(layer =>
                {
                    layer.Neurons.ForEach(neuron =>
                    {
                        foreach (var dendrite in neuron.Dendrites)
                        {
                            dendrite.Weight = startApproximation;

                            BaseNeuralParameter<double> bestWeightApproximation = this.Execute(inputs[inputInd]);
                            double step = 0.1;
                            dendrite.Weight = startApproximation - step;
                            var resultL = this.Execute(inputs[inputInd]);

                            dendrite.Weight = startApproximation + step;
                            var resultR = this.Execute(inputs[inputInd]);

                            if (resultL.Collection.Decrease(bestWeightApproximation.Collection, outputs[inputInd].Collection))
                            {
                                bestWeightApproximation = resultL;
                                step = -step;
                            }
                            else if (resultR.Collection.Decrease(bestWeightApproximation.Collection, outputs[inputInd].Collection))
                            {
                                bestWeightApproximation = resultR;
                            }
                            else
                            {
                                dendrite.Weight = startApproximation;
                                continue;
                            }

                            int insightsCount = 0;
                            BaseNeuralParameter<double> newBestWeightApproximation = new BaseNeuralParameter<double>(bestWeightApproximation.Collection.ToArray());
                            do
                            {
                                if (insightsCount > 10000) break;
                                bestWeightApproximation = new BaseNeuralParameter<double>(newBestWeightApproximation.Collection.ToArray());
                                dendrite.Weight += step;
                                newBestWeightApproximation = this.Execute(inputs[inputInd]);
                                insightsCount++;
                            } while (newBestWeightApproximation.Collection.Decrease(bestWeightApproximation.Collection, outputs[inputInd].Collection));
                            dendrite.Weight -= step;
                        };
                    });
                });
            }
        }
    }
}