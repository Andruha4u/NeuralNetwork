using NeuralNetwork.API.Extentions;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Models;
using System;
using System.Collections.Generic;

namespace NeuralNetwork.API
{
    public class MonteCarloNeuralNetworkTrainer : ITrainable<BaseNeuralParameter<double>, BaseNeuralParameter<double>>
    {
        private readonly Func<BaseNeuralParameter<double>, BaseNeuralParameter<double>> Execute;
        private readonly List<BaseLayer<double, double>> Layers;

        public MonteCarloNeuralNetworkTrainer(ref List<BaseLayer<double, double>> layers, Func<BaseNeuralParameter<double>, BaseNeuralParameter<double>> executor)
        {
            Execute = executor;
            Layers = layers;
        }

        public void Train(List<BaseNeuralParameter<double>> inputs, List<BaseNeuralParameter<double>> outputs)
        {
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
