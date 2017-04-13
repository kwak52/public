using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// SVN information 표출
    /// 생성 과정
    /// 1. DualsoftApp.exe project build 시, post build event 에서 svninfo 명령으로 svn-version.txt 파일 생성
    /// 1. DualsoftApp-Inno-Setup.iss 에서 setup build 시, svn-version.txt 파일 내용을 system registry 에 등록 (Software\Dualsoft\.DualsoftApp\SVNInfo)
    /// 1. 해당 registry 읽어서 정보 추출
    /// </summary>
    public static class SvnInfo
    {
        private static int _version;
        private static string _branch, _repositoryRoot, _author, _url;
        private static DateTime _lastChangeDate, _packageInstallDate;
        private static string _applicationTitleSuffix;

        public static int Version { get { return _version; } }
        public static string Branch { get { return _branch; } }
        public static DateTime LastChangeDate { get { return _lastChangeDate; } }
        public static string RepositoryRoot { get { return _repositoryRoot; } }
        public static string Author { get { return _author; } }
        public static string URL { get { return _url; } }
        public static string ApplicationTitleSuffix { get { return _applicationTitleSuffix; }}
        /// <summary> Application 설치 일자 </summary>
        public static DateTime PackageInstallDate { get { return _packageInstallDate; } }

        /// <summary>
        /// SVN 정보를 parsing 한다.  input 은 "svn info ." 명령어의 출력을 대상으로 한다.
        /// </summary>
        [Obsolete("Use ReadSvnInfoFromTextFile() instead.   Some computer has problem writing multiline text in a registry key.")]
        /* */   private static void ReadSvnInfoFromRegistry()
        /* */   {
        /* */       bool win32 = !Environment.Is64BitProcess;
        /* */       RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Dualsoft\.DualsoftApp");
        /* */       if (registryKey != null)
        /* */       {
        /* */           string bulidDate = (string)registryKey.GetValue("PackageInstallDate");
        /* */           _applicationTitleSuffix = String.Format("@x{0} [r{1}@{2}, {3}]", win32 ? "86" : "64", Version, Branch, bulidDate);
        /* */           _packageInstallDate = DateTime.Parse(bulidDate);
        /* */   
        /* */           //
        /* */           // 구분자 ':' 를 기준으로 key 와 value 의 sequence 로 분류하고, 이를 dictionary 에 저장한다.
        /* */           //
        /* */           var rawSvnInfo = (string)registryKey.GetValue("SVNVersion");
        /* */           var query = from row in rawSvnInfo.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
        /* */                       let regex = Regex.Match(row, @"([^\:]+): (.*)")
        /* */                       where regex.Groups.Count == 3
        /* */                       let key = regex.Groups[1].ToString()
        /* */                       let value = regex.Groups[2].ToString()
        /* */                       select new { key = key, value = value }
        /* */               ;
        /* */   
        /* */           var dict = new Dictionary<string, string>();
        /* */           foreach (var pr in query)
        /* */               dict.Add(pr.key, pr.value);
        /* */   
        /* */           _version = Int32.Parse(dict["Revision"]);
        /* */           _url = dict["URL"];
        /* */           _repositoryRoot = dict["Repository Root"];
        /* */           _branch = dict["Relative URL"].Replace("^", "");
        /* */           _author = dict["Last Changed Author"];
        /* */   
        /* */           var rawDateTime = dict["Last Changed Date"];    // e.g "2016-03-15 15:08:30 +0900 (화, 15 3 2016)"
        /* */           var dateTime = Regex.Replace(rawDateTime, @"(\+.*)", ""); // e.g "2016-03-15 15:08:30"
        /* */           _lastChangeDate = DateTime.Parse(dateTime);
        /* */       }
        /* */   }


        /// <summary>
        /// SVN 정보를 parsing 한다.  input 은 "svn info ." 명령어의 출력을 대상으로 한다.
        /// </summary>
        private static void ReadSvnInfoFromTextFile()
        {
            bool win32 = !Environment.Is64BitProcess;
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Dualsoft\.DualsoftApp");
            if (registryKey != null)
            {
                string bulidDate = (string)registryKey.GetValue("PackageInstallDate");
                _applicationTitleSuffix = String.Format("@x{0} [r{1}@{2}, {3}]", win32 ? "86" : "64", Version, Branch, bulidDate);
                _packageInstallDate = DateTime.Parse(bulidDate);
            }

            var dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var infoFile = Path.Combine(dir, "svn-version.txt");

            /*
             * 구분자 ':' 를 기준으로 key 와 value 의 sequence 로 분류하고, 이를 dictionary 에 저장한다.
             */
            var rawSvnInfo = File.ReadAllText(infoFile);
            var query = from row in rawSvnInfo.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        let regex = Regex.Match(row, @"([^\:]+): (.*)")
                        where regex.Groups.Count == 3
                        let key = regex.Groups[1].ToString()
                        let value = regex.Groups[2].ToString()
                        select new { key = key, value = value }
                ;

            var dict = new Dictionary<string, string>();
            foreach (var pr in query)
                dict.Add(pr.key, pr.value);

            _version = Int32.Parse(dict["Revision"]);
            _url = dict["URL"];
            _repositoryRoot = dict["Repository Root"];
            _branch = dict["Relative URL"].Replace("^", "");
            _author = dict["Last Changed Author"];

            var rawDateTime = dict["Last Changed Date"];    // e.g "2016-03-15 15:08:30 +0900 (화, 15 3 2016)"
            var dateTime = Regex.Replace(rawDateTime, @"(\+.*)", ""); // e.g "2016-03-15 15:08:30"
            _lastChangeDate = DateTime.Parse(dateTime);

        }

        static SvnInfo()
        {
            try { ReadSvnInfoFromTextFile(); }
            catch (Exception ex) {  MessageBox.Show("Error on getting SVN info : " + ex.Message); }  
        }
    }
}
