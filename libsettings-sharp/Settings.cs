using System;
using System.Collections.Generic;
using System.IO;
namespace SettingsLib
{
    public class SettingsList
    {
        public static readonly string[] DLineSep = {"\n"}; // line separator
        public static readonly string[] DValSep = {","}; // separator of values
        public static readonly string[] DKeyValSplit = {"=",":"}; // separator of field name and values

        public string[] this [string key] // indexer
        {
            get
            {
                return _collection[key];
            }
            set
            {
                _collection[key] = value;
            }
        }

        private readonly Dictionary<string,string[]> _collection; // internal storage
        private string _path; // file path

        public bool Exists(string key)
        {
            return _collection.ContainsKey(key);
        }

        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                if (File.Exists(value))
                    _path = value;
                else
                    throw new FileNotFoundException();
            }
        }

        public SettingsList()
        {
            _collection = new Dictionary<string, string[]>(); 
        }

        public SettingsList(Dictionary<string,string[]> collection)
        {
            _collection = collection;
        }

        public SettingsList(string path)
            : this(path, DLineSep, DValSep, DKeyValSplit)
        {
        }

        public SettingsList(string path, string[] lineSep, string[] valSep, string[] keyValSplit)
        {
            if (!File.Exists(path))
                //if(Directory.GetParent(path).Exists)
            {
                var a = File.Create(path);
                a.Close();
                a.Dispose();
            }

            _collection = Settings.Parse(path, lineSep, valSep, keyValSplit);
            _path = path;
        }






        public void Add(string key, string[] value)
        {
            _collection.Add(key,value);
        }

        public bool ContainsKey(string key)
        {
            return _collection.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return _collection.Remove(key);
        }

        public bool TryGetValue(string key, out string[] value)
        {
            return _collection.TryGetValue(key,out value);
        }

        public ICollection<string> Keys
        {
            get
            {
                return _collection.Keys;
            }
        }

        public ICollection<string[]> Values
        {
            get
            {
                return _collection.Values;
            }
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public bool Contains(string key)
        {
            return _collection.ContainsKey(key);
        }

        public int Count
        {
            get
            {
                return _collection.Count;
            }
        }
    }

    /// <summary>
    /// Settings class with loader and saver.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Parse sheet to string, that can be saved to file.
        /// </summary>
        /// <param name="sheet">Sheet</param>
        /// <param name="linesep">Line separator</param>
        /// <param name="valsep">Value separator</param>
        /// <param name="keyvalsep">Key and values separator</param>
        /// <returns>Output string</returns>
        public static string Parse(Dictionary<string, string[]> sheet, string[] linesep, string[] valsep, string[] keyvalsep)
        {
            string file = "";
            if (sheet.Count == 0)
                return "";
            var keyList = new List<string>(sheet.Keys);
            for (int i = 0; i < sheet.Count; i++)
            {
                file += keyList[i];
                file += keyvalsep[0];
                string[] source = sheet[keyList[i]];
                string attributes = "";
                for (int y = 0; y < source.Length; y++)
                {
                    attributes += source[y];
                    if (y != source.Length - 1)
                        attributes += valsep[0];
                }
                file += attributes;
                if (i != sheet.Count - 1)
                    file += linesep[0];
            }
            return file;
        }

        /// <summary>
        /// Parse string to sheet, which can be easily read.
        /// </summary>
        /// <param name="data">String</param>
        /// <param name="linesep">Line separator</param>
        /// <param name="valsep">Value separator</param>
        /// <param name="keyvalsep">Key and values separator</param>
        /// <returns>Output sheet</returns>
        public static Dictionary<string, string[]> Parse(string data, string[] linesep, string[] valsep, string[] keyvalsep)
        {
            var result = new Dictionary<string, string[]>();
            if (data.Length == 0)
                throw new Exception("Empty data!");
            string[] lines = data.Split(linesep, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 1)
                throw new Exception("Empty data!");
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i]=lines[i].TrimStart(new []{' ','\t'});
                if (lines[i].StartsWith("//",StringComparison.InvariantCultureIgnoreCase) || lines[i].StartsWith("#",StringComparison.InvariantCultureIgnoreCase))
                    continue;

                string[] zones = lines[i].Split(keyvalsep, StringSplitOptions.None);
                if (zones.Length < 2)
                    throw new Exception("Bad format!");
                string valuezone = String.Empty;
                if (zones.Length > 2)
                {
                    for (int z = 1; z < zones.Length; z++)
                    {
                        valuezone += (String.Concat(keyvalsep, zones[z]));
                    }
                }
                else
                    valuezone = zones[1];
                zones = new [] { zones[0], valuezone };
                string name = zones[0];
                string[] values = zones[1].Split(valsep, StringSplitOptions.None);
                if (values.Length < 1)
                    throw new Exception("Bad format!");
                result.Add(name, values);
            }
            return result;
        }
    }
}

