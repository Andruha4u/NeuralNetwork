using System.Collections.Generic;

namespace NeuralNetwork.Models
{
    public class BaseNeuralParameter<TItem>
    {
        public BaseNeuralParameter(List<TItem> collection)
        {
            Collection = collection;
        }

        public List<TItem> Collection { get; set; }
    }
}
