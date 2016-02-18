using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOLib {
    public class AOLibClass {

        public static void Init() {
            Settings.init("Data/MainSettings.xml");
            Logger.init(Settings.Single.logFolder, "AOService");
        }
    }
}
