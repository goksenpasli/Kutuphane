﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kutuphane.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("300")]
        public int DecodeHeight {
            get {
                return ((int)(this["DecodeHeight"]));
            }
            set {
                this["DecodeHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public double GünlükGecikmeBedeli {
            get {
                return ((double)(this["GünlükGecikmeBedeli"]));
            }
            set {
                this["GünlükGecikmeBedeli"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("15")]
        public double HızlıKitapGirişGünSüresi {
            get {
                return ((double)(this["HızlıKitapGirişGünSüresi"]));
            }
            set {
                this["HızlıKitapGirişGünSüresi"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool Aktarma {
            get {
                return ((bool)(this["Aktarma"]));
            }
            set {
                this["Aktarma"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool KitapResimGöster {
            get {
                return ((bool)(this["KitapResimGöster"]));
            }
            set {
                this["KitapResimGöster"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool KişiGirişEkranıVarsayılan {
            get {
                return ((bool)(this["KişiGirişEkranıVarsayılan"]));
            }
            set {
                this["KişiGirişEkranıVarsayılan"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("7")]
        public double YaklaşanİşlemlerGünSayısı {
            get {
                return ((double)(this["YaklaşanİşlemlerGünSayısı"]));
            }
            set {
                this["YaklaşanİşlemlerGünSayısı"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double QrMultipleBorderThickness {
            get {
                return ((double)(this["QrMultipleBorderThickness"]));
            }
            set {
                this["QrMultipleBorderThickness"] = value;
            }
        }
    }
}
