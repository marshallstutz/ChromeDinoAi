using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dino
{
    class NeuralNetwork
    {
        int inputNodes;
        int hiddenNodes;
        int outputNodes;
        float[,] weights_ih;
        float[,] weights_ho;
        float[,] bias_h;
        float[,] bias_o;
        public int score { get; set; }
        public void Network(NeuralNetwork neuralNetwork)
        {
            inputNodes = neuralNetwork.inputNodes;
            hiddenNodes = neuralNetwork.hiddenNodes;
            outputNodes = neuralNetwork.outputNodes;
            weights_ih = neuralNetwork.weights_ih;
            weights_ho = neuralNetwork.weights_ho;
            bias_h = neuralNetwork.bias_h;
            bias_o = neuralNetwork.bias_o;
        }
        public void Network(int inputNodes, int hiddenNodes, int outputNodes)
        {
            this.inputNodes = inputNodes;
            this.hiddenNodes = hiddenNodes;
            this.outputNodes = outputNodes;

            weights_ih = new float[inputNodes, hiddenNodes];
            weights_ho = new float[hiddenNodes, outputNodes];
            initializeArray(weights_ho);
            Thread.Sleep(50);
            initializeArray(weights_ih);
            Thread.Sleep(50);
            bias_h = new float[hiddenNodes,1];
            bias_o = new float[outputNodes,1];
            initializeArray(bias_h);
            Thread.Sleep(50);
            initializeArray(bias_o);
            Thread.Sleep(50);
        }

        override public string ToString()
        {
            string retVal = inputNodes.ToString() + "\n";
            retVal = string.Concat(retVal, hiddenNodes.ToString(), "\n");
            retVal = string.Concat(retVal, outputNodes.ToString(), "\n");
            for (int x = 0; x < inputNodes; x++)
            {
                for (int y = 0; y < hiddenNodes; y++)
                {
                    retVal = string.Concat(retVal, weights_ih[x,y].ToString() + ' ');
                }
            }
            retVal = string.Concat(retVal, "\n");
            for (int x = 0; x < hiddenNodes; x++)
            {
                for (int y = 0; y < outputNodes; y++)
                {
                    retVal = string.Concat(retVal, weights_ho[x, y].ToString() + ' ');
                }
            }
            retVal = string.Concat(retVal, "\n");
            for (int x = 0; x < hiddenNodes; x++)
            {
                retVal = string.Concat(retVal, bias_h[x, 0].ToString() + ' ');
            }
            retVal = string.Concat(retVal, "\n");
            for (int x = 0; x < outputNodes; x++)
            {
                retVal = string.Concat(retVal, bias_o[x, 0].ToString() + ' ');
            }
            retVal = string.Concat(retVal, "\n");
            return retVal;
        }
        public void Network(int inputNodes, int hiddenNodes, int outputNodes, float[,] ih, float[,] ho, float[,] bias_h, float[,] bias_o)
        {
            this.inputNodes = inputNodes;
            this.hiddenNodes = hiddenNodes;
            this.outputNodes = outputNodes;
            weights_ih = ih;
            weights_ho = ho;
            this.bias_h = bias_h;
            this.bias_o = bias_o;
        }

        private void initializeArray(float[,] arr)
        {
            Random random = new Random();
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for(int j = 0; j < arr.GetLength(1); j++)
                {
                    double val = random.NextDouble(); // range 0.0 to 1.0
                    val -= 0.5; // expected range now -0.5 to +0.5
                    val *= 2; // expected range now -1.0 to +1.0
                    arr[i, j] = (float)val;
                }
            }
        }

        public float[] predict(int[] inputs)
        {
            float[] hidden = new float[hiddenNodes];
            float[] output = new float[outputNodes];
            for (int i = 0; i < hiddenNodes; i++)
            {
                float total = 0;
                for (int j = 0; j < inputNodes; j++)
                {
                    total += weights_ih[j, i] * inputs[j];
                }
                hidden[i] = total + bias_h[i,0];
            }
            for (int i = 0; i < outputNodes; i++)
            {
                float total = 0;
                for (int j = 0; j < hiddenNodes; j++)
                {
                    total += weights_ho[j, i] * hidden[j];
                }
                output[i] = total + bias_o[i, 0];
            }
            return output;
        }
        public NeuralNetwork mutate()
        {
            NeuralNetwork newNet = new NeuralNetwork();
            newNet.Network(inputNodes, hiddenNodes, outputNodes);
            Random random = new Random();
            for (int i = 0; i < weights_ih.GetLength(0); i++)
            {
                for (int j = 0; j < weights_ih.GetLength(1); j++)
                {
                    double val = random.NextDouble(); // range 0.0 to 1.0
                    val -= 0.5; // expected range now -0.5 to +0.5
                    val *= 2; // expected range now -1.0 to +1.0
                    newNet.weights_ih[i, j] = weights_ih[i,j] * (float).1 * (float)val + weights_ih[i,j];
                }
            }
            for (int i = 0; i < weights_ho.GetLength(0); i++)
            {
                for (int j = 0; j < weights_ho.GetLength(1); j++)
                {
                    double val = random.NextDouble(); // range 0.0 to 1.0
                    val -= 0.5; // expected range now -0.5 to +0.5
                    val *= 2; // expected range now -1.0 to +1.0
                    newNet.weights_ho[i, j] = weights_ho[i,j] * (float).1 * (float)val + weights_ho[i,j];
                }
            }
            for (int i = 0; i < bias_h.GetLength(0); i++)
            {
                for (int j = 0; j < bias_h.GetLength(1); j++)
                {
                    double val = random.NextDouble(); // range 0.0 to 1.0
                    val -= 0.5; // expected range now -0.5 to +0.5
                    val *= 2; // expected range now -1.0 to +1.0
                    newNet.bias_h[i, j] = bias_h[i,j] * (float).1 * (float)val + bias_h[i,j];
                }
            }
            for (int i = 0; i < bias_o.GetLength(0); i++)
            {
                for (int j = 0; j < bias_o.GetLength(1); j++)
                {
                    double val = random.NextDouble(); // range 0.0 to 1.0
                    val -= 0.5; // expected range now -0.5 to +0.5
                    val *= 2; // expected range now -1.0 to +1.0
                    newNet.bias_o[i, j] = bias_o[i,j] * (float).1 * (float)val + bias_o[i,j];
                }
            }
            newNet.score = 9;
            return newNet;
        }
    }
}
