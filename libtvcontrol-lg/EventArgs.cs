//
//  EmptyClass.cs
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
using System.Collections.Generic;

namespace Libtvcontrol.LG
{
	[Serializable]
	/// <summary>
	/// Event arguments for received data handler.
	/// </summary>
	public sealed class ControlEventArgs : EventArgs
	{
		public static readonly Dictionary<Type,string> Commands = new Dictionary<Type, string>();
		static ControlEventArgs()
		{
			/* FIXME CONFLICTS:
			 * [ju] - [ku]: ColorTemp - AutoConfig
			 * [mb] - [xb]:   Program - Input
			 * [mc] - [kc]:       Key - Aspect Ratio
			 * [mg] - [kg]: Backlight - Contrast
			 */
			Commands.Add(typeof(AspectRatio),"AspectRatio");
			Commands.Add(typeof(AutoConfig),"AutoConfig");
			Commands.Add(typeof(Balance),"Balance");
			Commands.Add(typeof(Bass),"Bass");
			Commands.Add(typeof(Brightness),"Brightness");
			Commands.Add(typeof(ColorTemp),"ColorTemp");
			Commands.Add(typeof(Colour),"Colour");
			Commands.Add(typeof(Contrast),"Contrast");
			Commands.Add(typeof(EnergySaving),"EnergySaving");
			Commands.Add(typeof(Input),"Input");
			Commands.Add(typeof(OSDshow),"OSDshow");
			Commands.Add(typeof(Power),"Power");
			Commands.Add(typeof(ProgrammeUtil),"ProgrammeUtil");
			Commands.Add(typeof(RCControl),"RCControl");
			Commands.Add(typeof(ScreenMute),"ScreenMute");
			Commands.Add(typeof(Sharpness),"Sharpness");
			Commands.Add(typeof(Tint),"Tint");
			Commands.Add(typeof(Treble),"Treble");
			Commands.Add(typeof(Tune),"Tune");
			Commands.Add(typeof(VolumeControl),"VolumeControl");
			Commands.Add(typeof(VolumeMute),"VolumeMute");
		}
		/// <summary>
		/// Contains command return
		/// </summary>
		/// <value>The result.</value>
		public Result Result {
			get;
			private set;
		}
		public List<Type> Matches {
			get;
			internal set;
		} = null;
		/// <summary>
		/// Initializes a new instance of the <see cref="Libtvcontrol.LG.ControlEventArgs"/> class.
		/// </summary>
		/// <param name="r">Result from command.</param>
		public ControlEventArgs(Result r)
		{
			Result = r;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Libtvcontrol.LG.ControlEventArgs"/> class.
		/// </summary>
		/// <param name="cmd">Command 2.</param>
		/// <param name="stat">Return status.</param>
		/// <param name="data">Return data.</param>
		public ControlEventArgs(char cmd, string stat, string data)
		{
			Result = new Result(cmd, stat, data);
		}
		public void Match()
		{
			Matches = new List<Type>();
			foreach (KeyValuePair<Type,string> pair in Commands) {
				var instance = (Command)Activator.CreateInstance(pair.Key,true);
				bool ok = instance.Match(Result);
				if (ok)
					Matches.Add(pair.Key);
			}
		}
	}
}

