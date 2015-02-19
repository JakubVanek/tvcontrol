//
//  MainWindow.cs
//
//  Author:
//       Jakub Vanek <vanek.jakub4@seznam.cz>
//
//  Copyright (c) 2014 GPL v. 3.0
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
using Gtk;
using Libtvcontrol.LG;
using TVControl;
using InputCmd = Libtvcontrol.LG.Input;

public partial class MainWindow: Gtk.Window
{
    readonly GLib.TimeoutHandler handler;
    uint timerid = 0;
    SerialConn tv;
    bool connected;
	About d = null;
    public MainWindow(): base(Gtk.WindowType.Toplevel)
    {
        Build();
		handler = new GLib.TimeoutHandler(Timer_Tick);
    }

	private void inith(object sender, EventArgs e)
	{
		tv = new SerialConn(1, dev.Text);
		tv.DataReceived += HandleDataReceived;
		tv.Open();
		ready.SetFromIconName("gtk-connect", IconSize.Button);
		connected = true;
		Timer_Tick();
		GLib.Timeout.Add(500, ()=> {
			int volume = 6;
			int.TryParse(volumeview.Text, out volume);
			volumeselect.Value = volume;
			volumescroll.Value = volume;
			int channel = 1;
			int.TryParse(programview.Text, out channel);
			programselect.Value = Convert.ToInt32(channel);
			return false;
		});
		timerid = GLib.Timeout.Add(3000, handler);
	}

	private bool Timer_Tick()
    {
		tv.Send(new Power(0xFF));
		tv.Send(new AspectRatio(0xFF));
		tv.Send(new VolumeControl(0xFF));
		tv.Send(new Tune("FF"));
		tv.Send(new InputCmd(0xFF));
        return true;
    }

	private void quith(object sender, EventArgs e)
	{
		if (connected) {
			GLib.Source.Remove(timerid);
			tv.Close();
			connected = false;
		}
		Application.Quit();
	}

	private void abouth(object sender, EventArgs e)
	{
		if (d == null)
			d = new About();
		d.Show();
	}
		
