using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using MakuTweakerNew.Properties;
using Microsoft.Win32;
using ModernWpf.Controls;

namespace MakuTweakerNew
{
    public partial class Telemetry : System.Windows.Controls.Page
    {
        private sealed class RegistryValueChange
        {
            public RegistryValueChange(string keyPath, string valueName, object value, RegistryValueKind valueKind = RegistryValueKind.DWord)
            {
                KeyPath = keyPath;
                ValueName = valueName;
                Value = value;
                ValueKind = valueKind;
            }

            public string KeyPath { get; }

            public string ValueName { get; }

            public object Value { get; }

            public RegistryValueKind ValueKind { get; }
        }

        private bool isLoaded = false;

        private bool isNotify = true;

        private bool isbycheck = false;

        private MainWindow mw = (MainWindow)Application.Current.MainWindow;

        private static void ApplyRegistryChanges(IEnumerable<RegistryValueChange> changes)
        {
            foreach (RegistryValueChange change in changes)
            {
                Registry.LocalMachine.CreateSubKey(change.KeyPath).SetValue(change.ValueName, change.Value, change.ValueKind);
            }
        }

        private static void ApplyRegistryChangesSafe(IEnumerable<RegistryValueChange> changes)
        {
            try
            {
                ApplyRegistryChanges(changes);
            }
            catch (Exception)
            {
            }
        }

        public Telemetry()
        {
            InitializeComponent();
            checkReg();
            LoadLang();
            isLoaded = true;
        }

        private void t1_Toggled(object sender, RoutedEventArgs e)
        {
            if (!isLoaded)
            {
                return;
            }
            Settings.Default.t1 = t1.IsOn;
            if (t1.IsOn)
            {
                ApplyRegistryChangesSafe(new[]
                {
                    new RegistryValueChange("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection", "AllowTelemetry", 0),
                    new RegistryValueChange("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection", "AllowTelemetry", 0),
                    new RegistryValueChange("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection", "MaxTelemetryAllowed", 0),
                    new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows NT\\CurrentVersion\\Software Protection Platform", "NoGenTicket", 1),
                    new RegistryValueChange("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection", "DoNotShowFeedbackNotifications", 1),
                    new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat", "AITEnable", 0),
                    new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat", "AllowTelemetry", 0),
                    new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat", "DisableEngine", 1),
                    new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat", "DisableInventory", 1),
                    new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat", "DisablePCA", 1),
                    new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat", "DisableUAR", 1)
                });
                isNotify = false;
                if (!isbycheck)
                {
                    t2.IsOn = true;
                    t3.IsOn = true;
                    t4.IsOn = true;
                    t5.IsOn = true;
                    t6.IsOn = true;
                    mw.RebootNotify(1);
                }
                isNotify = true;
            }
            else
            {
                ApplyRegistryChangesSafe(new[]
                {
                    new RegistryValueChange("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection", "AllowTelemetry", 1),
                    new RegistryValueChange("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection", "AllowTelemetry", 1),
                    new RegistryValueChange("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection", "MaxTelemetryAllowed", 1),
                    new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows NT\\CurrentVersion\\Software Protection Platform", "NoGenTicket", 0),
                    new RegistryValueChange("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection", "DoNotShowFeedbackNotifications", 0),
                    new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat", "AITEnable", 1),
                    new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat", "AllowTelemetry", 1),
                    new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat", "DisableEngine", 0),
                    new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat", "DisableInventory", 0),
                    new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat", "DisablePCA", 0),
                    new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat", "DisableUAR", 0)
                });
                isNotify = false;
                if (!isbycheck)
                {
                    t2.IsOn = false;
                    t3.IsOn = false;
                    t4.IsOn = false;
                    t5.IsOn = false;
                    t6.IsOn = false;
                    mw.RebootNotify(1);
                }
                isNotify = true;
            }
        }

        private void t2_Toggled(object sender, RoutedEventArgs e)
        {
            if (isLoaded)
            {
                Settings.Default.t2 = t2.IsOn;
                if (t2.IsOn)
                {
                    ApplyRegistryChangesSafe(new[]
                    {
                        new RegistryValueChange("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\CapabilityAccessManager\\ConsentStore\\appDiagnostics", "Value", "Deny", RegistryValueKind.String)
                    });
                }
                else
                {
                    ApplyRegistryChangesSafe(new[]
                    {
                        new RegistryValueChange("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\CapabilityAccessManager\\ConsentStore\\appDiagnostics", "Value", "Allow", RegistryValueKind.String)
                    });
                    isbycheck = true;
                    t1.IsOn = false;
                    isbycheck = false;
                }
                if (isNotify)
                {
                    mw.RebootNotify(1);
                }
                if (t2.IsOn && t3.IsOn && t4.IsOn && t5.IsOn && t6.IsOn)
                {
                    isbycheck = true;
                    t1.IsOn = true;
                    isbycheck = false;
                }
            }
        }

