namespace NeuralNetwork.API.Models
{
    public class PreviousNeuron
    {
        public int LayerId { get; set; }
        public int NeuronId { get; set; }
    }

    public class Dendrite
    {
        public double Weight { get; set; }
        public PreviousNeuron PreviousNeuron { get; set; }
    }
}