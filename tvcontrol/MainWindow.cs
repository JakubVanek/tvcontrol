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
using TVControl;

public partial class MainWindow: Gtk.Window
{
    readonly GLib.TimeoutHandler handler;
    uint timerid = 0;
    LGTV tv;
    bool connected;

    public MainWindow()
        : base(Gtk.WindowType.Toplevel)
    {

        Build();
        handler = new GLib.TimeoutHandler(Timer_Tick);
    }

    bool Timer_Tick()
    {
        bool on = tv.SendPower(LGTV.Power.GetStatus).Item2 == "01";
        if (on)
        {
            tvimage.SetFromIconName("gtk-yes", IconSize.LargeToolbar);
            tvstate.Text = "Zapnuto";
        }
        else
        {
            tvimage.SetFromIconName("gtk-no", IconSize.LargeToolbar);
            tvstate.Text = "Vypnuto";
        }
        var aspectt = tv.SendAspect(LGTV.AspectRatio.GetStatus);
        if (aspectt.Item1)
        {
            switch ((LGTV.AspectRatio)Convert.ToByte(aspectt.Item2, 16))
            {
                case LGTV.AspectRatio.Normal:
                    aspect.Text = "4:3";
                    break;
                case LGTV.AspectRatio.CinemaZoom1:
                    aspect.Text = "Cinema Zoom 1";
                    break;
                case LGTV.AspectRatio.FullWide:
                    aspect.Text = "Plná šířka";
                    break;
                case LGTV.AspectRatio.JustScan:
                    aspect.Text = "Jen skenovat";
                    break;
                case LGTV.AspectRatio.Original:
                    aspect.Text = "Originál";
                    break;
                case LGTV.AspectRatio.Widescreen:
                    aspect.Text = "16:9";
                    break;
                case LGTV.AspectRatio.Zoom:
                    aspect.Text = "Přiblížený";
                    break;
                case LGTV.AspectRatio.ZoomedNormal:
                    aspect.Text = "14:9";
                    break;
            }
        }
        else
        {
            aspect.Markup = "<i>Neznámý</i>";
        }
        var volumett = tv.SendVolume(255);
        if (volumett.Item1)
        {
            int volume = Convert.ToInt32(volumett.Item2, 16);
            volumeview.Text = Convert.ToString(volume);
        }
        else
            volumeview.Markup = "<i>Neznámá</i>";
        var programtt = tv.SendChannel(LGTV.SendChannelGetState);
        if (programtt.Item1)
        {
            string program = programtt.Item2.Substring(2, programtt.Item2.Length - 4);
            string final = Convert.ToString(Convert.ToInt32(program, 16));
            programview.Text = final;
        }
        else
            programview.Markup = "<i>Neznámý</i>";
        var inputt = tv.SendInput(LGTV.Input.GetStatus);
        if (inputt.Item1)
        {
            var i = (LGTV.Input)Convert.ToByte(inputt.Item2, 16);
            switch (i)
            {
                case LGTV.Input.DTV:
                    inputview.Text = "DTV";
                    break;
                case LGTV.Input.AV1:
                    inputview.Text = "AV1";
                    break;
                case LGTV.Input.AV2:
                    inputview.Text = "AV2";
                    break;
                case LGTV.Input.Component:
                    inputview.Text = "Komponentní";
                    break;
                case LGTV.Input.HDMI:
                    inputview.Text = "HDMI";
                    break;                
                case LGTV.Input.RGB:
                    inputview.Text = "RGB";
                    break;
            }
            if (i == LGTV.Input.RGB)
                autoconf.Sensitive = true;
            else
                autoconf.Sensitive = false;
        }
        else
            inputview.Markup = "<i>Neznámý</i>";
        return true;
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        if (connected)
        {
            GLib.Source.Remove(timerid);
            tv.Close();
            connected = false;
        }
        Application.Quit();
        a.RetVal = true;
    }

