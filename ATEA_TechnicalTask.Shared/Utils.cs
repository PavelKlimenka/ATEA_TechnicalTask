﻿using System.Reflection;

namespace ATEA_TechnicalTask.Shared
{
    public static class Utils
    {
        public static string GetExecutableDirectoryPath()
        {
            return Path.GetFullPath(Path.Combine(Assembly.GetExecutingAssembly().Location, ".."));
        }
    }
}
