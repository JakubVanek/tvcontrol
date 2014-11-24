//
//  LG_TV.cs
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

namespace TVControl
{
    /// <summary>
    /// LG TV controlling interface.
    /// </summary>
    public class LGTV
    {
        /// <summary>
        /// Timeout for serial port
        /// </summary>
        public const int TIMEOUT = 500;


        /// <summary>
        /// Gets a value indicating whether is serial connection open.
        /// </summary>
        /// <value><c>true</c> if open; otherwise, <c>false</c>.</value>
        public bool Open { get; private set; } = false;


        /// <summary>
        /// Television ID
        /// </summary>
        /// <value>ID for controlling</value>
        public int ID { get; private set; }




        // Port
        SerialPort port;

        // Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TVControl.LGTV"/> class.
        /// </summary>
        /// <param name="device">Device, /dev/ttyS[0-...] or COM[1-...] -> /dev/ttyS3 or COM1</param>
        /// <param name="id">TV ID</param>
        public LGTV(string device, int id)
        {
            this.Open = false;
            port = new SerialPort(device, 9600, Parity.None, 8, StopBits.One)
            {
                NewLine = "\r",
                ReadTimeout = TIMEOUT,
                WriteTimeout = TIMEOUT
            };
            this.ID = id;
        }



        // Init/Close
        /// <summary>
        /// Init this instance.
        /// </summary>
        public void Init()
        {
            port.Open();
            this.Open = true;
        }

        /// <summary>
        /// Close this instance.
        /// </summary>
        public void Close()
        {
            port.Close();
            this.Open = false;
        }



        // Support things
        /// <summary>
        /// Converts TVKey to string.
        /// </summary>
        /// <returns>The string to send.</returns>
        /// <param name="key">TV Key.</param>
        public static string KeyConvert(TVKey key)
        {
            return Hex.ToHex((byte)key);
        }

        /// <summary>
        /// Keys on TV R/C, specifies key code
        /// </summary>
        public enum TVKey : byte
        {
            /// <summary>
            /// Power key.
            /// </summary>
            Power = 8,
            /// <summary>
            /// TV/Rad key.
            /// </summary>
            TvRad = 240,
            /// <summary>
            /// Inupt key.
            /// </summary>
            Input = 11,
            /// <summary>
            /// AV Mode key.
            /// </summary>
            AVMode = 48,
            /// <summary>
            /// Energy Saving key.
            /// </summary>
            EnergySaving = 149,
            /// <summary>
            /// Ratio key.
            /// </summary>
            Ratio = 121,
            /// <summary>
            /// One key.
            /// </summary>
            One = 17,
            /// <summary>
            /// Two key.
            /// </summary>
            Two = 18,
            /// <summary>
            /// Three key.
            /// </summary>
            Three = 19,
            /// <summary>
            /// Four key.
            /// </summary>
            Four = 20,
            /// <summary>
            /// Five key.
            /// </summary>
            Five = 21,
            /// <summary>
            /// Six key.
            /// </summary>
            Six = 22,
            /// <summary>
            /// Seven key.
            /// </summary>
            Seven = 23,
            /// <summary>
            /// Eight key,
            /// </summary>
            Eight = 24,
            /// <summary>
            /// Nine key.
            /// </summary>
            Nine = 25,
            /// <summary>
            /// Zero key.
            /// </summary>
            Zero = 16,
            /// <summary>
            /// List key.
            /// </summary>
            List = 83,
            /// <summary>
            /// Quick View key.
            /// </summary>
            QView = 26,
            /// <summary>
            /// Volume Up key.
            /// </summary>
            VolumeUp = 2,
            /// <summary>
            /// Volume Down key.
            /// </summary>
            VolumeDown = 3,
            /// <summary>
            /// Mute key.
            /// </summary>
            Mute = 9,
            /// <summary>
            /// Program Up key.
            /// </summary>
            ProgramUp = 0,
            /// <summary>
            /// Program Down key.
            /// </summary>
            ProgramDown = 1,
            /// <summary>
            /// OK key.
            /// </summary>
            OK = 68,
            /// <summary>
            /// Up arrow key.
            /// </summary>
            Up = 64,
            /// <summary>
            /// Down arrow key.
            /// </summary>
            Down = 65,
            /// <summary>
            /// Right arrow key.
            /// </summary>
            Right = 6,
            /// <summary>
            /// Left arrow key.
            /// </summary>
            Left = 7,
            /// <summary>
            /// Menu key.
            /// </summary>
            Menu = 67,
            /// <summary>
            /// Exit/Return key.
            /// </summary>
            Return = 40,
            /// <summary>
            /// Red key.
            /// </summary>
            Red = 114,
            /// <summary>
            /// Green key.
            /// </summary>
            Green = 113,
            /// <summary>
            /// Yellow key.
            /// </summary>
            Yellow = 99,
            /// <summary>
            /// Blue key.
            /// </summary>
            Blue = 97,
            /// <summary>
            /// Teletext key.
            /// </summary>
            TeleText = 32,
            /// <summary>
            /// Teletext Options key.
            /// </summary>
            TeleOpt = 33,
            /// <summary>
            /// Subtitles key.
            /// </summary>
            Subtitle = 57,
            /// <summary>
            /// SimpLink key.
            /// </summary>
            SimpLink = 126,
            /// <summary>
            /// Quick Menu key.
            /// </summary>
            QMenu = 69,
            /// <summary>
            /// Info key.
            /// </summary>
            Info = 170,
            /// <summary>
            /// Guide key.
            /// </summary>
            EPG = 171,
            /// <summary>
            /// Mark Fav key
            /// </summary>
            MarkFav = 30
        }

