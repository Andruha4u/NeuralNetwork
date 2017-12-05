# Neural Network
Here is basic Neural Network interfaces and specific one for function approximation.
For training was used Monte Carlo method.

`
    public class BaseLayer<TInput, TOutput>
    {
        public Func<TInput, TOutput> ActivationFunction { get; set; }

        public List<BaseNeuron<TOutput>> Neurons { get; set; }

        public TOutput Bias;
    }

    public class BaseNeuron<T>
    {
        public List<BaseDendrite<T>> Dendrites { get; set; }

        public Func<T> Akson { get; set; } = () => default(T);

        public T Delta { get; set; }
    }

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
    
    
    
    public class ApproximationNeuralNetwork : BaseNeuralNetwork<double, double>,
                                              INeuralNetwork<BaseNeuralParameter<double>, BaseNeuralParameter<double>>
`
