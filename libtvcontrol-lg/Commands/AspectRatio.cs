//
//  AspectRatio.cs
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
	/// Aspect ratio command.
	/// </summary>
	public class AspectRatio : IntegerCommand
    {
		public const int A4_3    = 0x1; // 4:3
		public const int A16_9   = 0x2; // 16:9
		public const int Azoom   = 0x4; // Zoom
		public const int Aorig   = 0x6; // Original
		public const int A14_9   = 0x7; // 14:9
		public const int Ascan   = 0x9; // Just scan
		public const int Awide   = 0xB; // Full wide
		public const int Aczoom1 = 0x10;// Cinema Zoom 1

		public override string Name   { get; }	= "kc";
		public override int UpLimit   { get; }	= 0x1F;
		public override int DownLimit { get; }	= 0x1;

		public AspectRatio(){}
		public AspectRatio(int value) : base(value) {}
    }
}

