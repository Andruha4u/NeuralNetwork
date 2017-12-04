using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NeuralNetwork.API.Models
{
    public class NeuralNetworkSetup
    {
        public int InputsCount { get; set; }
        public List<Layer> Layers { get; set; }

        public static NeuralNetworkSetup Instance
        {
            get
            {
                return new NeuralNetworkSetup();
            }
        }

        public static NeuralNetworkSetup FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<NeuralNetworkSetup>(json);
            }
            catch
            {
                throw new Exception($"An Error persists while mapping to neural network object from {json}");
            }
        }
    }
}
