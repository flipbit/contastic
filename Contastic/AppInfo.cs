using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Contastic
{
    /// <summary>
    /// Helper class to return information about the currently executing application
    /// </summary>
    public class AppInfo
    {
        /// <summary>
        /// The filename of the application
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// The shell prompt to use in help text.
        /// ">" for Windows
        /// "$" for Linux/MacOS
        /// </summary>
        public string Prompt { get; }

        /// <summary>
        /// Gets the application product name
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// The application description
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the application version
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Gets the date when the application was built
        /// </summary>
        public DateTime BuildDate { get; }

        public AppInfo() : this(Assembly.GetEntryAssembly())
        {
        }

        public AppInfo(Assembly assembly)
        {
            Title = GetAssemblyTitle(assembly);
            Description = GetAssemblyDescription(assembly);
            Version = GetAssemblyVersion(assembly);
            FileName = Path.GetFileName(assembly.Location);
            BuildDate = File.GetCreationTime(FileName);
            #if NETSTANDARD
            Prompt = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ">" : "$";
            #else
            Prompt = ">";
            #endif
        }

        private static string GetAssemblyVersion(Assembly assembly)
        {
            var version = string.Empty;

            var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (attribute != null)
            {
                version = attribute.InformationalVersion;
            }

            return version;
        }

        private static string GetAssemblyTitle(Assembly assembly)
        {
            var info = FileVersionInfo.GetVersionInfo(assembly.Location);
            var title = info.ProductName;

            if (!string.IsNullOrEmpty(title)) return title;

            var attribute = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
            if (attribute != null)
            {
                title = attribute.Title;
            }

            return title;
        }

        private static string GetAssemblyDescription(Assembly assembly)
        {
            var description = string.Empty;

            var attribute = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
            if (attribute != null)
            {
                description = attribute.Description;
            }

            return description;
        }
    }
}
