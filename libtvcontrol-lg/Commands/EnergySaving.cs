//
//  EnergySaving.cs
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
	public class EnergySaving : IntegerCommand
    {
		public const int Off	   = 0x0;
		public const int Minimum   = 0x1;
		public const int Medium	   = 0x2;
		public const int Maximum   = 0x3;
		public const int Auto	   = 0x4;
		public const int ScreenOff = 0x5;

		public override string Name { get; } = "jq";
		public override int UpLimit { get; } = 5;
		public override int DownLimit { get; } = 0;

		public EnergySaving(){}
		public EnergySaving(int value) : base(value) {}

    }
}

