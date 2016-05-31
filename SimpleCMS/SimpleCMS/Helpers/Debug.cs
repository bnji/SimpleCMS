using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General.Helpers
{
    public class Debug
    {
        public static bool IsDebugMode
        {
            get
            {
                bool debugging = false;
                DetermineDebugMode(ref debugging);
                return debugging;
            }
        }

        [Conditional("DEBUG")]
        private static void DetermineDebugMode(ref bool debugging)
        {
            debugging = true;
        }
    }
}
