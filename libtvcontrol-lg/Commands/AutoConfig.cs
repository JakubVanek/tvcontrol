//
//  AutoConfig.cs
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
	/// <summary>
	/// Auto configuration command.
	/// </summary>
    public class AutoConfig : Command
    {
		public const int Value = 0x1;
		public override string Name { get; } = "ju";

		public AutoConfig() { Data = Hex.To(Value); }
		public override bool Match(Result r)
		{
			bool match = r.Command2 == Name[1];
			match &= r.Success;
			int val;
			match &= int.TryParse(r.Data, out val);
			match &= val == Value;
			return match;
		}
		public override string ToString()
		{
			return string.Format("[{0},1]", Name);
		}
    }
}

