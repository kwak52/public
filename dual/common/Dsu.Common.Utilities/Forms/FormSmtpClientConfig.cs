using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Windows.Forms;
using Microsoft.Win32;
using Dsu.Common.Interfaces;
using Dsu.Common.Utilities.Exceptions;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities.Forms
{
    /// <summary>
    /// Mail 전송을 위한 smtp server, 사용자 계정, 비번 등을 설정하기 위한 form
    /// <para/> - 한번 설정된 정보는 registry 에 저장할 수 있다.
    /// <para/> - 다음 설정시에, registry 에 해당 정보가 기록되어 있으면 이를 사용한다.
    /// </summary>
    public partial class FormSmtpClientConfig : FormHelper
    {
        // HKLM\Software\Dualsoft
        private const string _registrySubKey = @"Software\Dualsoft\BugReportSmtpConfig";
        private RegistryKey _registryKey;
        private const string _cryptoKey = "foobarbaz";

        public string Server
        {
            get { return comboBoxServer.Text; }
            private set
            {
                int n = comboBoxServer.Items.IndexOf(value);
                if (n < 0)
                    n = comboBoxServer.Items.Add(value);

                comboBoxServer.SelectedIndex = n;
            }
        }

        public int Port { get { return numericTextBoxPort.GetIntValue(); } private set { numericTextBoxPort.Text = value.ToString(); } }
        public string User { get { return textBoxUserEmail.Text; } private set { textBoxUserEmail.Text = value; } }
        public string Password { get { return textBoxPassword.Text; } private set { textBoxPassword.Text = value; } }
        public bool IsEnableSSL { get { return checkBoxEnableSSL.Checked; } private set { checkBoxEnableSSL.Checked = value; } }

        public FormSmtpClientConfig()
        {
            InitializeComponent();
            ContextHelp.SetHelp(this, "bug-reporting.html");
        }

        private void FormSmtpClientConfig_Load(object sender, EventArgs e)
        {
            // 해당 키를 찾아보고 없으면 새로 생성
            _registryKey = Registry.LocalMachine.OpenSubKey(_registrySubKey, true);
            if (_registryKey == null)
                _registryKey = Registry.LocalMachine.CreateSubKey(_registrySubKey);
            else
            {
                Server = (string)_registryKey.GetValue("Server");
                Port = (int)_registryKey.GetValue("Port");
                User = (string)_registryKey.GetValue("User");
                Password = Crypto.Decrypt((string)_registryKey.GetValue("Password"), _cryptoKey);
                IsEnableSSL = bool.Parse((string)_registryKey.GetValue("IsEnableSSL"));
            }

            foreach (var server in new []{"google.com", "yahoo.com", "daum.net"})
                comboBoxServer.Items.Add(server);

            comboBoxServer.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (checkBoxSave.Checked)
            {
                _registryKey.SetValue("Server", Server);
                _registryKey.SetValue("Port", Port);
                _registryKey.SetValue("User", User);
                _registryKey.SetValue("Password", Crypto.Encrypt(Password, _cryptoKey));
                _registryKey.SetValue("IsEnableSSL", IsEnableSSL);
            }


            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Registry 에 smtp 관련 정보들이 기록되어 있으면 이 정보를 가공해서 반환하고,
        /// 없다면 form 을 통해서 정보를 수집하여 (registry 에 저장하고), 반환한다.
        /// 비밀번호는 encrytpion 해서 저장된다.
        /// </summary>
        /// <returns></returns>
        public static SmtpClientEx GetSmtpClientConfig()
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(_registrySubKey, false);
            if (registryKey == null)
            {
                using ( var dlg = new FormSmtpClientConfig() )
                {
                    if (DialogResult.OK == dlg.ShowDialog())
                    {
                        return new SmtpClientEx(dlg.Server, dlg.Port)
                            {
                                EnableSsl = dlg.IsEnableSSL,
                                Timeout = 10000,
                                DeliveryMethod = SmtpDeliveryMethod.Network,
                                UseDefaultCredentials = false,
                                User = dlg.User,
                                Password = Crypto.Encrypt(dlg.Password, _cryptoKey),
                                Credentials = new System.Net.NetworkCredential(dlg.User, Crypto.Encrypt(dlg.Password, _cryptoKey))
                            };
                    }
                }
            }
            else
            {
                string server = (string)registryKey.GetValue("Server");
                int port = (int)registryKey.GetValue("Port");
                string user = (string)registryKey.GetValue("User");
                string password = Crypto.Decrypt((string)registryKey.GetValue("Password"), _cryptoKey);
                bool isEnableSSL = bool.Parse((string)registryKey.GetValue("IsEnableSSL"));

                return new SmtpClientEx(server, port)
                    {
                        EnableSsl = isEnableSSL,
                        Timeout = 10000,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new System.Net.NetworkCredential(user, password),
                        User = user,
                        Password = password
                    };
            }

            return null;
        }

        private void action1_Update(object sender, EventArgs e)
        {
            btnOK.Enabled = !String.IsNullOrEmpty(numericTextBoxPort.Text)
                            && !String.IsNullOrEmpty(Server)
                            && !String.IsNullOrEmpty(User)
                            && !String.IsNullOrEmpty(Password)
                ;
        }

    }
}
