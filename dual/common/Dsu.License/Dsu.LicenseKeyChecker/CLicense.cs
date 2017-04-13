using System;
using System.IO;
using System.Windows.Forms;



namespace Dsu.LicenseKeyChecker
{
    public class CLicense : IDisposable
    {
        #region Member Variables

        private string LicenseVersion = "V1.1";

        private string m_sProduct = "";
        private string m_sAppPath = "";

        private CLicenseInfo m_cLicenseInfo = null;

        #endregion


        #region Initailze/Dispose

        public CLicense(string sProduct, string sAppPath)
        {
			m_sProduct = sProduct;
			m_sAppPath = sAppPath;
        }

        public void Dispose()
        {
            if (m_cLicenseInfo != null)
                Write(m_cLicenseInfo);
        }

        #endregion


        #region Public Properties

        public string Product
        {
            get { return m_sProduct; }
            set { m_sProduct = value; }
        }

        public string AppDirectory
        {
            set {m_sAppPath = value;}
        }

        public bool IsLicensed
        {
			get { if (m_cLicenseInfo != null) { return m_cLicenseInfo.IsLicensed; } else { return false; } }
        }

        #endregion


        #region Public Methods

        public bool CheckLicense()
        {
            bool bOK = false;

			m_cLicenseInfo = Read();
			if(m_cLicenseInfo == null || m_cLicenseInfo.IsLicensed == false)
			{
				string sCode = "";
				if(m_cLicenseInfo != null && m_cLicenseInfo.ActivationCode != null)
					sCode = m_cLicenseInfo.ActivationCode;
				else
					sCode = CLicenseActivator.GenerateActivationCode(m_sProduct);

				m_cLicenseInfo = ShowLicenseForm(m_sProduct, sCode);
				if (m_cLicenseInfo == null)
					m_cLicenseInfo = new CLicenseInfo();

				CompleteLicenseInfo(m_cLicenseInfo);
			}

			if (m_cLicenseInfo.IsLicensed)
			{
				bOK = CompareHardware(m_cLicenseInfo);
				if (bOK == false)
					return false;


				m_cLicenseInfo.ActivationCode = "";
				m_cLicenseInfo.ActivationKey = "";

				if (m_cLicenseInfo.IsDemo)
				{
					bOK = UpdateRemainsMinutes(m_cLicenseInfo);
					if (bOK)
					{
                        int RemainsDays = m_cLicenseInfo.DemoDays - m_cLicenseInfo.UsingDays;

                        if (RemainsDays==0)
                            MessageBox.Show("Demo License be exhausted....Please Request the Package in due form.. ", "Dsu License", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        MessageBox.Show(RemainsDays.ToString() + " Day(s) Remains for Trial License !!", "Dsu License", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					else
					{
						m_cLicenseInfo.IsLicensed = false;

						MessageBox.Show("Trial License is Expired!!", "Dsu License", MessageBoxButtons.OK, MessageBoxIcon.Error);
						bOK = false;
					}
				}
			}

			Write(m_cLicenseInfo);

            return bOK;
        }

        private double Format(double p1, string p2)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Private Methods

        private void Write(CLicenseInfo cInfo)
        {
            CLicenseIO cIO = new CLicenseIO();
            cIO.Write(Product, cInfo);
            cIO.Dispose();
            cIO = null;
        }

        private CLicenseInfo Read()
        {
			CLicenseIO cIO = new CLicenseIO();
			CLicenseInfo cInfo = cIO.Read( Product);

			cIO.Dispose();
			cIO = null;

            return cInfo;
        }

        private bool HasLincenseFile()
        {
            bool bOK = false;

            string sPath = "";
            if (m_sAppPath.Trim() == "")
                return false;

            sPath = m_sAppPath.Trim() + "\\License.dat";
            bOK = File.Exists(sPath);

            return bOK;
        }

        private bool CompareHardware(CLicenseInfo cInfo)
        {
            if (cInfo == null)
                return false;

            if (cInfo.Product.Trim() != m_sProduct.Trim())
                return false;

            CHardwareInfo cMachine = new CHardwareInfo();

			string sValue = "";

            sValue = cMachine.GetProcessorID();
            if (cInfo.ProcessID.Trim() != sValue.Trim())
            {
                cMachine.Dispose();
                cMachine = null;
                return false;
            }

            sValue = cMachine.GetHardDiskID(cInfo.HardDiskModel);
            if (cInfo.HardDiskID.Trim() != sValue.Trim())
            {
                cMachine.Dispose();
                cMachine = null;
                return false;
            }

			//sValue = cMachine.GetNetworkMACAddress(cInfo.NetworkModel);
			//if (cInfo.MacAddress.Trim() != sValue.Trim())
			//{
			//	cMachine.Dispose();
			//	cMachine = null;
			//	return false;
			//}

            return true;
        }

		private CLicenseInfo ShowLicenseForm(string sProduct, string sCode)
        {
			FrmLicense frmLicense = new FrmLicense(sProduct, sCode);
            frmLicense.ShowDialog();

			CLicenseInfo cInfo = frmLicense.LicenseInfo;
			return cInfo;
        }

        private bool CompleteLicenseInfo(CLicenseInfo cInfo)
        {
            CHardwareInfo cMachine = new CHardwareInfo();

            cInfo.Product = m_sProduct;
            cInfo.ProcessModel = cMachine.GetProcessorModel();
            cInfo.ProcessID = cMachine.GetProcessorID();
            cInfo.HardDiskModel = cMachine.GetHardDiskModel(0);
            cInfo.HardDiskID = cMachine.GetHardDiskID(0);
            cInfo.NetworkModel = cMachine.GetNetworkModel(0);
            cInfo.MacAddress = cMachine.GetNetworkMACAddress(0);
            cInfo.LicensedTime = DateTime.Now;
            cInfo.ExcuteTime = DateTime.Now;
			cInfo.RemainsMinutes = ((double)(cInfo.DemoDays * 24 + cInfo.DemoHours)) * 60;

            cMachine.Dispose();
            cMachine = null;

			return true;
        }

        private bool UpdateRemainsMinutes(CLicenseInfo cInfo)
        {
            if (cInfo.RemainsMinutes <= 0)
                return false;

            bool bOK = true;

            DateTime dtTime = DateTime.Now;
            if (dtTime > cInfo.ExcuteTime)
            {
                TimeSpan tsSpan = dtTime.Subtract(cInfo.ExcuteTime);
                cInfo.RemainsMinutes -= tsSpan.TotalMinutes;

                TimeSpan tsSpanDays = dtTime.Subtract(cInfo.LicensedTime);
                cInfo.UsingDays = (int)tsSpanDays.TotalDays;

            }
            else if(cInfo.ExcuteTime > dtTime)
            {
                cInfo.RemainsMinutes -= 10 * 24 * 60;//10days to minutes                
            }

            if (cInfo.RemainsMinutes <= 0)
            {
                cInfo.RemainsMinutes = -1;
                bOK = false;
            }

            cInfo.ExcuteTime = dtTime;

            return bOK;
        }

        #endregion


        #region Event Methods


        #endregion
    }
}
