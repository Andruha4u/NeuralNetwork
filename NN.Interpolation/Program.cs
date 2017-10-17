using NeuralNetwork.API;
using NeuralNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NN.Interpolation
{
    class Program
    {
        static void Main(string[] args)
        {
            int inputsCount = 2;

            // First Layer
            BaseLayer<double, double> firstLayer = new BaseLayer<double, double>(2, (x) => { Console.WriteLine(x.ToString() + "\n"); return x; }, 0.01);
            firstLayer.Neurons.Add(
            new BaseNeuron<double>
            {
                Dendrites = new List<BaseDendrite<double>>(inputsCount)
                {
                    new BaseDendrite<double>(0.1),
                    new BaseDendrite<double>(0.3)
                }
            });
            firstLayer.Neurons.Add(
            new BaseNeuron<double>
            {
                Dendrites = new List<BaseDendrite<double>>(inputsCount)
                {
                    new BaseDendrite<double>(0.2),
                    new BaseDendrite<double>(0.4)
                }
            });

            // Second Layer
            BaseLayer<double, double> secondLayer = new BaseLayer<double, double>(3, (x) => Math.Pow(x, 2), 0.01);
            secondLayer.Neurons.Add(
            new BaseNeuron<double>
            {
                Dendrites = new List<BaseDendrite<double>>(2)
                {
                    new BaseDendrite<double>(0.5, firstLayer.Neurons[0]),
                    new BaseDendrite<double>(0.1, firstLayer.Neurons[1])
                }
            });
            secondLayer.Neurons.Add(
            new BaseNeuron<double>
            {
                Dendrites = new List<BaseDendrite<double>>(1)
                {
                    new BaseDendrite<double>(0.6, firstLayer.Neurons[1])
                }
            });
            secondLayer.Neurons.Add(
            new BaseNeuron<double>
            {
                Dendrites = new List<BaseDendrite<double>>(1)
                {
                    new BaseDendrite<double>(0.2, firstLayer.Neurons[0])
                }
            });

            // Third Layer
            BaseLayer<double, double> thirdLayer = new BaseLayer<double, double>(2, (x) => Math.Pow(x, 3), 0.01);
            thirdLayer.Neurons.Add(
            new BaseNeuron<double>
            {
                Dendrites = new List<BaseDendrite<double>>(2)
                {
                    new BaseDendrite<double>(0.5, firstLayer.Neurons[1]),
                    new BaseDendrite<double>(0.1, secondLayer.Neurons[1])
                }
            });
            thirdLayer.Neurons.Add(
            new BaseNeuron<double>
            {
                Dendrites = new List<BaseDendrite<double>>(3)
                {
                    new BaseDendrite<double>(0.34, firstLayer.Neurons[0]),
                    new BaseDendrite<double>(0.24, secondLayer.Neurons[0]),
                    new BaseDendrite<double>(0.54, secondLayer.Neurons[1])
                }
            });

            List<BaseLayer<double, double>> layers = new List<BaseLayer<double, double>>
            {
                firstLayer,
                secondLayer,
                thirdLayer
            };

            ApproximationNeuralNetwork neuralNetwork = new ApproximationNeuralNetwork(layers, inputsCount);

            BaseNeuralParameter<double> result = neuralNetwork.Execute(new BaseNeuralParameter<double>(new List<double> { 10, 20 }));

            Console.WriteLine(string.Join(",", result.Collection.Select(i => i.ToString()).ToArray()));

            Console.ReadKey();
        }
    }
}
