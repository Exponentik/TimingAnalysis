using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace TimingAnalysis
{
    public partial class VisualizeForm : Form
    {
        int _signal_length;
        private bool isBlack = true;
        List<String> titles = new List<String>();
        List<Dictionary<String, List<double>>> withStimulation = new List<Dictionary<String, List<double>>>();
        List<Dictionary<String, List<double>>> withoutStimulation = new List<Dictionary<String, List<double>>>();
        bool timerFlag = false;
        bool signalFlag = false;
        bool withWithoutFlag = false;
        Dictionary<string, List<double>> helpD = new Dictionary<string, List<double>>();
        int n = 0;
        int m = 0;
        List<DateTime> with = new List<DateTime>();
        List<DateTime> without = new List<DateTime>();


        public VisualizeForm(int interval, int signal_length)
        {
            InitializeComponent();
            stimulationTimer.Interval = interval;
            _signal_length = signal_length;
            stimulationTimer.Start();

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
                helpD.Add(titles[i], new List<double>());
            }

        }
        private readonly object lockObject = new object();
        private void MessageHandler(object sender, NetManager.EventClientMsgArgs e)
        {
            lock (lockObject)
            {
                signalFlag = true;
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

                            if (withWithoutFlag)
                            {
                                // Проверка, что размер данных меньше _signal_length перед добавлением
                                if (helpD.Any(x => x.Value.Count < _signal_length))
                                {
                                    if (helpD[titles[i]].Count < _signal_length)
                                        helpD[titles[i]].Add(bucket[i, j]);
                                }
                                else
                                {
                                    // Данные собраны, меняем флаг
                                    timerFlag = false;
                                    if (helpD.All(x => x.Value.Count == _signal_length))
                                    {
                                        if ((withWithoutFlag))
                                        {
                                            withStimulation.Add(helpD);

                                        }
                                        else
                                        {
                                            withoutStimulation.Add(helpD);


                                        }
                                        with.Add(DateTime.Now);
                                        helpD = new Dictionary<string, List<double>>();
                                        for (int k = 0; k < 29; k++)
                                        {
                                            helpD.Add(titles[k], new List<double>());
                                        }
                                        break;
                                    }
                                   
                                }
                            }
                            else
                            {
                                // Проверка, что размер данных меньше _signal_length перед добавлением
                                if (helpD.Any(x => x.Value.Count < _signal_length))
                                {
                                    if (helpD[titles[i]].Count < _signal_length)
                                        helpD[titles[i]].Add(bucket[i, j]);
                                }
                                else
                                {
                                    // Данные собраны, меняем флаг
                                    timerFlag = false;
                                    
                                    if (helpD.All(x => x.Value.Count == _signal_length))
                                    {
                                        if ((withWithoutFlag))
                                        {
                                            withStimulation.Add(helpD);

                                        }
                                        else
                                        {
                                            withoutStimulation.Add(helpD);


                                        }
                                        without.Add(DateTime.Now);
                                        helpD = new Dictionary<string, List<double>>();
                                        for (int k = 0; k < 29; k++)
                                        {
                                            helpD.Add(titles[k], new List<double>());
                                        }
                                        break;
                                    }
                                }
                            }
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
        private void stimulationTimer_Tick(object sender, EventArgs e)
        {

            colorChangeTimer.Start();
        }

        private void colorChangeTimer_Tick(object sender, EventArgs e)
        {
            var rnd = new Random().Next(0, 3);
            if (isBlack)
            {
                
                
                if (rnd == 0)
                {

                    if (signalFlag)
                    {
                        n++;
                        withWithoutFlag = false;
                        timerFlag = true;
                    }
                }
                else
                {
                    mainPictureBox.BackColor = Color.White;
                    
                    if (signalFlag)
                    {
                        m++;
                        withWithoutFlag = true;
                        timerFlag = true;
                    }
                }

            }
            else
            {
                if (rnd != 0)
                {
                    mainPictureBox.BackColor = Color.Black;
                    colorChangeTimer.Stop();
                }
            }

            isBlack = !isBlack;
            
        }

        private void VisualizeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("C:\\Users\\user\\Desktop\\TimingAnalysis\\TimingAnalysis\\res.txt"))
                {
                    // Записываем данные с стимулированием
                    writer.WriteLine("With Stimulation:");
                    WriteListToFile(writer, withStimulation);

                    // Разделитель между секциями
                    writer.WriteLine();

                    // Записываем данные без стимулирования
                    writer.WriteLine("Without Stimulation:");
                    WriteListToFile(writer, withoutStimulation);

                    Console.WriteLine("Данные успешно записаны в файл.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
            reseiveClientControl1.Client.StopClient();
        }
        private void WriteListToFile(StreamWriter writer,
        List<Dictionary<string, List<double>>> data)
        {
            //foreach (var dictionary in data)
            //{
            //    foreach (var kvp in dictionary)
            //    {
            //        // Записываем ключ
            //        writer.Write(kvp.Key + ": ");

            //        // Записываем значения в список чисел
            //        writer.WriteLine(string.Join(", ", kvp.Value));
            //    }
            //    writer.WriteLine(); // Пустая строка между словарями
            //}
            List<List<double>> avgList = new List<List<double>>();
            for(int i = 0; i< 29; i++)
            {
                avgList.Add(new List<double>());

                for (int k = 0; k < _signal_length; k++) 
                {

                    double sum = 0;
                    for (int j = 0; j < data.Count; j++)
                    {
                        sum += data[j][titles[i]][k];
                    }
                    if(sum != 0)
                        avgList[avgList.Count - 1].Add(sum / data.Count);
                    else
                    avgList[avgList.Count - 1].Add(0);


                }

            }
            writer.WriteLine("AVG:");
            foreach (var kvp in titles)
            {
                // Записываем ключ
                writer.Write(kvp + ": ");

                // Записываем значения в список чисел
                writer.WriteLine(string.Join(", ", avgList[titles.IndexOf(kvp)]));
            }

        }
    }
}
