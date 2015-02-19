//
//  LGTVControl.cs
//
//  Author:
//       Jakub Vanek <vanek.jakub4@seznam.cz>
//
//  Copyright (c) 2014 GPL v. 3.0
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.


#define DENY_HIGHVOLUME

using System;
using System.IO.Ports;
using Thread = System.Threading.Thread;
using System.Collections.Generic;
using LGTVControl.Commands;

namespace LGTVControl
{
    /// <summary>
    /// LG TV controlling interface.
    /// </summary>
    public class LGTVControl
    {
        /// <summary>
        /// Timeout for serial port
        /// </summary>
        public const int TIMEOUT = 1000;


        /// <summary>
        /// Gets a value indicating whether is serial connection open.
        /// </summary>
        /// <value><c>true</c> if open; otherwise, <c>false</c>.</value>
        public bool Open { get { return port.IsOpen; } }

        public int ID { get; private set; }

        /// <summary>
        /// Scheduled returns of data
        /// </summary>
        public Queue<Data> returns;



        // Port
        public SerialPort port;
        Object locker;
        // Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TVControl.LGTV"/> class.
        /// </summary>
        /// <param name="device">Device, /dev/ttyS[0-...] or COM[1-...] -> /dev/ttyS3 or COM1</param>
        /// <param name="id">TV ID</param>
        public LGTVControl(string device, int id)
        {
            port = new SerialPort(device, 9600, Parity.None, 8, StopBits.One)
            {
                NewLine = "\r",
                ReadTimeout = TIMEOUT,
                WriteTimeout = TIMEOUT
            };
            this.ID = id;
            returns = new Queue<Data>();
            locker = new Object();
        }



        // Init/Close
        /// <summary>
        /// Init this instance.
        /// </summary>
        public void Init()
        {
            port.Open();
        }

        /// <summary>
        /// Close this instance.
        /// </summary>
        public void Close()
        {
            port.Close();
        }
        /// <summary>
        /// Return data types
        /// </summary>
        public enum Data
        {
            Power,
            AspectRatio,
            ScreenMute,
            VolumeMute,
            VolumeControl,
            Contrast,
            Brightness,
            Colour,
            Tint,
            Sharpness,
            OSD,
            RCLock,
            Treble,
            Bass,
            Balance,
            ColourTemperature,
            EnergySaving,
            Tune,
            ProgrammeAddSkip,
            BackLight,
            Input,
            String,
            Hide
        }
        public void Clear()
        {
            port.ReadTimeout = 20;
            port.ReadExisting();
            port.ReadTimeout = TIMEOUT;
        }
        public event Events.IntegerReturn PowerReturn;
        public event Events.IntegerReturn AspectRatioReturn;
        public event Events.IntegerReturn ScreenMuteReturn;
        public event Events.IntegerReturn VolumeMuteReturn;
        public event Events.IntegerReturn VolumeControlReturn;
        public event Events.IntegerReturn ContrastReturn;
        public event Events.IntegerReturn BrightnessReturn;
        public event Events.IntegerReturn ColourReturn;
        public event Events.IntegerReturn TintReturn;
        public event Events.IntegerReturn SharpnessReturn;
        public event Events.IntegerReturn OSDReturn;
        public event Events.IntegerReturn RCLockReturn;
        public event Events.IntegerReturn TrebleReturn;
        public event Events.IntegerReturn BassReturn;
        public event Events.IntegerReturn BalanceReturn;
        public event Events.IntegerReturn ColourTemperatureReturn;
        public event Events.IntegerReturn EnergySavingReturn;
        public event Events.TuneReturn TuneReturn;
        public event Events.IntegerReturn prgASReturn;
        public event Events.IntegerReturn BackLightReturn;
        public event Events.IntegerReturn InputReturn;
        public event Events.StringReturn StringReturn;
        public event Events.StringReturn HiddenReturn;

        /// <summary>
        /// Write a command with data for specific TV to serial port
        /// </summary>
        /// <param name="command">Command</param>
        /// <param name="data">Data.</param>
        void Write(string command, string data)
        {
            port.WriteLine(String.Format("{0} {1} {2}", command, ID, data));
        }
        /// <summary>
        /// Write the specified command.
        /// </summary>
        /// <param name="command">Command.</param>
        public void Write(string command)
        {
            port.WriteLine(command);
        }