        private void t3_Toggled(object sender, RoutedEventArgs e)
        {
            if (isLoaded)
            {
                Settings.Default.t3 = t3.IsOn;
                if (t3.IsOn)
                {
                    ApplyRegistryChangesSafe(new[]
                    {
                        new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\System", "UploadUserActivities", 0),
                        new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\System", "PublishUserActivities", 0)
                    });
                }
                else
                {
                    ApplyRegistryChangesSafe(new[]
                    {
                        new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\System", "UploadUserActivities", 1),
                        new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\System", "PublishUserActivities", 1)
                    });
                    isbycheck = true;
                    t1.IsOn = false;
                    isbycheck = false;
                }
                if (isNotify)
                {
                    mw.RebootNotify(1);
                }
                if (t2.IsOn && t3.IsOn && t4.IsOn && t5.IsOn && t6.IsOn)
                {
                    isbycheck = true;
                    t1.IsOn = true;
                    isbycheck = false;
                }
            }
        }

        private void t4_Toggled(object sender, RoutedEventArgs e)
        {
            if (isLoaded)
            {
                Settings.Default.t4 = t4.IsOn;
                if (t4.IsOn)
                {
                    ApplyRegistryChangesSafe(new[]
                    {
                        new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\WDI\\{9c5a40da-b965-4fc3-8781-88dd50a6299d}", "ScenarioExecutionEnabled", 0),
                        new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\DeviceHealthAttestationService", "EnableDeviceHealthAttestationService", 0)
                    });
                }
                else
                {
                    ApplyRegistryChangesSafe(new[]
                    {
                        new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Windows\\WDI\\{9c5a40da-b965-4fc3-8781-88dd50a6299d}", "ScenarioExecutionEnabled", 1)
                    });
                    isbycheck = true;
                    t1.IsOn = false;
                    isbycheck = false;
                }
                if (isNotify)
                {
                    mw.RebootNotify(1);
                }
                if (t2.IsOn && t3.IsOn && t4.IsOn && t5.IsOn && t6.IsOn)
                {
                    isbycheck = true;
                    t1.IsOn = true;
                    isbycheck = false;
                }
            }
        }

        private void t5_Toggled(object sender, RoutedEventArgs e)
        {
            if (isLoaded)
            {
                Settings.Default.t5 = t5.IsOn;
                if (t5.IsOn)
                {
                    ApplyRegistryChangesSafe(new[]
                    {
                        new RegistryValueChange("SOFTWARE\\Microsoft\\InputPersonalization", "RestrictImplicitTextCollection", 0),
                        new RegistryValueChange("SOFTWARE\\Microsoft\\InputPersonalization", "RestrictImplicitInkCollection", 0)
                    });
                }
                else
                {
                    ApplyRegistryChangesSafe(new[]
                    {
                        new RegistryValueChange("SOFTWARE\\Microsoft\\InputPersonalization", "RestrictImplicitTextCollection", 1),
                        new RegistryValueChange("SOFTWARE\\Microsoft\\InputPersonalization", "RestrictImplicitInkCollection", 1)
                    });
                    isbycheck = true;
                    t1.IsOn = false;
                    isbycheck = false;
                }
                if (isNotify)
                {
                    mw.RebootNotify(1);
                }
                if (t2.IsOn && t3.IsOn && t4.IsOn && t5.IsOn && t6.IsOn)
                {
                    isbycheck = true;
                    t1.IsOn = true;
                    isbycheck = false;
                }
            }
        }

        private void t6_Toggled(object sender, RoutedEventArgs e)
        {
            if (isLoaded)
            {
                Settings.Default.t6 = t6.IsOn;
                if (t6.IsOn)
                {
                    ApplyRegistryChangesSafe(new[]
                    {
                        new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Speech", "AllowSpeechModelUpdate", 0)
                    });
                }
                else
                {
                    ApplyRegistryChangesSafe(new[]
                    {
                        new RegistryValueChange("SOFTWARE\\Policies\\Microsoft\\Speech", "AllowSpeechModelUpdate", 1)
                    });
                    isbycheck = true;
                    t1.IsOn = false;
                    isbycheck = false;
                }
                if (isNotify)
                {
                    mw.RebootNotify(1);
                }
                if (t2.IsOn && t3.IsOn && t4.IsOn && t5.IsOn && t6.IsOn)
                {
                    isbycheck = true;
                    t1.IsOn = true;
                    isbycheck = false;
                }
            }
        }

