using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOLib {
    public class AOLibClass {

        public static void Init() {
            string path = Assembly.GetExecutingAssembly().Location;
            FileInfo fileInfo = new FileInfo(path);
            string dir = fileInfo.DirectoryName;
            Settings.init(dir+"/Data/MainSettings.xml");
            Logger.init(Settings.Single.logFolder, "AOService");
        }
    }
}
