//
//  Tune.cs
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
namespace Libtvcontrol.LG
{
	public class Tune : Command
    {
		public const int Analogue = 0x0;
		public const int DTV = 0x10;
        public const int Radio = 0x20;
		public const string Status = "FF";

		public override string Name { get; } = "ma";
		public Tune(){}
		public Tune(string tune) : base(tune){}
		public Tune(int channel, int mode) : base(Format(channel,mode)){}

        public static string Format(int program, int mode)
        {
			string noformat = String.Format("{0:X4}",(short)program);
            string prg_str = String.Format("{0} {1}",noformat.Substring(0,2),noformat.Substring(2,2));
            return String.Format("{0} {1:X2}", prg_str, mode);
        }
        public static Tuple<int,int> Deformat(string returned)
        {
            string channelStr = returned.Substring(0,4);
            string modeStr = returned.Substring(4,2);
            int channel = Hex.From(channelStr);
            int mode = Hex.From(modeStr);
            return new Tuple<int,int>(channel,mode);
        }

		public override bool Match(Result r)
		{
			bool match = r.Command2 == Name[1];
			match &= r.Success;
			match &= r.Data.Length == 6;
			int channel;
			int mode;
			try {
				Tuple<int,int> t = Deformat(r.Data);
				channel = t.Item1;
				mode = t.Item2;
			} catch {
				match = false;
			}
			return match;
		}
    }
}

