//
//  Logger.cs
//
//  Author:
//       kuba <>
//
//  Copyright (c) 2015 kuba
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
using System.IO;
using System.Text;

namespace LGTVsrv
{
    public class Logger
    {
        public readonly string file;
        public bool Verbose;
        private TextWriter stream;

        public Logger(string logfile, bool verbose, bool console)
        {
            file = logfile;
            Verbose = verbose;
            if (console)
            {
                stream = Console.Error;
            }
            else
            {
                stream = new StreamWriter(logfile,true,Encoding.UTF8);
            }
        }
        public void Write(string data)
        {
            stream.Write(data);
        }
        public void WriteLine(string data)
        {
            stream.WriteLine(data);
        }


        public void Write(string data, params object[] objects)
        {
            stream.Write(data,objects);
        }
        public void WriteLine(string data, params object[] objects)
        {
            stream.WriteLine(data,objects);
        }


        public void Write(Level level, string data, params object[] objects)
        {
            stream.Write(GetMarker(level)+" "+data,objects);
        }
        public void WriteLine(Level level, string data, params object[] objects)
        {
            stream.WriteLine(GetMarker(level)+" "+data,objects);
        }
        private static string GetMarker(Level level)
        {
            switch (level)
            {
                case Level.Debug:
                    return "[DD]";
                case Level.Info:
                    return "[--]";
                case Level.Notice:
                    return "[!!]";
                case Level.Error:
                    return "[EE]";
                default:
                    return "[EE]";
            }
        }
        public enum Level
        {
            Debug,
            Info,
            Notice,
            Error
        }
    }
}

