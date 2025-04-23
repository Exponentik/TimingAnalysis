using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TimingAnalysis
{
    public partial class VisualizeForm : Form
    {
        int _signal_length;
        int _stimulation_frequency;
        int _interval;
        int _stimulation_number;
        private bool isBlack = true;
        List<String> titles = new List<String>();
        List<Dictionary<String, double[]>> withStimulation = new List<Dictionary<String, double[]>>();
        List<Dictionary<String, double[]>> withoutStimulation = new List<Dictionary<String, double[]>>();
        bool timerFlag = false;
        bool signalFlag = false;
        bool withWithoutFlag = false;
        Dictionary<string, double[]> helpD = new Dictionary<string, double[]>();
        int[] currentDataLen = new int[29];
        List<DateTime> with = new List<DateTime>();
        List<DateTime> without = new List<DateTime>();
        List<int> stimulation_timings = new List<int>();
        Random rnd = new Random();
        int[] randomize_list = new int[1000];
        


        public VisualizeForm(int interval, int signal_length, int stimulation_frequency, int stimulation_number)
        {
            InitializeComponent();
            _interval = interval;

            _signal_length = signal_length;
            _stimulation_frequency = stimulation_frequency;
            _stimulation_number = stimulation_number;
            //stimulationTimer.Start();
            for (int i = 0; i < stimulation_number; i++)
            {
                // Generate a random number
                int newValue = rnd.Next(0, _stimulation_frequency);

                // If the previous value was zero, make sure the new value isn't zero
                if (i > 0 && newValue == 0 && randomize_list[i - 1] == 0)
                {
                    // Regenerate the value until it's non-zero
                    while (newValue == 0)
                    {
                        newValue = rnd.Next(0, _stimulation_frequency);
                    }
                }

                // Assign the value to the list
                randomize_list[i] = newValue;
            }

            titles.Add("FP1");
            titles.Add("F3");
            titles.Add("C3");
            titles.Add("P3");
            titles.Add("O1");
            titles.Add("F7");
            titles.Add("T3");
            titles.Add("T5");
            titles.Add("FZ");
            titles.Add("PZ");
            titles.Add("A1-A2");
            titles.Add("FP2");
            titles.Add("F4");
            titles.Add("C4");
            titles.Add("P4");
            titles.Add("O2");
            titles.Add("F8");
            titles.Add("T4");
            titles.Add("T6");
            titles.Add("FPZ");
            titles.Add("CZ");
            titles.Add("OZ");
            titles.Add("E1");
            titles.Add("E2");
            titles.Add("E3");
            titles.Add("E4");
            titles.Add("Не используется");
            titles.Add("Дыхание");
            titles.Add("Служебный канал");
            reseiveClientControl1.Client.StartClient();
            for (int i = 0; i < 29; i++)
            {
                helpD.Add(titles[i], new double[(int)(_signal_length + (_interval / 2))]);
            }

     

        }

        private async Task startExperiment()
        {
            await Task.Delay(3000);
            Random random = new Random();
            for (int i = 0; i < _stimulation_number; i++)
            {
                var rnd = random.Next(0, 4);
                stimulation_timings.Add(rnd);
                timerFlag = true;
                if (rnd == 0)
                {
                    if (randomize_list[i] == 0)
                    {
                        flagsChanging(false);
                    }
                    else
                    {
                        flagsChanging(true);
                        mainPictureBox.BackColor = Color.White;
                        await Task.Delay(100); // Асинхронная задержка вместо Thread.Sleep(100)
                        mainPictureBox.BackColor = Color.Black;
                    }
                    with.Add(DateTime.Now);
                    await Task.Delay(_interval - 100); // Асинхронная задержка вместо Thread.Sleep(_interval)
                    
                }
                else
                {
                    await Task.Delay(rnd * (_interval / 8));
                    if (randomize_list[i] == 0)
                    {
                        flagsChanging(false);
                    }
                    else
                    {
                        flagsChanging(true);
                        mainPictureBox.BackColor = Color.White;
                        await Task.Delay(100); // Асинхронная задержка вместо Thread.Sleep(100)
                        mainPictureBox.BackColor = Color.Black;
                    }
                    with.Add(DateTime.Now);
                    await Task.Delay(_interval - 100); // Асинхронная задержка вместо Thread.Sleep(_interval)
                    
                }

                
            }
            this.Invoke((Action)(() => this.Close()));
        }
        private readonly object lockObject = new object();
        private void MessageHandler(object sender, NetManager.EventClientMsgArgs e)
        {
            lock (lockObject)
            {
                if (!signalFlag && (this.WindowState == FormWindowState.Maximized))
                {
                    signalFlag = true;
                    startExperiment();
                }
               
                Frame f = new Frame(e.Msg);
                var bucket = new double[29, 24];
                var data = f.Data;
                

                if (timerFlag)
                {
                    for (int i = 0; i < 29; i++)
                    {
                        for (int j = 0; j < 24; j++)
                        {
                            bucket[i, j] = Convert.ToDouble(data[i * 24 + j]);

                            stimulationDataAccumulation(bucket[i, j], i);
                        
                        }
                    }
                    

                }
                else
                {
                    for (int i = 0; i < 29; i++)
                    {
                        for (int j = 0; j < 24; j++)
                        {
                            bucket[i, j] = Convert.ToDouble(data[i * 24 + j]);
                        }
                    }
                }
            }



        }

        private void stimulationDataAccumulation(double currData, int i)
        {
            if (currentDataLen[i] < (int)(_signal_length +( _interval / 2)))
            {
                helpD[titles[i]][currentDataLen[i]] = currData;
                currentDataLen[i]++;
            }
            else
            {
                if (currentDataLen.All(x => x == (int)(_signal_length +( _interval / 2))))
                {
                    timerFlag = false;
                    if (withWithoutFlag)
                    {
                        withStimulation.Add(helpD);
                    }
                    else
                    {
                        withoutStimulation.Add(helpD);
                    }
                    currentDataLen = new int[29];
                    //with.Add(DateTime.Now);
                    helpD = new Dictionary<string, double[]>();
                    for (int k = 0; k < 29; k++)
                    {
                        helpD.Add(titles[k], new double[(int)(_signal_length +( _interval /2))]);
                    }
                    
                }
            }
        }


        private void flagsChanging(bool wtWto)
        {
            if (signalFlag)
            {
                withWithoutFlag = wtWto;
                
            }
        }

        

        private void VisualizeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "results");

                // Создаем директорию, если она не существует
                Directory.CreateDirectory(directoryPath);
                string fileName = $"res_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
                using (StreamWriter writer = new StreamWriter(Path.Combine(directoryPath, fileName)))
                {
                    // Записываем данные с стимулированием
                    writer.WriteLine("With Stimulation:");
                    WriteListToFile(writer, withStimulation);

                    // Разделитель между секциями
                    writer.WriteLine();

                    // Записываем данные без стимулирования
                    writer.WriteLine("Without Stimulation:");
                    WriteListToFile(writer, withoutStimulation);
                    with.AddRange(without);
                    with.Sort();

                    Console.WriteLine("Данные успешно записаны в файл.");
                }
                reseiveClientControl1.Client.StopClient();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
            
        }
        private void WriteListToFile(StreamWriter writer,
        List<Dictionary<string, double[]>> data)
        {
            int stimulationCounter = 0;
            foreach (var dictionary in data)
            {
                stimulationCounter++;
                writer.WriteLine();
                writer.WriteLine($"stimulation {stimulationCounter}");
                writer.WriteLine();
                
                for (int k = 0; k < 5; k++)
                {
                    if (k == stimulation_timings[data.IndexOf(dictionary)])
                        writer.WriteLine("stimulation data");
                    writer.WriteLine($"stimulation {(k-2)* (_interval / 8)} ms");
                    for (int i = 0; i < ((int)(_signal_length)); i++)
                    {
                        for (int j = 0; j < 29; j++)
                        {
                            writer.Write(dictionary[titles[j]][i+(int)(k*_interval/8)] + "\t");
                        }
                        writer.Write("\n");
                    }
                    writer.WriteLine();
                    writer.WriteLine("------------------------------------------------------");
                    writer.WriteLine();
                }

            }

            List<List<double>> avgList = avgListCalc(data);
            writer.WriteLine("AVG:");
            for (int k = 0; k < 5; k++)
            {
                writer.WriteLine($"stimulation {(k - 2) * (_interval/8)} ms");
                for (int i = 0; i < _signal_length; i++)
                {

                    for (int j = 0; j < 29; j++)
                    {
                        writer.Write(avgList[j][i + (int)(k * _interval / 8)] + "\t");
                    }
                    writer.Write("\n");
                }
            }
        }
        private List<List<double>> avgListCalc(List<Dictionary<string, double[]>> data)
        {
            List<List<double>> avgList = new List<List<double>>();
            for (int i = 0; i < 29; i++)
            {
                avgList.Add(new List<double>());

                for (int k = 0; k < (int)(_signal_length +( _interval / 2)); k++)
                {

                    double sum = 0;
                    for (int j = 0; j < data.Count; j++)
                    {
                        sum += data[j][titles[i]][k];
                    }
                    if (sum != 0)
                        avgList[avgList.Count - 1].Add(sum / data.Count);
                    else
                        avgList[avgList.Count - 1].Add(0);


                }

            }
            return avgList;
        }


    }
}
