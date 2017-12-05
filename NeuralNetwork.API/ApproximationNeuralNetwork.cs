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

        private readonly ITrainable<BaseNeuralParameter<double>, BaseNeuralParameter<double>> Trainer;
        private readonly IExecutable<BaseNeuralParameter<double>, BaseNeuralParameter<double>> Executor;

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

        public ApproximationNeuralNetwork(List<BaseLayer<double, double>> layers, int inputsCount,
            ITrainable<BaseNeuralParameter<double>, BaseNeuralParameter<double>> trainer,
            IExecutable<BaseNeuralParameter<double>, BaseNeuralParameter<double>> executor)
        {
            Layers = layers;
            InputsCount = inputsCount;
            Trainer = trainer;
            Executor = executor;
        }

        public BaseNeuralParameter<double> Execute(BaseNeuralParameter<double> input)
        {
            if (InputsCount != input.Collection.Count)
            {
                throw new OperationCanceledException("Could run thru all neural network because of wrong inputs parameters." +
                    $" Current inputs count {input.Collection.Count}, expected inputs count {InputsCount}");
            }

            return Executor.Execute(input);
        }

        public void Train(List<BaseNeuralParameter<double>> inputs, List<BaseNeuralParameter<double>> outputs) => Trainer.Train(inputs, outputs);
    }
}