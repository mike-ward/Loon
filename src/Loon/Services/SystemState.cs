using System;
using System.ComponentModel;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using Loon.Interfaces;
using Microsoft.Win32;

namespace Loon.Services
{
    public class SystemState : ISystemState, INotifyPropertyChanged
    {
        [SupportedOSPlatform("windows")]
        public bool IsRegisteredInStartup
        {
            get
            {
                using var registryKey = OpenStartupSubKey();
                return registryKey.GetValue(ApplicationName) is not null;
            }

            set
            {
                if (IsRegisteredInStartup == value) return;

                using var registryKey = OpenStartupSubKey();
                if (value)
                {
                    var path = $"\"{AppContext.BaseDirectory}Loon.exe\"";
                    registryKey.SetValue(ApplicationName, path);
                }
                else
                {
                    registryKey.DeleteValue(ApplicationName);
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRegisteredInStartup)));
            }
        }

        private static string ApplicationName => ComputeMD5(AppContext.BaseDirectory);

        [SupportedOSPlatform("windows")]
        private static RegistryKey OpenStartupSubKey() => Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)!;

        private static string ComputeMD5(string input)
        {
            using var md5 = MD5.Create();

            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes  = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}