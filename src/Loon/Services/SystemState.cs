﻿using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using Loon.Interfaces;
using Microsoft.Win32;

namespace Loon.Services
{
    #if Windows32
#pragma warning disable CA1416

    public sealed class SystemState : INotifyPropertyChanged, ISystemState
    {
        public bool IsRegisteredInStartup
        {
            get
            {
                using var registryKey = OpenStartupSubKey();
                return registryKey.GetValue(ApplicationNameHash) is not null;
            }

            set
            {
                if (IsRegisteredInStartup == value) return;

                using var registryKey = OpenStartupSubKey();
                if (value)
                {
                    var path = $"\"{AppContext.BaseDirectory}Loon.exe\"";
                    registryKey.SetValue(ApplicationNameHash, path);
                }
                else
                {
                    registryKey.DeleteValue(ApplicationNameHash);
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRegisteredInStartup)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public static  string      ApplicationNameHash      => ComputeMD5(AppContext.BaseDirectory);
        private static string      ComputeMD5(string input) => Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(input)));
        private static RegistryKey OpenStartupSubKey()      => Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)!;
    }

    #else
    using Loon.Models;

    public sealed class SystemState : NotifyPropertyChanged, ISystemState
    {
        private bool isRegisteredInStartup;

        public bool IsRegisteredInStartup
        {
            get => isRegisteredInStartup;
            set => SetProperty(ref isRegisteredInStartup, false);
        }

        public static  string      ApplicationNameHash      => ComputeMD5(AppContext.BaseDirectory);
        private static string      ComputeMD5(string input) => Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(input)));
    }
    #endif
}