//
//  ScreenMute.cs
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
	public class ScreenMute : IntegerCommand
    {
        public const int ScreenOn = 0x0;
        public const int ScreenOff = 0x01;
        public const int VideoOff = 0x10;

		public override string Name   { get; }	= "kd";
		public override int UpLimit   { get; }	= 0x10;
		public override int DownLimit { get; }	= 0x0;

		public ScreenMute(){}
		public ScreenMute(int value) : base(value) {}
    }
}

