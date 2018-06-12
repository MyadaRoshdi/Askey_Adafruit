using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml;
using Windows.Storage.Streams;
using BioSensor;
using System.Diagnostics;

namespace BluetoothData.Code
{
    public class SensorsViewModel : INotifyPropertyChanged//, IDisposable
    {
        DispatcherTimer timer = new DispatcherTimer();
        DeviceInformation device;
        BluetoothLEDevice leDevice;
        string hrmHeartRate = "";

        public SensorsViewModel(DeviceInformation info)
        {
            this.device = info;
        }

        public void StartSendingData()
        {
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            UpdateAllData();
        }

        public void StopReceivingData()
        {
            timer.Stop();
        }
        

        public async void UpdateAllData()
        {
            leDevice = await BluetoothLEDevice.FromIdAsync(device.Id);
            string selector = "(System.DeviceInterface.Bluetooth.DeviceAddress:=\"" + leDevice.BluetoothAddress.ToString("X") + "\")";

            var services = await leDevice.GetGattServicesAsync();

            foreach (var service in services.Services)
            {
                switch (service.Uuid.ToString())
                {
                    case SensorUUIDs.UUID_UART_SERV:
                    {
                        var characteristics = await service.GetCharacteristicsAsync();

                        foreach (var character in characteristics.Characteristics)
                        {
                            switch (character.Uuid.ToString())
                            {
                                case SensorUUIDs.UUID_UART_RX:
                                {
                                    var writer = new DataWriter();
                                    // DATA:SET command not yet working
                                    //string message = "DATA:SET #22046524f4d205741544348\n";
                                    //writer.WriteString(message);
                                    //await character.WriteValueAsync(writer.DetachBuffer());

                                    // Reading and displaying heartrate from BioSensor
                                            RateSensor bs = new RateSensor();
                                            bs.RateSensorInit();
                                           // await Task.Delay(1000);
                                            bs.RateMonitorON();
                                            hrmHeartRate = bs.GetHeartRate();
                                            Debug.WriteLine($"Current heartrate: {hrmHeartRate}");
                                            // hrmHeartRate = bs.I2C_ReadRegData(HrmSensor, AS7000_REG_HRM_HEARTRATE);
                                            //await Task.Delay(1);



                                            //writing TRIGGER to the AdafruitBT
                                            writer.WriteString("TRIGGER\n");
                                            //writing current HeartRate to the Adafruit bluetooth 
                                            //writer.WriteString(hrmHeartRate);
                                    await character.WriteValueAsync(writer.DetachBuffer());
                                }
                                break;
                            }
                        }
                    }
                    break;
                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
       
    }
}
