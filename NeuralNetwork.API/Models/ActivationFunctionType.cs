using System;
using System.Collections.Generic;

namespace NeuralNetwork.API.Models
{
    public enum ActivationFunctionType
    {
        Linear = 1,
        Square = 2,
        Perceptrone = 3
    }

    public static class ActivationFunctionDefinition
    {
        public static Dictionary<ActivationFunctionType, Func<double, double>> Function = new Dictionary<ActivationFunctionType, Func<double, double>>
        {
            {ActivationFunctionType.Linear, x => x },
            {ActivationFunctionType.Square, x => x*x },
            {ActivationFunctionType.Perceptrone, x => x > 0 ? 1 : 0}
        };
    }
}
