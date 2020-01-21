using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace Dino
{
    class Program
    {
        //const UInt32 WM_KEYDOWN = 0x0100;
        //const int VK_F5 = 0x74;
        static Color gameOverColor = Color.FromArgb(255, 83, 83, 83);
        static bool inverse = false;
        static int neuralSize = 20;


        //[DllImport("user32.dll")]
        //static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        static void Main(string[] args)
        {
            string curFile = @"nn.txt";
            int generations = 0;
            NeuralNetwork[] nn = new NeuralNetwork[neuralSize];
            int[] scores = new int[neuralSize];
            if (File.Exists(curFile))
            {
                StreamReader sr = new StreamReader(curFile);

                for (int i = 0; i < nn.Length; i++)
                {
                    string line = sr.ReadLine();
                    int inputNodes = Convert.ToInt16(line);
                    line = sr.ReadLine();
                    int hiddenNodes = Convert.ToInt16(line);
                    line = sr.ReadLine();
                    int outputNodes = Convert.ToInt16(line);
                    nn[i] = new NeuralNetwork();
                    string[] ihString = line.Split(' ');
                    float[,] ih = new float[inputNodes, hiddenNodes];
                    float[,] ho = new float[hiddenNodes, outputNodes];
                    float[,] bias_h = new float[hiddenNodes, 1];
                    float[,] bias_o = new float[outputNodes, 1];
                    line = sr.ReadLine();
                    string[] stringArray = line.Split(' ');
                    for (int x = 0; x < inputNodes; x++)
                    {
                        for (int y = 0; y < hiddenNodes; y++)
                        {
                            ih[x, y] = (float)Convert.ToDouble(stringArray[x * y + y]);
                        }
                    }
                    line = sr.ReadLine();
                    stringArray = line.Split(' ');
                    for (int x = 0; x < hiddenNodes; x++)
                    {
                        for (int y = 0; y < outputNodes; y++)
                        {
                            ho[x, y] = (float)Convert.ToDouble(stringArray[x * y + y]);
                        }
                    }
                    line = sr.ReadLine();
                    stringArray = line.Split(' ');
                    for (int x = 0; x < hiddenNodes; x++)
                    {
                        bias_h[x, 0] = (float)Convert.ToDouble(stringArray[x]);
                    }
                    line = sr.ReadLine();
                    stringArray = line.Split(' ');
                    for (int x = 0; x < outputNodes; x++)
                    {
                        bias_o[x, 0] = (float)Convert.ToDouble(stringArray[x]);
                    }

                    nn[i].Network(inputNodes, hiddenNodes, outputNodes, ih, ho, bias_h, bias_o);
                }
                sr.Close();
            }
            else
            {
                for (int i = 0; i < nn.Length; i++)
                {
                    nn[i] = new NeuralNetwork();
                    nn[i].Network(95, 20, 3);
                    //Thread.Sleep(500);
                }
            }
            float[] outputs = new float[3];
            ChromeWrapper chrome = new ChromeWrapper();
            chrome.setChromeProcess(UrlFinder.GetActiveTabUrl());
            while (true)
            {
                generations++;

                //ChromeWrapper chrome = new ChromeWrapper(@"chrome://dino/");
                //set up neural networks


                //loop through each neural network
                for (int networkNum = 0; networkNum < nn.Length; networkNum++)
                {
                    Color[] colors = new Color[300];
                    scnShot shot = new scnShot();
                    Bitmap bitmap = shot.getScreenShot();
                    inverse = getInverse(bitmap);
                    int count = 0;
                    for (int j = 510; j < 580; j++)
                    {
                        colors[count] = bitmap.GetPixel(920, j);
                        count++;
                        colors[count] = bitmap.GetPixel(1000, j);
                        count++;
                    }
                    for (int i = 920; i < 1000; i++)
                    {
                        colors[count] = bitmap.GetPixel(i, 510);
                        count++;
                        colors[count] = bitmap.GetPixel(i, 580);
                        count++;
                    }

                    while (GameOver(colors)) //wait until game starts
                    {
                        shot = new scnShot();
                        bitmap = shot.getScreenShot();
                        inverse = getInverse(bitmap);
                        count = 0;
                        for (int j = 510; j < 580; j++)
                        {
                            colors[count] = bitmap.GetPixel(920, j);
                            count++;
                            colors[count] = bitmap.GetPixel(1000, j);
                            count++;
                        }
                        for (int i = 920; i < 1000; i++)
                        {
                            colors[count] = bitmap.GetPixel(i, 510);
                            count++;
                            colors[count] = bitmap.GetPixel(i, 580);
                            count++;
                        }
                        chrome.SendKey((char)38);//up to start the game
                        Thread.Sleep(30);
                        chrome.NoKey();
                    }
                    int score = 0;
                    while (!GameOver(colors))
                    {
                        shot = new scnShot();
                        bitmap = shot.getScreenShot();
                        inverse = getInverse(bitmap);
                        //Color[,] colorsXD = new Color[bitmap.Width, 700];
                        //for (int i = 0; i < bitmap.Width; i++) {
                        //    for(int j = 270; j < 700; j++)
                        //    {
                        //        colorsXD[i,j] = bitmap.GetPixel(i, j);
                        //    }
                        //}
                        count = 0;
                        for (int j = 510; j < 580; j++)
                        {
                            colors[count] = bitmap.GetPixel(920, j);
                            count++;
                            colors[count] = bitmap.GetPixel(1000, j);
                            count++;
                        }
                        for (int i = 920; i < 1000; i++)
                        {
                            colors[count] = bitmap.GetPixel(i, 510);
                            count++;
                            colors[count] = bitmap.GetPixel(i, 580);
                            count++;
                        }
                        //TODO
                        //send inputs into nn 
                        //send outputs to dino game
                        outputs = nn[networkNum].predict(GetInputs(bitmap));
                        //OUTPUTS -- 0 is nothing, 1 is jump, 2 is duck
                        //Console.WriteLine("Outputs 0 is {0} and 1 is {1} and 2 is {2}", outputs[0], outputs[1], outputs[2]);
                        if (outputs[0] > outputs[1] && outputs[0] > outputs[2])
                        {
                            //Console.WriteLine("0");
                            chrome.NoKey();
                        }
                        else if (outputs[1] > outputs[2])
                        {
                            //1 is biggest
                            //Console.WriteLine("1");
                            chrome.SendKey((char)38);
                        }
                        else
                        {
                            //2 is biggest
                            //Console.WriteLine("2");
                            chrome.SendKey((char)40);
                        }
                        //colors[0] = bitmap.GetPixel(920, 510);
                        //colors[1] = bitmap.GetPixel(920, 580);
                        //for (int i = 700; i < 750; i++)
                        //{
                        //    for (int j = 700; j < 750; j++)
                        //    {
                        //        bitmap.SetPixel(i, j, Color.Red);
                        //    }
                        //}
                        //Color gameOverColor = Color.FromArgb(255, 83, 83, 83);
                        //gameOver = true;
                        //for(int i = 0; i < colors.Length; i++)
                        //{
                        //    if(colors[i] != gameOverColor)
                        //    {
                        //        gameOver = false;
                        //        i = colors.Length + 1;
                        //    }
                        //}

                        // color is 255,83,83,83
                        //bitmap.Save("dinoShot2.png", ImageFormat.Png);
                        //Console.ReadLine();
                        //chrome.SendKey((char)40);//down
                        //38 is up
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        Thread.Sleep(20);
                        score++;
                    }

                    //TODO
                    //save score with nn
                    nn[networkNum].score = score;
                    //Console.ReadLine();
                    //Process[] processlist = Process.GetProcesses();
                    //Process[] processes = Process.GetProcessesByName("chrome");
                    Console.WriteLine("In generation {0} neural net {1} got score {2}", generations, networkNum, score);
                    //foreach (Process proc in processes)
                    //    PostMessage(proc.MainWindowHandle, WM_KEYDOWN, VK_F5, 0);
                }
                //TODO
                //kill half mutate 
                //add new 
                string saveDat = string.Empty;
                for (int i = 0; i < nn.Length; i++)
                {
                    saveDat = string.Concat(saveDat, nn[i].ToString());
                }
                StreamWriter sw = new StreamWriter(curFile);
                sw.Write(saveDat);
                sw.Close();

                if(generations == 10)
                {
                    saveDat = string.Empty;
                    for (int i = 0; i < nn.Length; i++)
                    {
                        saveDat = string.Concat(saveDat, nn[i].ToString());
                    }
                    sw = new StreamWriter("nn10");
                    sw.Write(saveDat);
                    sw.Close();
                }
                if (generations == 50)
                {
                    saveDat = string.Empty;
                    for (int i = 0; i < nn.Length; i++)
                    {
                        saveDat = string.Concat(saveDat, nn[i].ToString());
                    }
                    sw = new StreamWriter("nn50");
                    sw.Write(saveDat);
                    sw.Close();
                }
                if (generations == 100)
                {
                    saveDat = string.Empty;
                    for (int i = 0; i < nn.Length; i++)
                    {
                        saveDat = string.Concat(saveDat, nn[i].ToString());
                    }
                    sw = new StreamWriter("nn100");
                    sw.Write(saveDat);
                    sw.Close();
                }
                if (generations == 200)
                {
                    saveDat = string.Empty;
                    for (int i = 0; i < nn.Length; i++)
                    {
                        saveDat = string.Concat(saveDat, nn[i].ToString());
                    }
                    sw = new StreamWriter("nn200");
                    sw.Write(saveDat);
                    sw.Close();
                }

                bubbleSort(nn);
                for (int i = 0; i < neuralSize/2; i++)
                {
                    Console.WriteLine("Net {0} score: {1}", i, nn[i].score);
                    Console.WriteLine("Net {0} score: {1}", i + neuralSize/2, nn[i + neuralSize/2].score);
                    nn[i] = nn[i + neuralSize/2].mutate();
                }
                //Thread.Sleep(5000);
            }
            //TODO
            //kill half of nn
            //mutate other half to get back to full population
        }
        private static bool GameOver(Color[] colors)
        {
            bool gameOver = true;
            for (int i = 0; i < colors.Length; i++)
            {
                if (colors[i] != gameOverColor)
                {
                    gameOver = false;
                    i = colors.Length + 1;
                }
            }
            return gameOver;
        }
        private static int[] GetInputs(Bitmap bitmap)
        {
            int[] retVal = new int[95];
            int count = 0;
            for (int i = 220; i < 1600; i = i + 75)
            {
                for (int j = 550; j < 660; j = j + 25)
                {
                    if ((bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).G == 255 && bitmap.GetPixel(i, j).B == 255) || (bitmap.GetPixel(i, j).R == 0 && bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).B == 0))
                    {
                        if (inverse)
                        {
                            retVal[count] = 1;
                        }
                        else
                        {
                            retVal[count] = 0;
                        }
                    }
                    else
                    {
                        if (inverse)
                        {
                            retVal[count] = 0;
                        }
                        else
                        {
                            retVal[count] = 1;
                        }
                    }
                    //retVal[count] = bitmap.GetPixel(i, j).R;
                    //retVal[count+1] = bitmap.GetPixel(i, j).G;
                    //retVal[count+2] = bitmap.GetPixel(i, j).B;
                    count++;
                }
            }

            return retVal;
        }
        private static void bubbleSort(NeuralNetwork[] arr)
        {
            int i, j;
            for (i = 0; i < arr.Length - 1; i++)

                // Last i elements are already in place  
                for (j = 0; j < arr.Length - i - 1; j++)
                    if (arr[j].score > arr[j + 1].score)
                    {
                        NeuralNetwork temp;
                        temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
        }

        private static bool getInverse(Bitmap bitmap)
        {
            if (bitmap.GetPixel(30, 255).B == 0)
            {
                return true;
            }
            return false;
        }

    }
}
