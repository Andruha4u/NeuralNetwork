using NeuralNetwork.API;
using NeuralNetwork.API.Models;
using NeuralNetwork.Models;
using System.Collections.Generic;
using System.IO;

namespace NeuralNetwork.API.Mappers
{
    public static class NeuralNetworkMapper
    {
        public static ApproximationNeuralNetwork GetNeuralNetwork(this NeuralNetworkSetup nNSetup, string jsonSetupFilePath)
        {
            using (StreamReader r = new StreamReader(jsonSetupFilePath))
            {
                string json = r.ReadToEnd();
                nNSetup = NeuralNetworkSetup.FromJson(json);
            }

            List<BaseLayer<double, double>> layers = new List<BaseLayer<double, double>>();

            nNSetup.Layers.ForEach(l =>
            {
                List<BaseNeuron<double>> neurons = new List<BaseNeuron<double>>();
                l.Neurons.ForEach(n =>
                {
                    List<BaseDendrite<double>> dendrites = new List<BaseDendrite<double>>();
                    n.Dendrites.ForEach(d =>
                    {
                        BaseNeuron<double> previous = d.PreviousNeuron != null ?
                            layers[d.PreviousNeuron.LayerId].Neurons[d.PreviousNeuron.NeuronId] : null;

                        dendrites.Add(
                            new BaseDendrite<double>(d.Weight, previous)
                        );
                    });

                    neurons.Add(new BaseNeuron<double>
                    {
                        Dendrites = dendrites
                    });
                });

                layers.Add(
                    new BaseLayer<double, double>
                    {
                        Neurons = neurons,
                        Bias = l.Bias,
                        ActivationFunction = ActivationFunctionDefinition.Function[l.ActivationFunctionType]
                    }
                );
            });

            var executor = new ApproximationNeuralNetworkExecutor(ref layers);
            var trainer = new MonteCarloNeuralNetworkTrainer(ref layers, executor.Execute);

            return new ApproximationNeuralNetwork(layers, nNSetup.InputsCount, trainer, executor);
        }
    }
}
