﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimingAnalysis
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }
        private void experiment_start()
        {

            VisualizeForm vis_form = new VisualizeForm(int.Parse(timerTextBox.Text), int.Parse(signalTextBox.Text));
            vis_form.ShowDialog();  
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            experiment_start();
        }
    }
}