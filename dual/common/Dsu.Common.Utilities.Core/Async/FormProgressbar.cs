using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Dsu.Common.Utilities.ExtensionMethods;
using System.Threading.Tasks;

/*
 * - 문서 하단의 sample 참고
 * - Dsu.Common.Utilities.DX.DXWaitForm 참고
 */
namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Progressbar 를 이용한 report 를 수행하려는 form/UI 가 구현해야 할 interface.
    /// <br/> - Progress report 를 수행하려는 객체가 reference 해서 사용할 interface
    /// </summary>
    [ComVisible(true)]
    public interface IFormProgressbar
    {
        string ProgressCaption { get; set; }
        string ProgressDescription { get; set; }

        int ProgressTotal { get; set; }
        int ProgressPortion { get; set; }

        void AddProgressPortion(int portion);

        void StartProgressbar();
        void FinishProgressbar();
    }

    [ComVisible(false)]
    public interface IFormProgressbarCancelable : IFormProgressbar
    {
        CancellationToken CancellationToken { get; }
    }

    /// <summary>
    /// Cancel 을 지원하는 progress bar form.
    /// </summary>
    public partial class FormProgressbar : Form, IFormProgressbar
    {
        public string ProgressCaption { get { return Text; } set { Text = value; } }

        public string ProgressDescription
        {
            get { return _description; }
            set
            {
                _description = value;
                Task.Run(async () =>
                {
                    await this.DoAsync(() =>
                    {
                        label1.Text = String.Format("{0} {1}", _description, GetRealPercentageString());
                    });
                });
            }
        }

        private string _description;

        private string GetRealPercentageString()
        {                
            return String.Format("{0:0.##}", _portion * 100.0 / _total);
        }


        public CancellationToken CancellationToken { get { return _cts.Token; } }
        public int ProgressTotal { get { return _total; } set { _total = value; _portion = 0; } }
        private int _total;

        public int ProgressPortion
        {
            get { return _portion; } 
            set
            {
                _portion = value;
                Progress.Report(new ProgressReport() { ProgressPortion = _portion, Message = ProgressDescription });
            }
        }

        private int _portion;



        public void AddProgressPortion(int portion)
        {
            _portion += portion;
        }

        public void StartProgressbar()
        {
            Initialize();
        }

        public void FinishProgressbar() { Progress.Report(null); }
 
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private IProgress<ProgressReport> Progress { get { return _progress; }}
        private Progress<ProgressReport> _progress;
        public string ProgressbarText { get { return exProgressbar1.CenterText; } set { exProgressbar1.CenterText = value; } }

        public static IFormProgressbar StartProgressbar(string title, string label = null)
        {
            var form = new FormProgressbar() { ProgressCaption = title, ProgressDescription = label };
            form.Show();
            return form;
        }
        private void Initialize()
        {
            _total = 100;
            _portion = 0;
        }
        internal FormProgressbar()
        {
            InitializeComponent();
            Initialize();

            exProgressbar1.ShowPercentage = true;

            _progress = new Progress<ProgressReport>(pr =>
            {
                if (pr == null)
                {
                    Close();
                    return;
                }

                var showPercentage = pr.Message.IsNullOrEmpty();
                if (showPercentage != exProgressbar1.ShowPercentage)
                    exProgressbar1.ShowPercentage = showPercentage;

                exProgressbar1.Value = pr.ProgressPortion;
                exProgressbar1.CenterText = pr.Message;
            });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _cts.Cancel();
            Close();
        }



        class ProgressReport
        {
            public int ProgressPortion { get; set; }
            public string Message { get; set; }
        }
    }


    /// <summary>
    /// Dummy implementation of IFormProgressbar.
    /// <br/> Use DummyProgressbar.Instance as IFormProgressbar
    /// </summary>
    public class DummyProgressbar : IFormProgressbarCancelable
    {
        public string ProgressCaption { get; set; }
        public string ProgressDescription { get; set; }
        public CancellationToken CancellationToken { get; private set; }
        public int ProgressTotal { get; set; }
        public int ProgressPortion { get; set; }

        public void AddProgressPortion(int portion) {}

        public void StartProgressbar() {}
        public void FinishProgressbar() { }


        private DummyProgressbar(CancellationToken token)
        {
            CancellationToken = token;
        }

        public static DummyProgressbar Instance = new DummyProgressbar(new CancellationTokenSource().Token);
    }
}
















/*
        private async void testAsyncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Beep.Play();
            var form = new FormProgressbar() { Text = "Collecting simulation results...", Label = "Collecting result form dualsoftapp..." };
            form.Show();
            var result = await DoLongTimeTakingJobsAsync(form);
            MessageBox.Show(result ? "Finished!!!" : "Canceled!!");
        }

        private void testSyncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormProgressbar() { Text = "Collecting simulation results...", Label = "Collecting result form dualsoftapp..." };
            form.Show();

            //
            // needs additional thread to avoid main UI thread from blocking
            //
            System.Threading.Tasks.Task.Run(() =>
            {
                var result = DoLongTimeTakingJobs(form);
                MessageBox.Show(result ? "Finished!!!" : "Canceled!!");
            });
        }

        private bool DoLongTimeTakingJobs(IFormProgressbar progress)
        {
            CancellationToken ct = progress.CancellationToken;

            for (int i = 0; i < 100; i++)
            {
                if (ct.IsCancellationRequested)
                    throw new OperationCanceledException(token);

                progress.ReportPercentage(i);
                Thread.Sleep(100);
            }

            // null report notifies to close progress bar form.
            progress.Finish();

            return true;
        }


        private async Task<bool> DoLongTimeTakingJobsAsync(IFormProgressbar progress)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                return DoLongTimeTakingJobs(progress);
            }, progress.CancellationToken);
        }
 */
