using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.API.Extentions
{
    public static class CollectionExtentions
    {
        public static IEnumerable<double> SquareSubstraction(this IEnumerable<double> thisCollection, IEnumerable<double> collectionToApply)
        {
            return thisCollection.Zip(collectionToApply, (a, b) => (a - b) * (a - b));
        }

        public static IEnumerable<double> Substract(this IEnumerable<double> thisCollection, IEnumerable<double> collectionToSubstract)
        {
            return thisCollection.Zip(collectionToSubstract, (a, b) => a - b);
        }


        public static bool Decrease(this IEnumerable<double> thisCollection, IEnumerable<double> collectionToCompare, IEnumerable<double> baseSubstractor)
        {
            return thisCollection.SquareSubstraction(baseSubstractor).Substract(collectionToCompare.SquareSubstraction(baseSubstractor)).All(item => item < 0);
        }
    }
}