        /// <summary>
        /// Write a command with data for specific TV to serial port
        /// </summary>
        /// <param name="command">Command</param>
        /// <param name="data">Data.</param>
        void Write(string command, string data)
        {
            port.WriteLine(String.Format("{0} {1} {2}", command, ID, data));
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
            answer = null;
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
                        Console.Error.WriteLine("warn: timeout reached, remaining {0}", remaining);
                        remaining--;
                        continue;
                    }
                    else
                    {
                        Console.Error.WriteLine("warn: timeout reached, no answer");
                        break;
                    }
                }
            }
            return success; 
        }

        /// <summary>
        /// Sends the key.
        /// </summary>
        /// <returns><c>true</c>, if key was sent, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        public bool SendKey(TVKey key)
        {
            string nic;
            return Send("mc", KeyConvert(key), false, out nic);
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

        /// <summary>
        /// Power enumeration.
        /// </summary>
        public enum Power : byte
        {
            /// <summary>
            /// TV off.
            /// </summary>
            PowerOff = 0,
            /// <summary>
            /// TV on.
            /// </summary>
            PowerOn = 1,
            /// <summary>
            /// Query power status.
            /// </summary>
            GetStatus = 255
        }

        /// <summary>
        /// Sends power command to TV.
        /// </summary>
        /// <returns>Status or data</returns>
        /// <param name="pwr">Mode or query</param>
        /// <param name = "actual">Actual mode</param>
        public bool SendPower(Power pwr, out Power actual)
        {
            int pow;
            bool a = Send("ka", pwr == Power.GetStatus, (int)pwr, out pow);
            actual = (Power)pow;
            return a;
        }

        /// <summary>
        /// Aspect ratio enumeration.
        /// </summary>
        public enum AspectRatio : byte
        {
            /// <summary>
            /// 4:3 ratio.
            /// </summary>
            Normal = 1,
            /// <summary>
            /// 16:9 ratio.
            /// </summary>
            Widescreen = 2,
            /// <summary>
            /// Zoom ratio (crops 4:3 input to 16:9).
            /// </summary>
            Zoom = 4,
            /// <summary>
            /// Original source ratio.
            /// </summary>
            Original = 6,
            /// <summary>
            /// 14:9 (crops 4:3 input ot 14:9).
            /// </summary>
            ZoomedNormal = 7,
            /// <summary>
            /// Just Scan (only RGB).
            /// </summary>
            JustScan = 9,
            /// <summary>
            /// Full width of screen.
            /// </summary>
            FullWide = 11,
            /// <summary>
            /// Cinema Zoom 1.
            /// </summary>
            CinemaZoom1 = 16,
            /// <summary>
            /// Query aspect ratio.
            /// </summary>
            GetStatus = 255
        }

        /// <summary>
        /// Sends aspect command to TV.
        /// </summary>
        /// <returns>Status or data</returns>
        /// <param name="aspect">Mode or query</param>
        /// <param name = "actual">Actual aspect ration</param>
        public bool SendAspect(AspectRatio aspect, out AspectRatio actual)
        {
            int asp;
            bool a = Send("kc", aspect == AspectRatio.GetStatus, (int)aspect, out asp);
            actual = (AspectRatio)asp;
            return a;
        }

        /// <summary>
        /// Screen mute enumeration.
        /// </summary>
        public enum ScreenMute : byte
        {
            /// <summary>
            /// Screen and video running.
            /// </summary>
            Off = 0,
            /// <summary>
            /// Screen (whole display) off.
            /// </summary>
            ScreenOff = 1,
            /// <summary>
            /// Video (only picture) off.
            /// </summary>
            VideoOff = 16,
            /// <summary>
            /// Query screen mute.
            /// </summary>
            GetStatus = 255
        }

        /// <summary>
        /// Sends screen mute command to TV.
        /// </summary>
        /// <returns>Status or data</returns>
        /// <param name="mute">Mode or query</param>
        /// <param name = "actual">Actual screen mute</param>
        public bool SendScrMute(ScreenMute mute, out ScreenMute actual)
        {
            int scr;
            bool a = Send("kd", mute == ScreenMute.GetStatus, (int)mute, out scr);
            actual = (ScreenMute)scr;
            return a;
        }

        /// <summary>
        /// Volume mute enumeration.
        /// </summary>
        public enum Volume : byte
        {
            /// <summary>
            /// Audio running.
            /// </summary>
            VolumeOn = 1,
            /// <summary>
            /// Audio off.
            /// </summary>
            VolumeOff = 0,
            /// <summary>
            /// Query volume mute.
            /// </summary>
            GetStatus = 255
        }

        /// <summary>
        /// Sends volume mute command to TV.
        /// </summary>
        /// <returns>Status or data</returns>
        /// <param name="volume">Mode or query</param>
        /// <param name = "actual">Actual volume mute</param>
        public bool SendVolumeMute(Volume volume, out Volume actual)
        {
            int vol; 
            bool a = Send("ke", volume == Volume.GetStatus, (int)volume, out vol);
            actual = (Volume)vol;
            return a;
        }

        /// <summary>
        /// Sends the volume change command to TV.
        /// </summary>
        /// <returns>Status or data</returns>
        /// <param name="level">Level or query (query=255)</param>
        /// <param name = "actual">Actual volume</param>
        public bool SendVolume(int level, out int actual)
        {
            #if DENY_HIGHVOLUME
            if (level > 20 && level != 255)
                throw new InvalidOperationException("Volumes above 20 aren't allowed!!");
            #endif

            string actual_string;
            string data_string = Hex.ToHex(level);
            bool success = Send("kf", data_string, level == 255, out actual_string);
            if (success)
                actual = Hex.FromHex(actual_string);
            else
                actual = 0;
            return success;
        }

        /// <summary>
        /// Energy saving modes enumeration.
        /// </summary>
        public enum EnergySaving
        {
            /// <summary>
            /// No energy saving.
            /// </summary>
            Off = 0,
            /// <summary>
            /// Minimal energy saving.
            /// </summary>
            Minimum = 1,
            /// <summary>
            /// Medium energy saving.
            /// </summary>
            Medium = 2,
            /// <summary>
            /// Maximum energy saving.
            /// </summary>
            Maximum = 3,
            /// <summary>
            /// Screen Off
            /// </summary>
            ScreenOff = 5,
            /// <summary>
            /// Query energy saving status.
            /// </summary>
            GetStatus = 255
        }

        /// <summary>
        /// Sends the energy saving mode change command to TV.
        /// </summary>
        /// <returns>Status or data</returns>
        /// <param name="mode">Mode or query</param>
        /// <param name = "actual">Actual energy saving</param>
        public bool SendEnergySaving(EnergySaving mode, out EnergySaving actual)
        {
            int es; 
            bool a = Send("jq", mode == EnergySaving.GetStatus, (int)mode, out es);
            actual = (EnergySaving)es;
            return a;
        }

        /// <summary>
        /// Query code for SendChannel function.
        /// </summary>
        public const int CHANNEL_GETSTATE = Int32.MaxValue;

        /// <summary>
        /// Sends the channel mode change command to TV.
        /// </summary>
        /// <returns>Status or data</returns>
        /// <param name="channel">Channel or query</param>
        /// <param name = "actual">Actual channel</param>
        public bool SendChannel(int channel, out int actual)
        {
            string actual_string;
            bool retval;
            bool getstate = channel == CHANNEL_GETSTATE;
            retval = Send("ma", 
                getstate ? Hex.ToHex(255) : String.Format("0 {0:x} 10", channel), 
                getstate, out actual_string);
            try
            {
                actual_string = actual_string.Substring(2, actual_string.Length - 4);
                actual = Hex.FromHex(actual_string);
            }
            catch
            {
                actual = -1;
            }
            return retval;
        }

        /// <summary>
        /// Input enumeration.
        /// </summary>
        public enum Input : byte
        {
            /// <summary>
            /// Digital TV.
            /// </summary>
            DTV = 0,
            /// <summary>
            /// AV no. 1.
            /// </summary>
            AV1 = 32,
            /// <summary>
            /// AV no. 2.
            /// </summary>
            AV2 = 33,
            /// <summary>
            /// HDMI.
            /// </summary>
            HDMI = 144,
            /// <summary>
            /// RGB (VGA).
            /// </summary>
            RGB = 96,
            /// <summary>
            /// Component (YPbPr).
            /// </summary>
            Component = 64,
            /// <summary>
            /// The get status.
            /// </summary>
            GetStatus = 255
        }

        /// <summary>
        /// Sends the input command to TV.
        /// </summary>
        /// <returns>Status or data</returns>
        /// <param name="input">Input or query</param>
        /// <param name = "state">Actual input</param>
        public bool SendInput(Input input, out Input state)
        {
            int inp; 
            bool a = Send("xb", input == Input.GetStatus, (int)input, out inp);
            state = (Input)inp;
            return a;
        }

        /// <summary>
        /// Sends the video autoconfig command to TV. NOTE: only for RGB/VGA.
        /// </summary>
        /// <returns><c>true</c>, if success, <c>false</c> otherwise.</returns>
        public bool SendAutoConfig()
        {
            string nic;
            return Send("ju", Hex.ToHex(1), false, out nic);
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
    }
}

