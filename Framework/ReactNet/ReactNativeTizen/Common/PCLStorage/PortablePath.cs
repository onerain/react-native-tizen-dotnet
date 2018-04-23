using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PCLStorage
{
    /// <summary>
    /// Provides portable versions of APIs such as Path.Combine
    /// </summary>
    public static class PortablePath
    {
        /// <summary>
        /// The character used to separate elements in a file system path
        /// </summary>
        public static char DirectorySeparatorChar
        {
            get
            {
                return Path.DirectorySeparatorChar;
            }
        }

        /// <summary>
        /// Combines multiple strings into a path
        /// </summary>
        /// <param name="paths">Path elements to combine</param>
        /// <returns>A combined path</returns>
        public static string Combine(params string[] paths)
        {
            return Path.Combine(paths);
        }
    }
}
