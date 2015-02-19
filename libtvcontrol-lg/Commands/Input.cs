//
//  Input.cs
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
	public class Input : IntegerCommand
    {
		public const int DTV	   = 0x0;
		public const int ATV	   = 0x10;
		public const int AV		   = 0x20;
		public const int Component = 0x40;
		public const int RGB	   = 0x60;
		public const int HDMI	   = 0x90;

		public const int Input1 = 0x0;
		public const int Input2 = 0x1;
		public const int Input3 = 0x2;
		public const int Input4 = 0x3;

		public override string Name { get; }	= "xb";
		public override int UpLimit { get; }	= 0x93;
		public override int DownLimit { get; }	= 0x0;

		public Input(){}
		public Input(int value) : base(value) {}
		public Input Combine(int iface, int number)
		{
			return new Input(iface + number);
		}
    }
}

