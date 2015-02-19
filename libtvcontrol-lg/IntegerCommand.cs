//
//  IntegerCommand.cs
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
using System;
using System.Globalization;

namespace Libtvcontrol.LG
{
	public abstract class IntegerCommand : Command
    {
		public abstract int  DownLimit { get; }
		public abstract int  UpLimit { get; }
		public int CustomData {
			get { return Hex.From(Data); }
			set { Data = Hex.To(value); }
		}

		protected IntegerCommand() {}
		protected IntegerCommand(int value)
		{
			if (!Validate(value))
				throw new ArgumentOutOfRangeException("value", value, 
					String.Format("Value is out of range {0}-{1}", DownLimit, UpLimit));
			CustomData = value;
         }

		public bool Validate(int value)
		{ // value in range or status request
			return (value >= DownLimit && value <= UpLimit) || value == 0xFF;
   		}
		public override bool Match(Result r)
		{
			bool match = r.Success;
			match &= r.Command2 == Name[1];
			int val = 0;
			try{ val = Hex.From(r.Data); } catch { match = false; }
			match &= Validate(val);
			return match;
		}
    }
}

