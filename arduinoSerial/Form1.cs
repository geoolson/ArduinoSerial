using System;
using System.Windows.Forms;
using System.IO.Ports;

namespace arduinoSerial
{
    public partial class Form1 : Form
    {
        private int state;
        dynamic[] comm;
        public static String[] ports = SerialPort.GetPortNames();
        public String[] baudRates = {"300","1200","2400","4800","9600","19200",
                                     "38400","57600","74880","115200","230400",
                                     "250000","500000","1000000","2000000"};

        public Form1()
        {
            InitializeComponent();
            comm = new dynamic[4];
            InputOptions.Items.Add("DEC");
            InputOptions.Items.Add("ASCII");
            InputOptions.Items.Add("String");
            InputOptions.Items.Add("HEX");
            for (int i = 0; i < ports.Length; ++i)
            {
                comboBox1.Items.Add(ports[i]);
            }
            for (int i = 0; i < baudRates.Length; ++i)
            {
                comboBox2.Items.Add(baudRates[i]);
            }
        }

        // event for sending a string to be sent on Serial
        private void Send_Click(object sender, EventArgs e)
        {
            if (comm[state] == null)
            {
                label2.Text = "ERROR: COM Port or Baud Rate not set.";
                return;
            }
            comm[state].write(txtInput.Text);
        }

        // change the desired format for input
        private void InputOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            state = InputOptions.SelectedIndex;
        }

        // changes the COM port
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //com port selected
            String selection = comboBox1.SelectedItem.ToString();
            comm[0] = new Dec(selection);
            comm[1] = new ASCII(selection);
            comm[2] = new StrSerial(selection);
            comm[3] = new HEX(selection);
            CommManager.ComPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }

        // adds data recieved on serial port to the text box
        private void DataReceivedHandler(
                        object sender,
                        SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke((MethodInvoker)delegate ()
                {
                    textBox1.Text += indata + "\n";
                });
            }
        }

        // changes the baud rate to user-selected option
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comm[state] != null)
                comm[state].BaudRate = Int32.Parse(comboBox2.SelectedItem.ToString());
        }
    }
}