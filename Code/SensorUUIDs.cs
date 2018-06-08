using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothData.Code
{
    public static class SensorUUIDs
    {

        // Adding UUIDs for the Nordic UART
        public const string UUID_UART_SERV = "6e400001-b5a3-f393-e0a9-e50e24dcca9e";//Nordic UART service
        public const string UUID_UART_TX = "6e400003-b5a3-f393-e0a9-e50e24dcca9e";//TX Read Notify
        public const string UUID_UART_RX = "6e400002-b5a3-f393-e0a9-e50e24dcca9e";//RX, Write characteristic

    }
}
