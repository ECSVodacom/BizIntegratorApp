﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BizIntegrator.OrderManager.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.7.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool Enabled {
            get {
                return ((bool)(this["Enabled"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<RSAKeyValue><Modulus>qInYjKFVS+4ZB+SlGJs29uWm2utqkRjbeGLK1cmFIddIlUxKJcI2axDkcEEDwjGuxjraTjFdJeUVho6W8zxMhz9bYHFiq4Egp1ME96hIWfBEpyu5nUSYb9T7cFQFV7AB17weiZPN1IWVZNcdeExxTnehU95ucjZPP+cNVHQnYzE=</Modulus><Exponent>AQAB</Exponent><P>2u5SGxShB4jbaODSIlgm8SpIfXHpDqdKuway//BWavczIeWgu0ZEduGwnRZpRghT3PGk19eQer4906dFr6Q9cw==</P><Q>xRM8bbrHPLD3ZY1EpLGpVhulAtoCybA1/r8vtVIFv/1OQ5TSMhh2pojxOcf8aFLx+1OAUKoqBdSgIALcsXBzyw==</Q><DP>HfRzyX7GFajVFIGGXuqe0WqOppNaZwXexL5C6Z/xwwr1tRKdLmKL5ZQ1vRie6NUk3Fs3ycyV2SWEghwl/cVEHw==</DP><DQ>oUOIY1I/zs4Q46yhyxMGOkwMzzSOq3Ph+z9/TqR7yAsDjBGvnwadsNDGdD3NsaCOGtbNXTShhnvjO86bwSb/2Q==</DQ><InverseQ>Qoe4ulQGh8/SwYWJH6jPvj6H5Uc+NeSc+jN8eDOu4pAADu51TZv+9jMrv59H7k8Yp746GqD0KtlXJ/m2LrUcoQ==</InverseQ><D>SJkIJJuiKFDpi1LcIttJM9T8qLRbdSDl+NdlU+24YTlg1J7GGbmswzXFkO0Qd2f7Rvw26ROuchJZ0Vo+f4vr0msvv5GVTZNAQy7R5xBfZeDbyJKlb5WX6ucnjFdk2f8YHaa4i+EpXiJWuSS0Bor+Jryucph+3St1eCPlCL7agLk=</D></RSAKeyValue>")]
        public string PrivateKey {
            get {
                return ((string)(this["PrivateKey"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=192.168.101.36,1433;Initial Catalog=BizIntegrator;User ID=sparuser;Pa" +
            "ssword=ECsqlOnline!;MultipleActiveResultSets=true")]
        public string ConnectionString {
            get {
                return ((string)(this["ConnectionString"]));
            }
        }
    }
}