        // Sending functions
        /// <summary>
        /// Send the specified command with specified data.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="data">Data.</param>
        /// <param name="requestAnswer">Whether to request answer from the TV or not.</param>
        /// <param name="answer">Answered value or null.</param>
        /// <returns>Success or fail</returns>
        public bool Send(string command, string data, bool requestAnswer, out string answer)
        {
            lock (locker)
            {
                answer = "0";
                if (!this.Open)
                    throw new ConnNotOpenException("Connection not opened!!!");
                if (!requestAnswer)
                {
                    try
                    {
                        Write(command, data);
                        port.ReadTimeout = TIMEOUT / 4;
                        bool success1 = ParseReturn(port.ReadTo("x"), out answer);
                        return success1;
                    }
                    catch (TimeoutException)
                    {
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        answer = "0";
                        return false;
                    }
                    finally
                    {
                        port.ReadTimeout = TIMEOUT;
                    }
                }


                bool success = false;
                int remaining = 1;
                bool ok = false;
                while (!ok)
                {
                    try
                    {
                        Write(command, data);
                        success = ParseReturn(port.ReadTo("x"), out answer);
                        ok = true;
                    }
                    catch (TimeoutException)
                    {
                        if (remaining != 0)
                        {
                            #if DEBUG
                        Console.Error.WriteLine("warn: timeout reached, remaining {0}", remaining);
                            #endif
                            remaining--;
                            continue;
                        }
                        else
                        {
                            #if DEBUG
                        Console.Error.WriteLine("warn: timeout reached, no answer");
                            #endif
                            break;
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        answer = "0";
                        return false;
                    }
                }
                return success; 
            }
        }


        /// <summary>
        /// Parses the TV return code.
        /// </summary>
        /// <returns>The .</returns>
        /// <param name="retval">Retval.</param>
        /// <param name = "answer"></param>
        public bool ParseReturn(string retval, out string answer)
        {
            //         ||[    0   ]   [ 1]   [      2       ]||
            // Format: ||[Command2][ ][ID][ ][OK/NG][DATA][x]||
            //         ||XXXXXXXXXX[ ]XXXX[ ][OK/NG__DATA]---||
            // Example: 'a 1 OK01x'
            string data = retval.Substring(5, retval.Length - 5);
            string status = data.Substring(0, 2);
            answer = data.Substring(2, data.Length - 2);
            return status == "OK";
        }

        bool Send(string command, bool request, int data, out int result)
        {
            string actual_string;
            string data_string = Hex.ToHex(data);
            bool success = Send(command, data_string, request, out actual_string);
            if (success)
                result = Hex.FromHex(actual_string);
            else
                result = 0;
            return success;
        }
        /// <summary>
        /// Sends command with specified integer as an argument and schedules return.
        /// </summary>
        /// <param name="CMD">CMD</param>
        /// <param name="value">Argument</param>
        public void SendRetInt(string CMD, int value)
        {
            Write(CMD, Hex.ToHex(value));
            addRet(CMD);
        }
        /// <summary>
        /// Sends command with specified string as an argument and schedules return.
        /// </summary>
        /// <param name="CMD">CMD</param>
        /// <param name="value">Argument</param>
        public void SendRetString(string CMD, string value)
        {
            Write(CMD, value);
            addRet(CMD);
        }

        void addRet(string CMD)
        {
            switch (CMD)
            {
                case AspectRatio.CMD:
                    returns.Enqueue(Data.AspectRatio);
                    break;
                case AutoConfig.CMD:
                    returns.Enqueue(Data.Hide);
                    break;
                case Balance.CMD:
                    returns.Enqueue(Data.Balance);
                    break;
                case Bass.CMD:
                    returns.Enqueue(Data.Bass);
                    break;
                case Brightness.CMD:
                    returns.Enqueue(Data.Brightness);
                    break;
                case ColorTemp.CMD:
                    returns.Enqueue(Data.ColourTemperature);
                    break;
                case Colour.CMD:
                    returns.Enqueue(Data.Colour);
                    break;
                case Contrast.CMD:
                    returns.Enqueue(Data.Contrast);
                    break;
                case EnergySaving.CMD:
                    returns.Enqueue(Data.EnergySaving);
                    break;
                case Input.CMD:
                    returns.Enqueue(Data.Input);
                    break;
                case OSDshow.CMD:
                    returns.Enqueue(Data.OSD);
                    break;
                case Power.CMD:
                    returns.Enqueue(Data.Power);
                    break;
                case ProgrammeUtil.CMD:
                    returns.Enqueue(Data.ProgrammeAddSkip);
                    break;
                case RCControl.CMD:
                    returns.Enqueue(Data.Hide);
                    break;
                case ScreenMute.CMD:
                    returns.Enqueue(Data.ScreenMute);
                    break;
                case Sharpness.CMD:
                    returns.Enqueue(Data.Sharpness);
                    break;
                case Tint.CMD:
                    returns.Enqueue(Data.Tint);
                    break;
                case Treble.CMD:
                    returns.Enqueue(Data.Treble);
                    break;
                case Tune.CMD:
                    returns.Enqueue(Data.Tune);
                    break;
                case VolumeControl.CMD:
                    returns.Enqueue(Data.VolumeControl);
                    break;
                case VolumeMute.CMD:
                    returns.Enqueue(Data.VolumeMute);
                    break;
                default:
                    returns.Enqueue(Data.Hide);
                    break;
            }
        }
        private void UseIntFromString(String hexData, Action<int> call)
        {
            String data;
            bool success = ParseReturn(hexData, out data);
            if (success)
                call(Hex.FromHex(data));
            else
                call(-1);
        }
        /// <summary>
        /// Handles scheduled returns
        /// </summary>
        public void PollEvents()
        {
            while (returns.Count > 0)
            {
                String retVal = port.ReadTo("x");

                Data d = returns.Dequeue();
                switch (d)
                {
                    case Data.Power:
                        UseIntFromString(retVal, delegate(int data) {
                            if(PowerReturn!=null)
                                PowerReturn(data);
                        });
                    break;
                    case Data.AspectRatio:
                        UseIntFromString(retVal, delegate(int data) {
                            if(AspectRatioReturn!=null)
                                AspectRatioReturn(data);
                        });
                    break;
                    case Data.ScreenMute:
                        UseIntFromString(retVal, delegate(int data) {
                            if(ScreenMuteReturn!=null)
                                ScreenMuteReturn(data);
                        });
                    break;
                    case Data.VolumeMute:
                        UseIntFromString(retVal, delegate(int data) {
                            if(VolumeMuteReturn!=null)
                                VolumeMuteReturn(data);
                        });
                    break;
                    case Data.VolumeControl:
                        UseIntFromString(retVal, delegate(int data) {
                            if(VolumeControlReturn!=null)
                                VolumeControlReturn(data);
                        });
                    break;
                    case Data.Contrast:
                        UseIntFromString(retVal, delegate(int data) {
                            if(ContrastReturn!=null)
                                ContrastReturn(data);
                        });
                    break;
                    case Data.Brightness:
                        UseIntFromString(retVal, delegate(int data) {
                            if(BrightnessReturn!=null)
                                BrightnessReturn(data);
                        });
                    break;
                    case Data.Colour:
                        UseIntFromString(retVal, delegate(int data) {
                            if(ColourReturn!=null)
                                ColourReturn(data);
                        });
                    break;
                    case Data.Tint:
                        UseIntFromString(retVal, delegate(int data) {
                            if(TintReturn!=null)
                                TintReturn(data);
                        });
                    break;
                    case Data.Sharpness:
                        UseIntFromString(retVal, delegate(int data) {
                            if(SharpnessReturn!=null)
                                SharpnessReturn(data);
                        });
                    break;
                    case Data.OSD:
                        UseIntFromString(retVal, delegate(int data) {
                            if(OSDReturn!=null)
                                OSDReturn(data);
                        });
                    break;
                    case Data.RCLock:
                        UseIntFromString(retVal, delegate(int data) {
                            if(RCLockReturn!=null)
                                RCLockReturn(data);
                        });
                    break;
                    case Data.Treble:
                        UseIntFromString(retVal, delegate(int data) {
                            if(TrebleReturn!=null)
                                TrebleReturn(data);
                        });
                    break;
                    case Data.Bass:
                        UseIntFromString(retVal, delegate(int data) {
                            if(BassReturn!=null)
                                BassReturn(data);
                        });
                    break;
                    case Data.Balance:
                        UseIntFromString(retVal, delegate(int data) {
                            if(BalanceReturn!=null)
                                BalanceReturn(data);
                        });
                    break;
                    case Data.ColourTemperature:
                        UseIntFromString(retVal, delegate(int data) {
                            if(ColourTemperatureReturn!=null)
                                ColourTemperatureReturn(data);
                        });
                    break;
                    case Data.EnergySaving:
                        UseIntFromString(retVal, delegate(int data) {
                            if(EnergySavingReturn!=null)
                                EnergySavingReturn(data);
                        });
                    break;
                    case Data.Tune:
                        if (TuneReturn!=null)
                        {
                            String data;
                            bool success = ParseReturn(retVal,out data);
                            if (success)
                            {
                                var deformat = Tune.Deformat(data);
                                TuneReturn(deformat.Item1, deformat.Item2);
                            }
                            else
                                TuneReturn(-1,-1);
                        }
                        break;
                    case Data.ProgrammeAddSkip:
                        UseIntFromString(retVal, delegate(int data) {
                            if(prgASReturn!=null)
                                prgASReturn(data);
                        });
                    break;
                    case Data.BackLight:
                        UseIntFromString(retVal, delegate(int data) {
                            if(BackLightReturn!=null)
                                BackLightReturn(data);
                        });
                    break;
                    case Data.Input:
                        UseIntFromString(retVal, delegate(int data) {
                            if(InputReturn!=null)
                                InputReturn(data);
                        });
                    break;
                    case Data.String:
                        if (StringReturn != null)
                            StringReturn(retVal);
                    break;
                    case Data.Hide:
                        if (HiddenReturn != null)
                            HiddenReturn(retVal);
                    break;
                }
            }
        }

    }
}