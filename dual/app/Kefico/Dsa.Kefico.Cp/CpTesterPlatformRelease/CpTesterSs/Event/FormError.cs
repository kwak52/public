using Dsu.Driver.Util.Emergency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reactive.Linq;
using Dsu.Common.Utilities.ExtensionMethods;
using CpTesterPlatform.CpTester;
using System.Threading.Tasks;

namespace CpTesterSs.Event
{
    public partial class FormError : Form
    {
        public static FormError TheForm;
        private bool _closeRequestedByProgram = false;
        private static List<Tuple<SignalEnum, string>> _signals = new List<Tuple<SignalEnum, string>>();
        public static void AddSignalDescription(SignalEnum signal, string description) => _signals.Add(Tuple.Create(signal, description));
        public static async void ShowErrorForm()
        {
            if (_signals.Any() && FormError.TheForm == null)
            {
                await FormAppSs.TheMainFrame.DoAsync(() =>
                {
                    var form = new FormError();
                    form.DoModal();
                });
            }
        }

        public FormError()
        {
            InitializeComponent();
            TheForm = this;
        }

        public FormError(SignalEnum signal, string description)
            : this()
        {
            AddReason(signal, description);
            labelDescription.Text = description;
        }

        public FormError(string description)
        : this()
        {
            labelDescription.Text = description;
        }

        public void DoModal()
        {
            FormAppSs.TheMainForm.Enabled = false;
            Show();
        }

        private void FormError_Load(object sender, EventArgs e)
        {
            labelDescription.Text = String.Join("\r\n", _signals.Select(tpl => tpl.Item2));
        }


        public void AddReason(SignalEnum signal, string description)
        {
            _signals.Add(Tuple.Create(signal, description));
            labelDescription.Text += "\r\n" + description;
        }

        public void AddReason(string description)
        {
            labelDescription.Text += "\r\n" + description;
        }


        private async Task RedrawError()
        {
            await this.DoAsync(() =>
            {
                labelDescription.Text = String.Join("\r\n", _signals.Select(tpl => tpl.Item2));
            });
        }

        //public static void ClearError(SignalEnum signal)
        //{
        //    _signals.RemoveAll(t => t.Item1 == signal);
        //    if (_signals.IsNullOrEmpty() && _theForm != null)
        //    {
        //        _theForm.Close();
        //        _theForm = null;
        //    }
        //}
        public static async void ClearError(SignalEnum signal)
        {
            _signals.RemoveAll(tpl => tpl.Item1 == signal);
            if (TheForm != null)
            {
                if (_signals.Any())
                {
                    await TheForm.RedrawError();
                }
                else
                {
                    await TheForm.DoAsync(() =>
                    {
                        TheForm._closeRequestedByProgram = true;
                        TheForm.Close();
                        TheForm = null;
                    });
                }
            }
        }

        public static async Task ClearAllErrors()
        {
            _signals.Clear();
            if (TheForm != null)
            {
                await TheForm.DoAsync(() =>
                {
                    TheForm.Close();
                    TheForm = null;
                });
            }
        }

        private void FormError_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (!_closeRequestedByProgram)
            //{
            //    if (FormAdmin.DoModal())
            //        _signals.Clear();
            //    else
            //    {
            //        e.Cancel = true;
            //        return;
            //    }
            //}

            _signals.Clear();
            TheForm = null;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormError_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormAppSs.TheMainForm.Enabled = true;
        }
    }
}