    protected void tvradh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.TvRad);
    }

    protected void avmodeh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.AVMode);
    }

    protected void powerh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Power);
    }

    protected void inputh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Input);
    }

    protected void energysavingh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.EnergySaving);
    }

    protected void n1h(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.One);
    }

    protected void n2h(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Two);
    }

    protected void n3h(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Three);
    }

    protected void n4h(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Four);
    }

    protected void n5h(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Five);
    }

    protected void n6h(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Six);
    }

    protected void n7h(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Seven);
    }

    protected void n8h(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Eight);
    }

    protected void n9h(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Nine);
    }

    protected void n0h(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Zero);
    }

    protected void listh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.List);
    }

    protected void qviewh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.QView);
    }

    protected void vuph(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.VolumeUp);
    }

    protected void vdownh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.VolumeDown);
    }

    protected void puph(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.ProgramUp);
    }

    protected void pdownh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.ProgramDown);
    }

    protected void muteh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Mute);
    }

    protected void uph(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Up);
    }

    protected void okh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.OK);
    }

    protected void righth(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Right);
    }

    protected void downh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Down);
    }

    protected void lefth(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Left);
    }

    protected void menuh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Menu);
    }

    protected void exit(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Return);
    }

    protected void redh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Red);
    }

    protected void greenh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Green);
    }

    protected void yellowh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Yellow);
    }

    protected void blueh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Blue);
    }

    protected void texth(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.TeleText);
    }

    protected void topth(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.TeleOpt);
    }

    protected void subtitleh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Subtitle);
    }

    protected void simplinkh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.SimpLink);
    }

    protected void qmenuh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.QMenu);
    }

    protected void infoh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Info);
    }

    protected void epgh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.EPG);
    }

    protected void markfavh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.MarkFav);
    }

    protected void ratioh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendKey(LGTV.TVKey.Ratio);
    }

    protected void inith(object sender, EventArgs e)
    {
        tv = new LGTV(dev.Text, 1);
        tv.Init();
        ready.SetFromIconName("gtk-connect", IconSize.Button);
        connected = true;
        Timer_Tick();
        int volume = 6;
        int.TryParse(volumeview.Text, out volume);
        volumeselect.Value = volume;
        volumescroll.Value = volume;
        int channel = 1;
        int.TryParse(programview.Text, out channel);
        programselect.Value = Convert.ToInt32(channel);
        timerid = GLib.Timeout.Add(5000, handler);
    }

    protected void sendaspect(object sender, EventArgs e)
    {
        if (!connected)
            return;
        LGTV.AspectRatio a = LGTV.AspectRatio.FullWide;
        switch (aspectselect.ActiveText)
        {
            case "16:9":
                a = LGTV.AspectRatio.Widescreen;
                break;
            case "14:9":
                a = LGTV.AspectRatio.ZoomedNormal;
                break;
            case "4:3":
                a = LGTV.AspectRatio.Normal;
                break;
            case "Cinema Zoom 1":
                a = LGTV.AspectRatio.CinemaZoom1;
                break;
            case "Originál":
                a = LGTV.AspectRatio.Original;
                break;
            case "Přiblížený":
                a = LGTV.AspectRatio.Zoom;
                break;
            case "Jen skenovat":
                a = LGTV.AspectRatio.JustScan;
                break;
            case "Plná šířka":
                a = LGTV.AspectRatio.FullWide;
                break;
        }
        tv.SendAspect(a);
        Timer_Tick();
    }

    protected void sendvolume(object sender, EventArgs e)
    {
        if (!connected)
            return;
        byte volume = Convert.ToByte(volumeselect.Value);
        if (volume > 20 || volume < 0)
            return;
        tv.SendVolume(volume);
        Timer_Tick();
    }

    protected void volumeselecth(object sender, EventArgs e)
    {
        volumescroll.Value = volumeselect.Value;
    }

    protected void volumescrollh(object sender, EventArgs e)
    {
        volumeselect.Value = volumescroll.Value;
    }

    protected void programsend(object sender, EventArgs e)
    {
        if (!connected)
            return;
        byte channel = Convert.ToByte(programselect.Value);
        tv.SendChannel(channel);
        Timer_Tick();
    }

    protected void autoconfh(object sender, EventArgs e)
    {
        if (connected)
            tv.SendAutoConfig();
    }

    protected void inputsendh(object sender, EventArgs e)
    {
        if (!connected)
            return;
        LGTV.Input i = LGTV.Input.DTV;
        switch (inputselect.ActiveText)
        {
            case "DTV":
                i = LGTV.Input.DTV;
                break;
            case "AV1":
                i = LGTV.Input.AV1;
                break;
            case "AV2":
                i = LGTV.Input.AV2;
                break;
            case "Komponentní":
                i = LGTV.Input.Component;
                break;
            case "HDMI":
                i = LGTV.Input.HDMI;
                break;
            case "RGB":
                i = LGTV.Input.RGB;
                break;
        }
        tv.SendInput(i);
        Timer_Tick();
    }

    protected void quith(object sender, EventArgs e)
    {
        if (connected)
        {
            GLib.Source.Remove(timerid);
            tv.Close();
            connected = false;
        }
        Application.Quit();
    }

    protected void abouth(object sender, EventArgs e)
    {
        About d = new About();
        d.Show();
    }
}