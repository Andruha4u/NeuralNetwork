using System.Collections.Generic;

namespace NeuralNetwork.Models
{
    public class BaseNeuralParameter<TItem>
    {
        public BaseNeuralParameter(params TItem[] collection)
        {
            Collection = new List<TItem>(collection);
        }

        public List<TItem> Collection { get; set; }
    }
}
