//
//  Result.cs
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
namespace Libtvcontrol.LG
{
	[Serializable]
	/// <summary>
	/// Command result struct.
	/// </summary>
	public struct Result
	{
		/// <summary>
		/// Command 2.
		/// </summary>
		public readonly char Command2;
		/// <summary>
		/// Return data.
		/// </summary>
		public readonly string Data;
		/// <summary>
		/// Return status.
		/// </summary>
		public readonly string Status;
		/// <summary>
		/// Gets a value indicating whether this <see cref="Libtvcontrol.LG.Result"/> is success.
		/// </summary>
		/// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
		public bool Success {
			get {
				return Status == "OK";
			}
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Libtvcontrol.LG.Result"/> struct.
		/// </summary>
		/// <param name="cmd2">Command 2.</param>
		/// <param name="status">Return status.</param>
		/// <param name="data">Return data.</param>
		public Result(char cmd2, string status, string data)
		{
			Command2 = cmd2;
			Status = status;
			Data = data;
		}
	}
}

