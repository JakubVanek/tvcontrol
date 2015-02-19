//
//  SerialConn.cs
//
//  Author:
//       Jakub Vaněk <vanek.jakub4@seznam.cz>
//
//  Copyright (c) 2015 GNU GPL v2.0
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;

namespace Libtvcontrol.LG
{
    public class SerialConn
    {
        public delegate void DataReceivedHandler(object sender, ControlEventArgs e);
		public event DataReceivedHandler DataReceived;
		public int ID {
			get; private set;
		}
		//int nodata = 0;
		readonly SerialPort stream; // PRIVATE
        readonly StringBuilder receiveBuffer = new StringBuilder();
		readonly Timer timer; // PRIVATE
		readonly Queue<Type> history;

        public SerialConn(int id, string device)
        {
            stream = new SerialPort(device, 9600, Parity.None,
                8, StopBits.One) {
                ReadTimeout  = 1000,
                WriteTimeout = 1000,
				RtsEnable = true,
            };
			history = new Queue<Type>();
			timer = new Timer(200);
			timer.Elapsed += stream_receive;
            ID = id;
		}
		public void Open()
		{
			stream.Open();
			timer.Start();
		}
		public void Close()
		{
			timer.Stop();
			stream.Close();
			history.Clear();
		}
		void stream_receive(object sender, ElapsedEventArgs e)
        {
			if (stream.BytesToRead == 0) {
				/*nodata++;
				if (nodata > 1000 / timer.Interval) {
					for (int i = 0; i < history.Count / 4; i++)
						history.Dequeue();
					nodata = 0;
				}*/
				return;
			}
			//nodata = 0;
            receiveBuffer.Append(stream.ReadExisting());
			var regex = new Regex(@"[a-z] " + Hex.To(ID) + @" (NG|OK)([a-f]|[0-9])+x");
            Match match;
            do {
                match = regex.Match(receiveBuffer.ToString());
				if (match.Success) {
                    Process(match.Captures[0].Value);
                    receiveBuffer.Remove(match.Captures[0].Index, match.Captures[0].Length);
                }
            } while (match.Success);
        }

        void Process(string retval)
		{
			Type latest = null;
			Type[] arr = null;
			try{
				latest = history.Dequeue();
				arr = history.ToArray();
			} catch {}

			if (DataReceived == null)
				return;
            //         ||[    0   ]   [ 1]   [       2      ]||
            // Format: ||[Command2][ ][ID][ ][OK/NG][DATA][x]||
            //         ||XXXXXXXXXX[ ]XXXX[ ][OK/NG__DATA]---||
            // Example: 'a 01 OK01x'
            char cmd		= retval.Substring(0, 1)[0];	// a
            string status	= retval.Substring(5, 2);		// OK/NG
            string data		= retval.Substring(7, retval.Length - 8); // number

			var e = new ControlEventArgs(cmd,status,data);
			e.Match();

			try {
				if (e.Matches.Count > 1) {
					var query = from type in e.Matches
					            where type.Name == latest.Name
					            select type;
					Type result = null;
					foreach (var type in query) {
						result = type;
						break;
					}
					if (result == null) {
						query = from type in e.Matches
						        where arr.Contains(type)
						        select type;
						foreach (var type in query) {
							result = type;
							break;
						}
					}
					e.Matches = new []{result}.ToList();
				}
			} catch(Exception ex) {
				Console.Error.WriteLine("ERROR: {0}\nTRACE: {1}",ex.Message,ex.StackTrace);
			}
			DataReceived(this,e);
        }

		public void Send(Command m)
        {
			if (!stream.IsOpen)
				throw new Exception("Serial port is not open!!!");
			history.Enqueue(m.GetType());
			var packet = new StringBuilder(6+m.Data.Length);
			packet.AppendFormat("{0} {1} {2}\r",	m.Name,
													ID,
													m.Data);
			stream.Write(packet.ToString());
        }
    }
}

