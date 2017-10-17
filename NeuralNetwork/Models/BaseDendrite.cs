namespace NeuralNetwork.Models
{
    public class BaseDendrite<T>
    {
        public BaseDendrite(double weight, BaseNeuron<T> previousNeuron = null)
        {
            Weight = weight;
            PreviousNeuron = previousNeuron;
        }

        public BaseNeuron<T> PreviousNeuron { get; set; }

        public double Weight { get; set; }
    }
}