        private void LoadLang()
        {
            string languageCode = Settings.Default.lang ?? "en";
            Dictionary<string, Dictionary<string, string>> tel = MainWindow.Localization.LoadLocalization(languageCode, "tel");
            Dictionary<string, Dictionary<string, string>> basel = MainWindow.Localization.LoadLocalization(languageCode, "base");
            label.Text = tel["main"]["label"];
            t1.Header = tel["main"]["t1"];
            t2.Header = tel["main"]["t2"];
            t3.Header = tel["main"]["t3"];
            t4.Header = tel["main"]["t4"];
            t5.Header = tel["main"]["t5"];
            t6.Header = tel["main"]["t6"];
            t1.OffContent = basel["def"]["off"];
            t2.OffContent = basel["def"]["off"];
            t3.OffContent = basel["def"]["off"];
            t4.OffContent = basel["def"]["off"];
            t5.OffContent = basel["def"]["off"];
            t6.OffContent = basel["def"]["off"];
            t1.OnContent = basel["def"]["on"];
            t2.OnContent = basel["def"]["on"];
            t3.OnContent = basel["def"]["on"];
            t4.OnContent = basel["def"]["on"];
            t5.OnContent = basel["def"]["on"];
            t6.OnContent = basel["def"]["on"];
        }

        private void checkReg()
        {
            ToggleSwitch toggleSwitch = t1;
            RegistryKey? registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection");
            int isOn;
            if (registryKey != null && registryKey.GetValue("AllowTelemetry")?.Equals(0) == true)
            {
                RegistryKey? registryKey2 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection");
                if (registryKey2 != null && registryKey2.GetValue("AllowTelemetry")?.Equals(0) == true)
                {
                    RegistryKey? registryKey3 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection");
                    if (registryKey3 != null && registryKey3.GetValue("MaxTelemetryAllowed")?.Equals(0) == true)
                    {
                        RegistryKey? registryKey4 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\Windows NT\\CurrentVersion\\Software Protection Platform");
                        isOn = ((registryKey4 != null && registryKey4.GetValue("NoGenTicket")?.Equals(1) == true) ? 1 : 0);
                        goto IL_0132;
                    }
                }
            }
            isOn = 0;
            goto IL_0132;

        IL_0132:
            toggleSwitch.IsOn = (byte)isOn != 0;
            t2.IsOn = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\CapabilityAccessManager\\ConsentStore\\appDiagnostics")?.GetValue("Value")?.ToString() == "Deny";
            ToggleSwitch toggleSwitch2 = t3;
            RegistryKey? registryKey5 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\Windows\\System");
            int isOn2;
            if (registryKey5 == null || registryKey5.GetValue("UploadUserActivities")?.Equals(0) != true)
            {
                RegistryKey? registryKey6 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\Windows\\System");
                isOn2 = ((registryKey6 != null && registryKey6.GetValue("PublishUserActivities")?.Equals(0) == true) ? 1 : 0);
            }
            else
            {
                isOn2 = 1;
            }
            toggleSwitch2.IsOn = (byte)isOn2 != 0;
            ToggleSwitch toggleSwitch3 = t4;
            RegistryKey? registryKey7 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\Windows\\WDI\\{9c5a40da-b965-4fc3-8781-88dd50a6299d}");
            int isOn3;
            if (registryKey7 != null && registryKey7.GetValue("ScenarioExecutionEnabled")?.Equals(0) == true)
            {
                RegistryKey? registryKey8 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\DeviceHealthAttestationService");
                isOn3 = ((registryKey8 != null && registryKey8.GetValue("EnableDeviceHealthAttestationService")?.Equals(0) == true) ? 1 : 0);
            }
            else
            {
                isOn3 = 0;
            }
            toggleSwitch3.IsOn = (byte)isOn3 != 0;
            ToggleSwitch toggleSwitch4 = t5;
            RegistryKey? registryKey9 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\InputPersonalization");
            int isOn4;
            if (registryKey9 == null || registryKey9.GetValue("RestrictImplicitTextCollection")?.Equals(0) != true)
            {
                RegistryKey? registryKey10 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\InputPersonalization");
                isOn4 = ((registryKey10 != null && registryKey10.GetValue("RestrictImplicitInkCollection")?.Equals(0) == true) ? 1 : 0);
            }
            else
            {
                isOn4 = 1;
            }
            toggleSwitch4.IsOn = (byte)isOn4 != 0;
            ToggleSwitch toggleSwitch5 = t6;
            RegistryKey? registryKey11 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\Speech");
            toggleSwitch5.IsOn = registryKey11 != null && registryKey11.GetValue("AllowSpeechModelUpdate")?.Equals(0) == true;
            if (t1.IsOn)
            {
                t2.IsOn = true;
                t3.IsOn = true;
                t4.IsOn = true;
                t5.IsOn = true;
                t6.IsOn = true;
            }
        }
    }
}
