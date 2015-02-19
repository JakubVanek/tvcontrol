//
//  IO.cs
//
//  Author:
//       Jakub Vanek <vanek.jakub4@seznam.cz>
//
//  Copyright (c) 2014 GPL v. 2.0
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
using System;
using System.IO.Ports;

namespace TVControl
{
    public class IO : IDisposable
    {
        SerialPort port;

        public bool Open
        {
            public get;
            private set;
        }

        public int ID
        {
            public get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TVControl.IO"/> class.
        /// </summary>
        /// <param name="device">Device, /dev/ttyS[0-...] or COM[1-...] -> /dev/ttyS3 or COM1</param>
        /// <param name="id">TV ID</param>
        public IO(string device, int id)
        {
            this.Open = false;
            port = new SerialPort(device, 9600, Parity.None, 8, StopBits.One)
            {
                NewLine = "\r",
                ReadTimeout = 1000,
                WriteTimeout = 1000
            };
            this.ID = id;
        }

        public void open()
        {

        }



        private bool disposed = false;

        void Dispose()
        { 
            Dispose(true); 
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {      
                if (disposing)
                {
                    // Manual release of managed resources.
                    port.Dispose();
                }
                // Release unmanaged resources.
                if (Open)
                    port.Close();
                disposed = true;
            }
        }

        ~IO()
        {
            Dispose(false);
        }
    }
}

