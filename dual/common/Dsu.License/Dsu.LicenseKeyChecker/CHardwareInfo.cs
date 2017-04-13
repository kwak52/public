using System;
using System.Management;

namespace Dsu.LicenseKeyChecker
{
    internal class CHardwareInfo : IDisposable
    {
        #region Member Variables

        private ManagementObjectSearcher m_mosSearcher = null;

        #endregion


        #region Initailize/Dispose

        public CHardwareInfo()
        {
            m_mosSearcher = new ManagementObjectSearcher();
        }

        public void Dispose()
        {
            if (m_mosSearcher != null)
            {
                m_mosSearcher.Dispose();
                m_mosSearcher = null;
            }
        }

        #endregion


        #region Public Properties


        #endregion


        #region Public Methods

        #region Processor

        public string GetProcessorModel()
        {
            string sValue = "";
            object oValue = GetHardwareInfo("Win32_Processor", "Name");
            if (oValue != null)
                sValue = oValue.ToString();

            return sValue;
        }

        public string GetProcessorID()
        {
            string sValue = "";
            object oValue = GetHardwareInfo("Win32_Processor", "ProcessorID");
            if (oValue != null)
                sValue = oValue.ToString();

            return sValue;
        }

        public int GetCoreCount()
        {
            int iCount = 0;
            object oValue = GetHardwareInfo("Win32_Processor", "NumberOfCores");
            if (oValue != null)
            {
                string sValue = oValue.ToString();
                iCount = int.Parse(sValue.Trim());
            }

            return iCount;
        }

        #endregion

        #region HardDisk

        public string GetHardDiskModel(int iIndex)
        {
            string sValue = "";
            object oValue = GetHardwareInfo("Win32_DiskDrive", "Model", iIndex);
            if (oValue != null)
                sValue = oValue.ToString();

            return sValue;
        }

        public string GetHardDiskID(int iIndex)
        {
            string sValue = "";
            object oValue = GetHardwareInfo("Win32_DiskDrive", "SerialNumber", iIndex);
            if (oValue != null)
                sValue = oValue.ToString();

            return sValue;
        }

        public string GetHardDiskID(string sModel)
        {
            sModel = "'" + sModel + "'";

            string sValue = "";            
            object oValue = GetHardwareInfo("Win32_DiskDrive", "SerialNumber", "Model", sModel);
            if (oValue != null)
                sValue = oValue.ToString();

            return sValue;
        }

        public long GetHardDiskSize(int iIndex)
        {
            long nValue = 0;
            object oValue = GetHardwareInfo("Win32_DiskDrive", "Size", iIndex);
            if (oValue != null)
            {
                string sValue = oValue.ToString();
                nValue = long.Parse(sValue);
            }

            return nValue;
        }

        public long GetHardDiskSize(string sModel)
        {
            sModel = "'" + sModel + "'";

            long nValue = 0;
            object oValue = GetHardwareInfo("Win32_DiskDrive", "Size", "Model", sModel);
            if (oValue != null)
            {
                string sValue = oValue.ToString();
                nValue = long.Parse(sValue);
            }

            return nValue;
        }

        #endregion


        #region Network Adapter

        public string GetNetworkModel(int iIndex)
        {
            string sValue = "";
            object oValue = GetHardwareInfo("Win32_NetworkAdapter", "Description", iIndex, "PhysicalAdapter", "True");
            if (oValue != null)
                sValue = oValue.ToString();

            return sValue;
        }

        public string GetNetworkMACAddress(int iIndex)
        {
            string sValue = "";
            object oValue = GetHardwareInfo("Win32_NetworkAdapter", "MACAddress", iIndex, "PhysicalAdapter", "True");
            if (oValue != null)
                sValue = oValue.ToString();

            return sValue;
        }

        public string GetNetworkMACAddress(string sModel)
        {
            sModel = "'" + sModel + "'";

            string sValue = "";
            object oValue = GetHardwareInfo("Win32_NetworkAdapter", "MACAddress", "Description", sModel);
            if (oValue != null)
                sValue = oValue.ToString();

            return sValue;
        }

        #endregion

        #endregion


        #region Private Methods

        private object GetHardwareInfo(string sCategory, string sKey)
        {
            object oValue = null;

            try
            {
                if (m_mosSearcher != null)
                {
                    m_mosSearcher.Query = new ObjectQuery("select " + sKey + " from " + sCategory);
                    foreach (ManagementObject oItem in m_mosSearcher.Get())
                    {
                        if (oItem == null || oItem.Properties.Count == 0)
                            break;

                        foreach (PropertyData oData in oItem.Properties)
                        {
                            oValue = oData.Value;
                            break;
                        }

                        if (oValue != null)
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                ex.Data.Clear();
                oValue = null;
            }

            return oValue;
        }

        private object GetHardwareInfo(string sCategory, string sKey, int iIndex)
        {
            object oValue = null;
            int iCount = 0;

            try
            {
                if (m_mosSearcher != null)
                {
                    m_mosSearcher.Query = new ObjectQuery("select " + sKey + " from " + sCategory);
                    foreach (ManagementObject oItem in m_mosSearcher.Get())
                    {
                        if (oItem == null || oItem.Properties.Count == 0)
                            break;

                        if (iCount == iIndex)
                        {
                            foreach (PropertyData oData in oItem.Properties)
                            {
                                oValue = oData.Value;
                                break;
                            }
                        }
                        else if (iCount < iIndex)
                            iCount += 1;
                        else
                            break;

                        if (oValue != null)
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                ex.Data.Clear();
                oValue = null;
            }

            return oValue;
        }

        private object GetHardwareInfo(string sCategory, string sKey, string sConstrainKey, string sConstrainValue)
        {
            object oValue = null;

            try
            {
                if (m_mosSearcher != null)
                {
                    m_mosSearcher.Query = new ObjectQuery("select " + sKey + " from " + sCategory + " where " + sConstrainKey + " = " + sConstrainValue);
                    foreach (ManagementObject oItem in m_mosSearcher.Get())
                    {
                        if (oItem == null || oItem.Properties.Count == 0)
                            break;

                        foreach (PropertyData oData in oItem.Properties)
                        {
                            oValue = oData.Value;
                            break;
                        }

                        if (oValue != null)
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                ex.Data.Clear();
                oValue = null;
            }

            return oValue;
        }

        private object GetHardwareInfo(string sCategory, string sKey, int iIndex, string sConstrainKey, string sConstrainValue)
        {
            object oValue = null;
            int iCount = 0;

            try
            {
                if (m_mosSearcher != null)
                {
                    m_mosSearcher.Query = new ObjectQuery("select " + sKey + " from " + sCategory + " where " + sConstrainKey + " = " + sConstrainValue);
                    foreach (ManagementObject oItem in m_mosSearcher.Get())
                    {
                        if (oItem == null || oItem.Properties.Count == 0)
                            break;

                        if (iCount == iIndex)
                        {
                            foreach (PropertyData oData in oItem.Properties)
                            {
                                oValue = oData.Value;
                                break;
                            }
                        }
                        else if (iCount < iIndex)
                            iCount += 1;
                        else
                            break;

                        if (oValue != null)
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                ex.Data.Clear();
                oValue = null;
            }

            return oValue;
        }

        #endregion
    }
}
