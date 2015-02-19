//
//  Hex.cs
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
    /// Hex-string support
    /// </summary>
    public static class Hex
    {
        /// <summary>
        /// Converts number to hex notation.
        /// </summary>
        /// <returns>Hex.</returns>
        /// <param name="number">Number.</param>
        public static string To(int number)
        {
            return number.ToString("X2");
        }

        /// <summary>
        /// Convert hex string to Int32
        /// </summary>
        /// <returns>Int32</returns>
        /// <param name="number">Hex string</param>
        public static int From(string number)
        {
            return Convert.ToInt32(number, 16);
        }
    }
}

