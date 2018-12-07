﻿using System;
using System.IO;
using System.Windows.Forms;

using ArduinoCom;

namespace VirtualPanel
{
    public partial class MsgLogForm : Form
    {
        private int lines = 0;
        private ArduinoPort arduinoport;
        private int MsgNum = 0;
        private bool onHold=false;

        public MsgLogForm(ArduinoPort port)
        {
            arduinoport = port;
            InitializeComponent();
            arduinoport.MessageSent += Arduinoport_MessageSent;
            arduinoport.MessageReceived += Arduinoport_MessageReceived;
        }

        private void Arduinoport_MessageReceived(object sender, MessageEventArgs<object> e)
        {
            WriteMonitor(MsgNum++ + "  R  " + ((ChannelId)e.ChannelID).ToString() + "\t" + e.Type.ToString() + "\t" + e.Data.ToString());
        }

        private void Arduinoport_MessageSent(object sender, MessageEventArgs<object> e)
        {
            WriteMonitor(MsgNum++ + "  S  " + ((ChannelId)e.ChannelID).ToString() + "\t" + e.Type.ToString() + "\t" + e.Data.ToString());

        }

        public void WriteMonitor(String inputline)
        {
            if (!onHold)
            {
                if (lines >= 2000)
                {
                    logmonitor.Clear();
                    lines = 0;
                }

                logmonitor.AppendText(inputline + Environment.NewLine); 
                logmonitor.ScrollToCaret();
                lines++;
            }
        }

        public void LogFormClear()
        {
            logmonitor.Clear();
            lines = 0;
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Visible = false;
            e.Cancel = true;
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            arduinoport.Reset();
        }

        private void LogHoldButton_Click(object sender, EventArgs e)
        {
            onHold = !onHold;

            if (onHold)
                LogHoldButton.Text = "resume";
            else
                LogHoldButton.Text = "hold";
            
        }

        private void LogClearButton_Click(object sender, EventArgs e)
        {
            logmonitor.Clear();
            lines = 0;
        }

        private void LogFileButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, logmonitor.Text);
            }
            
        }
    }
}