	private void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		if (connected) {
			GLib.Source.Remove(timerid);
			tv.Close();
			connected = false;
		}
		Application.Quit();
		a.RetVal = true;
	}

	private void handleKey(object sender, EventArgs e)
	{
		if (!connected)
			return;
		if (sender == ratio) {
			sendKey(RCControl.Ratio);
			return;
		}
		if (sender == markfav) {
			sendKey(RCControl.MarkFav);
			return;
		}
		if (sender == epg) {
			sendKey(RCControl.EPG);
			return;
		}
		if (sender == info) {
			sendKey(RCControl.Info);
			return;
		}
		if (sender == qmenu) {
			sendKey(RCControl.QMenu);
			return;
		}
		if (sender == simplink) {
			sendKey(RCControl.SimpLink);
			return;
		}
		if (sender == subtitle) {
			sendKey(RCControl.Subtitle);
			return;
		}
		if (sender == topt) {
			sendKey(RCControl.TeleOpt);
			return;
		}
		if (sender == text) {
			sendKey(RCControl.TeleText);
			return;
		}
		if (sender == blue) {
			sendKey(RCControl.Blue);
			return;
		}
		if (sender == yellow) {
			sendKey(RCControl.Yellow);
			return;
		}
		if (sender == green) {
			sendKey(RCControl.Green);
			return;
		}
		if (sender == red) {
			sendKey(RCControl.Red);
			return;
		}
		if (sender == menu) {
			sendKey(RCControl.Menu);
			return;
		}
		if (sender == left) {
			sendKey(RCControl.Left);
			return;
		}
		if (sender == right) {
			sendKey(RCControl.Right);
			return;
		}
		if (sender == ok) {
			sendKey(RCControl.OK);
			return;
		}
		if (sender == up) {
			sendKey(RCControl.Up);
			return;
		}
		if (sender == down) {
			sendKey(RCControl.Down);
			return;
		}
		if (sender == mute) {
			sendKey(RCControl.Mute);
			return;
		}
		if (sender == pdown) {
			sendKey(RCControl.ProgramDown);
			return;
		}
		if (sender == pup) {
			sendKey(RCControl.ProgramUp);
			return;
		}
		if (sender == vdown) {
			sendKey(RCControl.VolumeDown);
			return;
		}
		if (sender == vup) {
			sendKey(RCControl.VolumeUp);
			return;
		}
		if (sender == qview) {
			sendKey(RCControl.QView);
			return;
		}
		if (sender == list) {
			sendKey(RCControl.List);
			return;
		}
		if (sender == n) {
			sendKey(RCControl.Zero);
			return;
		}
		if (sender == n1) {
			sendKey(RCControl.One);
			return;
		}
		if (sender == n2) {
			sendKey(RCControl.Two);
			return;
		}
		if (sender == n3) {
			sendKey(RCControl.Three);
			return;
		}
		if (sender == n4) {
			sendKey(RCControl.Four);
			return;
		}
		if (sender == n5) {
			sendKey(RCControl.Five);
			return;
		}
		if (sender == n6) {
			sendKey(RCControl.Six);
			return;
		}
		if (sender == n7) {
			sendKey(RCControl.Seven);
			return;
		}
		if (sender == n8) {
			sendKey(RCControl.Eight);
			return;
		}
		if (sender == n9) {
			sendKey(RCControl.Nine);
			return;
		}
		if (sender == energysaving) {
			sendKey(RCControl.EnergySaving);
			return;
		}
		if (sender == input) {
			sendKey(RCControl.Input);
			return;
		}
		if (sender == power) {
			sendKey(RCControl.Power);
			return;
		}
		if (sender == avmode) {
			sendKey(RCControl.AVMode);
			return;
		}
		if (sender == tvrad) {
			sendKey(RCControl.TvRad);
			return;
		}
		if (sender == exit) {
			sendKey(RCControl.Exit);
			return;
		}

		if (sender == play) {
			sendKey(RCControl.Play);
			return;
		}
		if (sender == pause) {
			sendKey(RCControl.Pause);
			return;
		}
		if (sender == stop) {
			sendKey(RCControl.Stop);
			return;
		}
		if (sender == fwd) {
			sendKey(RCControl.Forward);
			return;
		}
		if (sender == bwd) {
			sendKey(RCControl.Backward);
			return;
		}
	}
	protected void sendKey(int key)
	{
		tv.Send(new RCControl(key));
	}

	private void HandleDataReceived (object sender, ControlEventArgs e)
    {
		if (e.Matches.Count == 0)
			return;
		var result = e.Result;
		var match = e.Matches[0];
		if (match == typeof(Power)) {
			int pwr = Hex.From(result.Data);
			bool on = pwr == Power.TVon;
			if (on) {
				tvimage.SetFromIconName("gtk-yes", IconSize.LargeToolbar);
				tvstate.Text = "Zapnuto";
			} else {
				tvimage.SetFromIconName("gtk-no", IconSize.LargeToolbar);
				tvstate.Text = "Vypnuto";
			}
			return;
		}

		if (match == typeof(AspectRatio)) {
			int _aspect = Hex.From(result.Data);
			switch (_aspect) {
				case AspectRatio.A4_3:
					aspect.Text = "4:3";
					break;
				case AspectRatio.Aczoom1:
					aspect.Text = "Cinema Zoom 1";
					break;
				case AspectRatio.Awide:
					aspect.Text = "Plná šířka";
					break;
				case AspectRatio.Ascan:
					aspect.Text = "Jen skenovat";
					break;
				case AspectRatio.Aorig:
					aspect.Text = "Originál";
					break;
				case AspectRatio.A16_9:
					aspect.Text = "16:9";
					break;
				case AspectRatio.Azoom:
					aspect.Text = "Přiblížený";
					break;
				case AspectRatio.A14_9:
					aspect.Text = "14:9";
					break;
				default:
					aspect.Markup = "<i>Neznámý</i>";
					break;
			}
			return;
		}
		if (match == typeof(VolumeControl)) {
			volumeview.Text = Convert.ToString(Hex.From(result.Data));
			return;
		}
		if (match == typeof(Tune)) {
			programview.Text = Convert.ToString(Tune.Deformat(result.Data).Item1);
			return;
		}
		if (match == typeof(InputCmd)) {
			switch (Hex.From(result.Data)) {
				case InputCmd.DTV:
					inputview.Text = "DTV";
					break;
				case InputCmd.AV:
					inputview.Text = "AV1";
					break;
				case InputCmd.AV + InputCmd.Input2:
					inputview.Text = "AV2";
					break;
				case InputCmd.Component:
					inputview.Text = "Komponentní";
					break;
				case InputCmd.HDMI:
					inputview.Text = "HDMI";
					break;                
				case InputCmd.RGB:
					inputview.Text = "RGB";
					break;
			}
			if (Hex.From(result.Data) == InputCmd.RGB)
				autoconf.Sensitive = true;
			else
				autoconf.Sensitive = false;
			return;
		}
    }


	private void sendDirect(object sender, EventArgs e)
	{
		if (!connected)
			return;
		if(sender == aspectsend) {
			int a = AspectRatio.A16_9;
			switch (aspectselect.ActiveText)
			{
				case "16:9":
					a = AspectRatio.A16_9;
					break;
				case "14:9":
					a = AspectRatio.A14_9;
					break;
				case "4:3":
					a = AspectRatio.A4_3;
					break;
				case "Cinema Zoom 1":
					a = AspectRatio.Aczoom1;
					break;
				case "Originál":
					a = AspectRatio.Aorig;
					break;
				case "Přiblížený":
					a = AspectRatio.Azoom;
					break;
				case "Jen skenovat":
					a = AspectRatio.Ascan;
					break;
				case "Plná šířka":
					a = AspectRatio.Awide;
					break;
			}
			tv.Send(new AspectRatio(a));
			return;
		}
		if (sender == volumesend) {
			int volume = Convert.ToInt32(volumeselect.Value);
			tv.Send(new VolumeControl(volume));
			return;
		}
		if (sender == volumeselect) {
			volumescroll.Value = volumeselect.Value;
			return;
		}
		if (sender == volumescroll) {
			volumeselect.Value = volumescroll.Value;
			return;
		}
		if (sender == programbut) {
			int channel = Convert.ToInt32(programselect.Value);
			tv.Send(new Tune(channel,Tune.DTV));
			return;
		}
		if (sender == autoconf) {
			tv.Send(new AutoConfig());
			return;
		}
		if (sender == inputsend) {
			int i = InputCmd.DTV;
			switch (inputselect.ActiveText)
			{
				case "DTV":
					i = InputCmd.DTV;
					break;
				case "AV1":
					i = InputCmd.AV;
					break;
				case "AV2":
					i = InputCmd.AV + InputCmd.Input2;
					break;
				case "Komponentní":
					i = InputCmd.Component;
					break;
				case "HDMI":
					i = InputCmd.HDMI;
					break;
				case "RGB":
					i = InputCmd.RGB;
					break;
			}
			tv.Send(new InputCmd(i));
			return;
		}
	}
}