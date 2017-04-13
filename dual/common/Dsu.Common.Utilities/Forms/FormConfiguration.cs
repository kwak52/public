using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;
using Dsu.Common.Interfaces;

namespace Dsu.Common.Utilities
{
    [Guid("E3FD24D7-CAF4-4454-A198-7829158270CA")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [ProgId("Dsu.Common.Utilities.FormConfiguration")]
    public partial class FormConfiguration : Form
    {
        protected object _selectedObject;
        protected RegistryKey _registryKey;
        public FormConfiguration()
        {
            InitializeComponent();
        }


        //[MethodSignaturesCritical]
        public virtual void Initialize(object configuration, RegistryKey registryKey, Icon icon)
        {
            _selectedObject = configuration;
            _registryKey = registryKey;
            Icon = icon;
            Location = Cursor.Position;
        }

        private void FormConfiguration_Load(object sender, System.EventArgs e)
        {
            if (_selectedObject is IRegistrySerializable)
                RegistrySerializer.FromRegistry((IRegistrySerializable)_selectedObject, _registryKey);

            propertyGrid1.SelectedObject = _selectedObject;
        }

        private void FormConfiguration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_selectedObject is IRegistrySerializable)
                RegistrySerializer.ToRegistry((IRegistrySerializable)_selectedObject, _registryKey);
        }
    }
}
