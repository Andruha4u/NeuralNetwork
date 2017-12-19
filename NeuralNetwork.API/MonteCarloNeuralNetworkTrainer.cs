using NeuralNetwork.API.Extentions;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.API
{
    public class IterationResult
    {
        public List<double> Weights { get; set; }
        public double Energy { get; set; }
    }

    public class MonteCarloNeuralNetworkTrainer : ITrainable<BaseNeuralParameter<double>, BaseNeuralParameter<double>>
    {
        private readonly Func<BaseNeuralParameter<double>, BaseNeuralParameter<double>> Execute;
        private readonly List<BaseLayer<double, double>> Layers;
        private IterationResult _BestPossibleWeigths;

        public MonteCarloNeuralNetworkTrainer(ref List<BaseLayer<double, double>> layers, Func<BaseNeuralParameter<double>, BaseNeuralParameter<double>> executor)
        {
            Execute = executor;
            
            Layers = layers;
        }

        public void Train(List<BaseNeuralParameter<double>> input, List<BaseNeuralParameter<double>> output)
        {
            double _startPoint = -2;
            double _endPoint = 2;
            double _step = 1;

            List<BaseDendrite<double>> dendrides = Layers.SelectMany(layer => layer.Neurons.SelectMany(neurone => neurone.Dendrites)).ToList();
            dendrides.ForEach(d =>
            {
                d.Weight = _startPoint;
            });

            _BestPossibleWeigths = new IterationResult
            {
                Weights = dendrides.Select(el => el.Weight).ToList(),
                Energy = RegressionEnergyFunctional(input, output)
            };

            for (int i = 0; i < dendrides.Count; i++)
            {
                var dendride = dendrides[i];
                dendride.Weight += _step;

                var currEnergy = RegressionEnergyFunctional(input, output);

                // Resertting energy is it is better the particular one
                if (currEnergy < _BestPossibleWeigths.Energy)
                {
                    _BestPossibleWeigths.Weights.Clear();
                    _BestPossibleWeigths.Energy = currEnergy;
                    _BestPossibleWeigths.Weights = dendrides.Select(el => el.Weight).ToList();
                }

                if (dendride.Weight > _endPoint)
                {
                    // For all dendrides bedore and particular one set start weight
                    dendrides.GetRange(0, i + 1).ForEach(d =>
                    {
                        d.Weight = _startPoint;
                    });
                }
                else
                {
                    // Go to start
                    i = -1;
                }
            }

            for (int i = 0; i < dendrides.Count; i++)
            {
                dendrides[i].Weight = _BestPossibleWeigths.Weights[i];
            }
        }

        private double RegressionEnergyFunctional(List<BaseNeuralParameter<double>> input, List<BaseNeuralParameter<double>> output)
        {
            double energy = 0;
            for (int i = 0; i < input.Count; i++)
            {
                double element = output[i].Collection.Zip(Execute(input[i]).Collection, (a, b) => a - b).First();
                energy += Math.Pow(element, 2);
            }

            return energy;
        }
    }
}
