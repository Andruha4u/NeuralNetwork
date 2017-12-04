using NeuralNetwork.API;
using NeuralNetwork.API.Mappers;
using NeuralNetwork.API.Models;
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
            ApproximationNeuralNetwork neuralNetwork = NeuralNetworkSetup.Instance.GetNeuralNetwork(@"..\..\NeuralNetwork.json");

            List<BaseNeuralParameter<double>> inputs = new List<BaseNeuralParameter<double>>();
            List<BaseNeuralParameter<double>> outputs = new List<BaseNeuralParameter<double>>();

            Func<double, double> f = (x) => 2*x*x;

            for (double i = 1; i < 10; i ++)
            {
                inputs.Add(new BaseNeuralParameter<double>(Enumerable.Repeat(i, neuralNetwork.InputsCount).ToArray()));
                outputs.Add(new BaseNeuralParameter<double>(Enumerable.Repeat(f(i), neuralNetwork.InputsCount).ToArray()));
            }
            neuralNetwork.Train(inputs,outputs);


            for (double i = 0; i < 15; i++)
            {
                BaseNeuralParameter<double> result = neuralNetwork.Execute(new BaseNeuralParameter<double>(i));
                Console.Write($"Expexted ({i}) " + string.Join(",", f(i)));
                Console.Write(" / / ");
                Console.Write($"Actual({i}) " + string.Join(",", result.Collection.Select(e => e.ToString()).ToArray()));
                Console.Write(" Error  " + (f(i) - result.Collection.First()));
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
