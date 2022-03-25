using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using Loon.Interfaces;
using Loon.Models;
using Microsoft.Win32;

namespace Loon.Services
{
    #if X86
    #pragma warning disable CA1416
    public sealed class SystemState : INotifyPropertyChanged, ISystemState
    {
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

        public event PropertyChangedEventHandler? PropertyChanged;

        private static string      ApplicationName          => ComputeMD5(AppContext.BaseDirectory);
        private static string      ComputeMD5(string input) => Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(input)));
        private static RegistryKey OpenStartupSubKey()      => Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)!;
    }

    #else
    public sealed class SystemState : NotifyPropertyChanged, ISystemState
    {
        private bool isRegisteredInStartup;

        public bool IsRegisteredInStartup
        {
            get => isRegisteredInStartup;
            set => SetProperty(ref isRegisteredInStartup, false);
        }
    }
    #endif
}