//
//  Program.cs
//
//  Author:
//       kuba <>
//
//  Copyright (c) 2014 kuba
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
using System.IO;
using System.Net;
using System.Net.Sockets;
using NDesk.Options;
using System.Reflection;
using System.Diagnostics;


namespace LGTVsrv
{
    public static class Program
    {
        static readonly string defaultConf = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"lgtvsrv.conf");
        static readonly string defaultAuth = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"lgtvsrv-auth.conf");
        static readonly string defaultLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"lgtvsrv.log");
        static void Main(String[] args)
        {
            bool help = false;
            bool version = false;
            string ip = "0.0.0.0";
            ushort port = 15678;
            string conf = defaultConf;
            string log = defaultLog;
            bool noredirect = false;
            bool verbose = false;
            int clients = 4;
            string authfile = defaultAuth;
            bool auth = true;
            bool noconf = false;
            int id = 1;
            string comport = Environment.OSVersion.Platform == PlatformID.Unix  ? "/dev/ttyS0" : "COM1";
            bool usermanager;
            var opts = new OptionSet {
                {"h|help","Show help and exit [-]",v => help = v!=null},
                {"V|version","Show version and exit [-]",v => version = v!=null},
                {"d|noconf","Don't use config file [-]",v => noconf = v!=null},
                {"a|address","Use specified {IP} address for listening, for all IPs use \"0.0.0.0\" ["+ip+"]",v => ip = v},
                {"p|port","Use specified {PORT} [15678]",v => port = Convert.ToUInt16(v)},
                {"c|conf","Use specified configuration {FILE} []",v => conf = v},
                {"n|noredirect","Don't redirect output to log []",v => noredirect=v!=null},
                {"l|log","Use specified log {FILE} [./lgtvsrv.conf]",v => log = v},
                {"v|verbose","Be verbose [-]",v => verbose=v!=null},
                {"m|clients","Maximum number of {CONNECTIONS} [4]",v => clients=Convert.ToInt32(v)},
                {"e|auth","Use authorization [+]",v => auth=v!=null},
                {"f|authfile","Use specified auth {FILE} [./lgtvsrv-auth.conf]",v => authfile = v},
                {"i|id","Use specified LG TV {ID} [1]",v => id = Convert.ToInt32(v)},
                {"s|comport","Use specified COM {PORT} ["+comport+"]",v => comport=v},
                {"u|usermgr","Perform user tasks (no server)[-]",v => usermanager= v!=null}
            };
            try{
                opts.Parse(args);
            }catch(Exception e){
                Console.Error.WriteLine("[EE] Error occured during option parsing: {0}",e.Message);
                return;
            }
            if (version)
            {
                ShowVersion();
                return;
            }
            if (help)
            {
                ShowAbout();
                opts.WriteOptionDescriptions(Console.Out);
                return;
            }



            Logger logger = new Logger(log,verbose,noredirect);









            if (!usermanager)
            {
                if (noconf && auth && !File.Exists(authfile))
                {
                    logger.WriteLine(Logger.Level.Error,"No authfile exists!");
                    return;
                }





                IPAddress addr;
                bool success = IPAddress.TryParse(ip, out addr);
                if (!success)
                {
                    logger.WriteLine(Logger.Level.Error,"Error occured during IP option parsing");
                    return;
                }
            }
            else
            {
                UserManager.Start(authfile);
            }

        }
        static void ShowVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            Console.WriteLine("lgtvcontrol-server {0}", fvi.FileVersion);
        }
        static void ShowAbout()
        {
            ShowVersion();
            ShowContact();
        }
        static void ShowContact()
        {
            Console.WriteLine("Jakub Vaněk <vanek.jakub4@seznam.cz> - GNU GPL v2.0");
        }
        static void ShowData(bool help, bool version, string ip,int port, string conf, string log,
            bool noredirect, bool verbose, int clients, string authfile, bool auth, bool noconf,
            int id, string comport)
        {

            Console.WriteLine("help: {0}",help);
            Console.WriteLine("version: {0}",version);
            Console.WriteLine("ip: {0}",ip);
            Console.WriteLine("port: {0}",port);
            Console.WriteLine("conf: {0}",conf);
            Console.WriteLine("log: {0}",log);
            Console.WriteLine("noredirect: {0}",noredirect);
            Console.WriteLine("verbose: {0}",verbose);
            Console.WriteLine("clients: {0}",clients);
            Console.WriteLine("authfile: {0}",authfile);
            Console.WriteLine("auth: {0}",auth);
            Console.WriteLine("noconf: {0}",noconf);
            Console.WriteLine("id: {0}",id);
            Console.WriteLine("comport: {0}",comport);
        }
    }
}

