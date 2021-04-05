using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWN.Framework.Lite;

namespace Source.Module
{
    class Load
    {
        // This method will be run whenever the script "x2_mod_def_load" is run. 
        // In our example module, this happens when the server finishes loading the module file.
        // Script names must adhere to the NWN restrictions (alphanumeric with some special characters and no longer than 16 characters)
        // The method name is arbitrary and can be called whatever you want.
        // Methods must be public and static so that the framework can pick them up when the module loads.
        [ScriptHandler("x2_mod_def_load")]
        public static void OnModuleLoad() => Console.WriteLine($"x2_mod_def_load launched at {DateTime.Now}.");
    }
}
