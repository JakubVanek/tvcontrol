//
//  RCControl.cs
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
	public class RCControl : IntegerCommand
    {
        public const int Power = 0x8;
        public const int TvRad = 0xF0;
        public const int Input = 0xB;
        public const int AVMode = 0x30;
        public const int EnergySaving = 0x95;
        public const int Ratio = 0x79;
        public const int One = 0x11;
        public const int Two = 0x12;
        public const int Three = 0x13;
        public const int Four = 0x14;
        public const int Five = 0x15;
        public const int Six = 0x16;
        public const int Seven = 0x17;
        public const int Eight = 0x18;
        public const int Nine = 0x19;
        public const int Zero = 0x10;
        public const int List = 0x53;
        public const int QView = 0x1A;
        public const int VolumeUp = 0x2;
        public const int VolumeDown = 0x3;
        public const int Mute = 0x9;
        public const int ProgramUp = 0x0;
        public const int ProgramDown = 0x1;
        public const int OK = 0x44;
        public const int Up = 0x40;
        public const int Down = 0x41;
        public const int Right = 0x6;
        public const int Left = 0x7;
        public const int Menu = 0x43;
        public const int Return = 0x28;
        public const int Exit = 0x5B;
        public const int Red = 0x72;
        public const int Green = 0x71;
        public const int Yellow = 0x63;
        public const int Blue = 0x61;
        public const int TeleText = 0x20;
        public const int TeleOpt = 0x21;
        public const int Subtitle = 0x39;
        public const int SimpLink = 0x7E;
        public const int QMenu = 0x45;
        public const int Info = 0xAA;
        public const int EPG = 0xAB;
        public const int MarkFav = 0x1E;

        public const int Stop = 0xB1;
        public const int Play = 0xB0;
        public const int Pause = 0xBA;
        public const int Forward = 0x8E;
        public const int Backward = 0x8F;

		public override string Name   { get; }	= "mc";
		public override int UpLimit   { get; }	= 0xF0;
		public override int DownLimit { get; }	= 0x0;

		public RCControl(){}
		public RCControl(int value) : base(value) {}
    }
}

