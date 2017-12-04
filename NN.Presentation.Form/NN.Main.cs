using NeuralNetwork.API;
using NeuralNetwork.API.Mappers;
using NeuralNetwork.API.Models;
using NeuralNetwork.Models;
using NN.Presentation.Form.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using WindowsForm = System.Windows.Forms.Form;

namespace NN.Presentation.Form
{
    public partial class NN : WindowsForm
    {
        ApproximationNeuralNetwork _neuralNetwork;
        Func<double, double> _function = (x) => 2 * x * x;


        public NN()
        {
            InitializeComponent();
            _neuralNetwork = NeuralNetworkSetup.Instance.GetNeuralNetwork(@"..\..\NeuralNetwork.json");
            VisualiseNN();
        }

        private void NNRun_Click(object sender, EventArgs e)
        {
            string actual = "Actual";
            string expected = "Expected";
            string currentChartArea = "CurrentChartArea";

            NNChart.Series.Add(actual);
            NNChart.Series.Add(expected);
            NNChart.Series[actual].ChartType = SeriesChartType.FastLine;
            NNChart.Series[expected].ChartType = SeriesChartType.FastLine;
            NNChart.Series[actual].ChartArea = currentChartArea;
            NNChart.Series[expected].ChartArea = currentChartArea;

            for (double i = 0; i < 30; i++)
            {
                BaseNeuralParameter<double> actualResult = _neuralNetwork.Execute(new BaseNeuralParameter<double>(i));
                NNChart.Series[actual].Points.AddXY(i, actualResult.Collection.First());
                NNChart.Series[expected].Points.AddXY(i, _function(i));
                
            }
        }

        private void NNTrain_Click(object sender, EventArgs e)
        {
            List<BaseNeuralParameter<double>> inputs = new List<BaseNeuralParameter<double>>();
            List<BaseNeuralParameter<double>> outputs = new List<BaseNeuralParameter<double>>();

            

            for (double i = 1; i < 10; i++)
            {
                inputs.Add(new BaseNeuralParameter<double>(Enumerable.Repeat(i, _neuralNetwork.InputsCount).ToArray()));
                outputs.Add(new BaseNeuralParameter<double>(Enumerable.Repeat(_function(i), _neuralNetwork.InputsCount).ToArray()));
            }
            _neuralNetwork.Train(inputs, outputs);
            VisualiseNN();
        }
        
        private void VisualiseNN()
        {
            NNBox.ToNN(_neuralNetwork, 110, 10);
            NNTree.ToNN(_neuralNetwork);
            NNTree.ExpandAll();
        }
    }
}
