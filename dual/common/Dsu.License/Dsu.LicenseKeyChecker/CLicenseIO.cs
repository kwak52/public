using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace Dsu.LicenseKeyChecker
{
    internal class CLicenseIO : IDisposable
    {

        #region Member Variables

        private byte[] m_arBytes = ASCIIEncoding.ASCII.GetBytes("Dual2016");

        #endregion


        #region Initialize/Dispose

        public CLicenseIO()
        {

        }

        public void Dispose()
        {

        }

        #endregion


        #region Public Properties


        #endregion


        #region Public Methods

        internal bool Write(string sPath, CLicenseInfo cInfo)
        {
            bool bOK = false;

            if (cInfo == null)
                return false;

            RegistryKey reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("DualSoft");

            byte[] arBytes = Serialize(cInfo);
            if (arBytes == null || arBytes.Length == 0)
                return false;

            arBytes = Encrypt(arBytes);
            if (arBytes == null || arBytes.Length == 0)
                return false;

            reg.SetValue(sPath, arBytes);
            reg.Close();

            return bOK;
        }

        internal CLicenseInfo Read(string sPath)
        {
            CLicenseInfo cInfo = null;
            RegistryKey reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("DualSoft");

            byte[] arBytes = (byte[])reg.GetValue(sPath);
            if (arBytes == null || arBytes.Length == 0)
                return null;

            arBytes = Decrypt(arBytes);
            if (arBytes == null || arBytes.Length == 0)
                return null;

            cInfo = DeSerialize(arBytes);
            reg.Close();

            return cInfo;
        }


        #endregion


        #region Private Methods

        private byte[] Serialize(CLicenseInfo cInfo)
        {
            byte[] arBytes = null;

            MemoryStream cStream = new MemoryStream();
            BinaryFormatter cFormatter = new BinaryFormatter();

            try
            {
                cFormatter.Serialize(cStream, cInfo);
                arBytes = cStream.ToArray();

            }
            catch (System.Exception ex)
            {
                ex.Data.Clear();
                arBytes = null;
            }
            finally
            {
                if (cFormatter != null)
                    cFormatter = null;

                if (cStream != null)
                {
                    cStream.Close();
                    cStream.Dispose();
                    cStream = null;
                }
            }

            return arBytes;
        }

        private CLicenseInfo DeSerialize(byte[] arBytes)
        {
            CLicenseInfo cInfo = null;

            MemoryStream cStream = null;
            BinaryFormatter cFormatter = new BinaryFormatter();

            try
            {
                cStream = new MemoryStream(arBytes);
                cInfo = (CLicenseInfo)cFormatter.Deserialize(cStream);
            }
            catch (System.Exception ex)
            {
                ex.Data.Clear();
                arBytes = null;
            }
            finally
            {
                if (cFormatter != null)
                    cFormatter = null;

                if (cStream != null)
                {
                    cStream.Close();
                    cStream.Dispose();
                    cStream = null;
                }
            }

            return cInfo;
        }

        private byte[] Encrypt(byte[] arBytesOrg)
        {
            byte[] arBytes = null;
            MemoryStream cMemStream = null;
            CryptoStream cCryptStream = null;
            BinaryWriter cWriter = null;

            try
            {
                DESCryptoServiceProvider cProvider = new DESCryptoServiceProvider();
                cMemStream = new MemoryStream();
                cCryptStream = new CryptoStream(cMemStream, cProvider.CreateEncryptor(m_arBytes, m_arBytes), CryptoStreamMode.Write);
                cWriter = new BinaryWriter(cCryptStream);

                cWriter.Write(arBytesOrg);
                cWriter.Flush();
                cCryptStream.FlushFinalBlock();
                cWriter.Flush();

                arBytes = cMemStream.ToArray();
            }
            catch (System.Exception ex)
            {
                ex.Data.Clear();
                arBytesOrg = null;
            }
            finally
            {
                if (cMemStream != null)
                {
                    cMemStream.Close();
                    cMemStream.Dispose();
                    cMemStream = null;
                }

                if (cCryptStream != null)
                {
                    cCryptStream.Close();
                    cCryptStream.Dispose();
                    cCryptStream = null;
                }

                if (cWriter != null)
                {
                    cWriter.Close();
                    cWriter = null;
                }
            }

            return arBytes;
        }

        private byte[] Decrypt(byte[] arBytesOrg)
        {
            byte[] arBytes = null;

            MemoryStream cMemStream = null;
            CryptoStream cCryptStream = null;
            BinaryReader cReader = null;

            try
            {
                DESCryptoServiceProvider cProvider = new DESCryptoServiceProvider();
                cMemStream = new MemoryStream(arBytesOrg);
                cCryptStream = new CryptoStream(cMemStream, cProvider.CreateDecryptor(m_arBytes, m_arBytes), CryptoStreamMode.Read);

                cReader = new BinaryReader(cCryptStream);
                arBytes = cReader.ReadBytes((int)cMemStream.Length);

            }
            catch (System.Exception ex)
            {
                ex.Data.Clear();
            }
            finally
            {
                if (cMemStream != null)
                {
                    cMemStream.Close();
                    cMemStream.Dispose();
                    cMemStream = null;
                }

                if (cCryptStream != null)
                {
                    cCryptStream.Close();
                    cCryptStream.Dispose();
                    cCryptStream = null;
                }

                if (cReader != null)
                {
                    cReader.Close();
                    cReader = null;
                }
            }

            return arBytes;
        }

        private bool WriteFile(string sPath, byte[] arBytes)
        {
            bool bOK = true;

            FileStream cWriter = null;

            try
            {
                cWriter = new FileStream(sPath, FileMode.Create, FileAccess.Write);
                cWriter.Write(arBytes, 0, arBytes.Length);
                cWriter.Flush();
            }
            catch (System.Exception ex)
            {
                ex.Data.Clear();
                bOK = false;
            }
            finally
            {
                if (cWriter != null)
                {   
                    cWriter.Close();
                    cWriter.Dispose();
                    cWriter = null;
                }
            }

            return bOK;
        }

        private byte[] ReadFile(string sPath)
        {
            byte[] arBytes = null;

            FileStream cReader = null;

            try
            {
                cReader = new FileStream(sPath, FileMode.Open, FileAccess.Read);
                int iLength = (int)cReader.Length;
                arBytes = new byte[iLength];

                cReader.Read(arBytes, 0, iLength);
            }
            catch (System.Exception ex)
            {
                ex.Data.Clear();
            }
            finally
            {
                if (cReader != null)
                {
                    cReader.Close();
                    cReader.Dispose();
                }
            }

            return arBytes;
        }        

        #endregion
    }
}
