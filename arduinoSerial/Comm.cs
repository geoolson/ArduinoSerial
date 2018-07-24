using System;
using System.IO.Ports;


namespace arduinoSerial
{
    public class CommManager
    {
        public int baudRate = 9600;
        public int BaudRate
        {
            get {
                if (ComPort != null)
                    return ComPort.BaudRate;
                return 0;
            }
            set
            {
                if(ComPort != null)
                    ComPort.BaudRate = value;
            }
        }
        public static SerialPort ComPort;
        public static String[] ports;

        public CommManager()
        {
            ports = SerialPort.GetPortNames();
        }
        public CommManager(String port)
        {
            ports = SerialPort.GetPortNames();
            if (ComPort != null && ComPort.PortName == port)
                return;
            else if(ComPort != null)
                ComPort.Close();
            ComPort = new SerialPort(port);
            try
            {
                ComPort.Open();
            }
            catch
            {

            }
            ComPort.WriteTimeout = 500;
        }
        public int write(String Input) { return 0;}
    }

    // converts string into its numerical byte value then passes the byte value into Serial.Write()
    class Dec : CommManager
    {
        public Dec() : base() { }
        public Dec(String input) : base(input){}
        public new int write(String input)
        {
            byte[] value;
            value = new byte[2];
            try
            {
                value[0] = byte.Parse(input);
            }
            catch (Exception)
            //if the input is greater than 255 or less than 0 set the byte value to 0
            {
                value[0] = 0;
            }
            value[1] = 0;
            try
            {
                ComPort.Write(value, 0, 1);
            }
            catch (Exception)
            {
                return -1;
            }
            return 0;
        }
    }

    // Passes one character from the input into Serial.Write()
    class ASCII : CommManager
    {
        public ASCII() : base() { }
        public ASCII(String input) : base(input) { }
        public new int write(String input)
        {
            byte[] value;
            value = new byte[2];
            try
            {
                value[0] = (byte)input[0];
            }
            catch (Exception)
            //if the input is greater than 255 or less than 0 set the byte value to 0
            {
                value[0] = 0;
            }
            value[1] = 0;
            try
            {
                ComPort.Write(value, 0, 1);
            }
            catch
            {
                return -1;
            }
            return 0;
        }
    }

    // this class may be removed and imlpemented into the base class.
    class StrSerial : CommManager
    {
        public StrSerial() : base() { }
        public StrSerial(String input) : base(input) { }

        public new int write(string Input)
        {
            ComPort.Write(Input);
            return 0;
        }
    }

    // class for managing strings containing hex values 
    class HEX : CommManager
    {
        public HEX() : base() { }
        public HEX(String input) : base(input) { }
        public new int write(String input)
        {
            int result = Convert.ToInt32(input, 16);
            byte [] value;
            value = new byte[2];
            value[0] = (byte)result;
            value[1] = 0;
            ComPort.Write(value, 0, 1);
            return 0;
        }
    }
}