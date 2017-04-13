using CpTesterPlatform.CpMngLib.Manager;
using CpTesterPlatform.CpTester;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Driver.Base;
using Dsu.Driver.Util.Emergency;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace CpTesterSs.UIManual
{
    public partial class FormDoorControl : Form
    {
        private class Door
        {
            public SignalEnum Sensor { get; private set; }
            public SignalEnum Lock { get; private set; }
            public Label Label { get; private set; }
            public Button ButtonLock { get; private set; }
            public Button ButtonUnlock { get; private set; }
            public Door(SignalEnum sensor, SignalEnum locker, Label label, Button btnLock, Button btnUnlock)
            {
                Sensor = sensor;
                Lock = locker;
                Label = label;
                ButtonLock = btnLock;
                ButtonUnlock = btnUnlock;
            }
        }

        /// Door {sensor, lock} signals.  lock signal 은 undefined 일 수도 있다. 
        private List<Door> _doors;
        private List<CpMngDIOControl> _dios;
        public FormDoorControl(IEnumerable<CpMngDIOControl> dios)
        {
            InitializeComponent();
            _dios = dios.ToList();

            if (DriverBaseGlobals.IsAudit78())
            {
                _doors = new[] {
                    new Door(SignalEnum.UDoor1, SignalEnum.UDoorLock1, label1, btnLock1, btnUnlock1),
                    new Door(SignalEnum.UDoor2, SignalEnum.UDoorLock2, label2, btnLock2, btnUnlock2),
                    new Door(SignalEnum.UDoor3, SignalEnum.UDoorLock3, label3, btnLock3, btnUnlock3),
                    new Door(SignalEnum.UDoor4, SignalEnum.UDoorLock4, label4, btnLock4, btnUnlock4),
                    }.ToList();
            }
            else if (DriverBaseGlobals.IsAuditGCVT())
            {
                _doors = new[] {
                    new Door(SignalEnum.UDoor1, SignalEnum.UDoorLock1, label1, btnLock1, btnUnlock1),
                    new Door(SignalEnum.UDoor2, SignalEnum.UDoorLock2, label2, btnLock2, btnUnlock2),
                    new Door(SignalEnum.UDoor3, SignalEnum.Undefined,  label3, btnLock3, btnUnlock3),
                    new Door(SignalEnum.UDoor4, SignalEnum.Undefined,  label4, btnLock4, btnUnlock4),
                    }.ToList();
            }
        }

        private Font _boldLabelFont;
        private Font _labelFont;
        private void OnDoorOpenStatusChanged(Door d, bool opened)
        {
            var parsedSignalT = SignalManager.GetParsedSignal(d.Sensor);
            var name = parsedSignalT.Message;
            d.Label.Text = name + (opened ? " opened" : " closed");
            if (_boldLabelFont == null)
            {
                _labelFont = d.Label.Font;
                _boldLabelFont = new Font(d.Label.Font, FontStyle.Bold);
            }

            d.Label.Font = opened ? _boldLabelFont : _labelFont;
            
            if (opened)
            {
                d.ButtonLock.Enabled = false;
                d.ButtonUnlock.Enabled = false;
            }
            else
            {
                var locked = ! CpSignalManager.GetDio(d.Lock, _dios);
                d.ButtonLock.Enabled = !locked;
                d.ButtonUnlock.Enabled = locked;
            }

        }

        private void FormDoorControl_Load(object sender, EventArgs args)
        {
            if (!FormAdmin.DoModal())
                return;

            _doors.ForEach(d =>
            {
                d.ButtonLock.Visible = true;
                d.ButtonUnlock.Visible = true;
                var opened = CpSignalManager.GetDio(d.Sensor, _dios);
                OnDoorOpenStatusChanged(d, opened);

                d.ButtonLock.Click += (s, e) =>
                {
                    CpSignalManager.SetDioOff(d.Lock, _dios);       // OFF 시 잠김
                    d.ButtonLock.Enabled = false;
                    d.ButtonUnlock.Enabled = true;
                };

                d.ButtonUnlock.Click += (s, e) =>
                {
                    CpSignalManager.SetDioOn(d.Lock, _dios);
                    d.ButtonLock.Enabled = true;
                    d.ButtonUnlock.Enabled = false;
                };
            });

            var subscription =
                SignalManager.FilteredSignalSubject
                .Subscribe(fs =>
                {
                    var sensedDoor = _doors.FirstOrDefault(d => d.Sensor == fs.Enum);
                    if ( sensedDoor != null )
                    {
                        this.Do(() =>
                        {
                            var opened = fs.Value;
                            OnDoorOpenStatusChanged(sensedDoor, opened);
                        });
                    }
                });

            FormClosed += (s, e) => subscription.Dispose();
        }
    }
}
