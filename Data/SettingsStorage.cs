using System;
using Contracts;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Data
{
    public class SettingsStorage
    {
        public ObserverSettings Settings
        {
            get { return JsonConvert.DeserializeObject<ObserverSettings>(SettingsAsJson); }
            set { SettingsAsJson = JsonConvert.SerializeObject(value, Formatting.Indented); }
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public string SettingsAsJson
        {
            get { return FromFile(); }
            set { ToFile(value); }
        }

        public virtual string SettingsFile => "JenkinsObserverSettings.json";

        /* http://stackoverflow.com/a/19840148/2899390
         * UnauthorizedAccessException 
         * IOException
         * FileNotFoundException
         * DirectoryNotFoundException
         * PathTooLongException
         * NotSupportedException
         * SecurityException
         * ArgumentException
         * 
         * Most Important to Catch:
         * IOException                 //Several above derive from here
         * NotSupportedException       //Path is not in a valid format
         * UnauthorizedAccessException
         */

        private string FromFile()
        {
            try
            {
                return File.ReadAllText(SettingsFile, Encoding.UTF8);
            }
            catch (FileNotFoundException)
            {
                //Under the hoods the default settings will be serialized and written to the file
                //If this throws an exception, something is wrong and we won't handle it
                Settings = ObserverSettings.DefaultSettings;
                //Pull it back out, if the exception happens something is horible and we won't deal with it
                return File.ReadAllText(SettingsFile, Encoding.UTF8);
            }
        }

        private void ToFile(string json)
        {
            File.WriteAllText(SettingsFile, json, Encoding.UTF8);
        }
    }
}