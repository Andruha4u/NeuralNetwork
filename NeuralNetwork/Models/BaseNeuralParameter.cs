using System.Collections.Generic;

namespace NeuralNetwork.Models
{
    public class BaseNeuralParameter<TItem>
    {
        public List<TItem> Collection { get; set; }
    }
}
