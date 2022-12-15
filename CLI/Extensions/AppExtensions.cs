﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Extensions
{
    internal static class AppExtensions
    {
        public static string AddArguments(this App app, string arg1, string arg2) 
        {
            if(int.TryParse(arg1, out int arg1int) && int.TryParse(arg2, out int arg2int))
            {
                return (arg1int + arg2int).ToString();
            }

            if(float.TryParse(arg1, out float arg1float) && float.TryParse(arg2, out float arg2float))
            {
                return (arg1float + arg2float).ToString();
            }

            return arg1 + arg2;
        }
    }
}
