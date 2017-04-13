using System;
using System.Reflection;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Dsu.Common.Utilities.Exceptions;

namespace Dsu.Common.Utilities.Controls
{
    /// <summary>
    /// 'Most Recently Used' list manager
    /// http://www.codeproject.com/Articles/32154/Create-a-Recent-File-List-Menu-and-Save-to-File
    /// </summary>
    public class MRUManager
    {
        public static readonly LogProxy logger = LogProxy.CreateLoggerProxy(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Main Form 의 "Recent Files.." 에 해당하는 toolstrip
        /// </summary>
        private ToolStripMenuItem _mruMenu;

        /// <summary> MRC 항목 중의 하나가 선택되었을 때에 수행할 작업 지정 </summary>
        private EventHandler _handlerMruFileSelected;

        /// <summary> MUR item 항목이 변경될 때에 수행할 작업 지정 </summary>
        /// e.g : thumbnail 그리기 등.
        private EventHandler _handlerMruItemChanged;

        /// <summary> MRC 항목을 저장할 file 이름 </summary>
        private string _mruListFile;

        public string LastFile { get { return _mruList.FirstOrDefault(); } }
        /// <summary>
        /// MRU list : [0] 가 가장 최근의 항목
        /// </summary>
        private List<string> _mruList = new List<string>();
        public IEnumerable<string> MRUlist { get { return _mruList; } }

        private int _mruSize = 12;
        public int MRUSize { get { return _mruSize; } set { _mruSize = value; } }

        public IEnumerable<ToolStripMenuItem> MruItems { get { return _mruMenu.DropDownItems.Cast<ToolStripMenuItem>(); } }


        private void AddMenuItem(ToolStripMenuItem mruItem)
        {
            _mruMenu.DropDownItems.Add(mruItem); //add the menu to "recent" menu
            _handlerMruItemChanged.Handle(mruItem, new EventArgs());
        }
        public MRUManager(ToolStripMenuItem mruMenu, EventHandler mruFileSelectedHandler, EventHandler mruItemChangedHandler, string mruListFile)
        {
            _mruMenu = mruMenu;
            _handlerMruFileSelected = mruFileSelectedHandler;
            _handlerMruItemChanged = mruItemChangedHandler;
            _mruListFile = mruListFile;

            LoadList();
            foreach (string item in MRUlist)
            {
                //create new menu for each item in list
                AddMenuItem(new ToolStripMenuItem(item, null, RecentFile_click));
            }
        }

        private void RecentFile_click(object sender, EventArgs e)
        {
            _handlerMruFileSelected(sender.ToString(), new EventArgs());
        }

        public void Add(string file)
        {
            _mruMenu.DropDownItems.Clear(); //clear all recent list from menu
            LoadList(); //load list from file

            if (_mruList.Contains(file))
                _mruList.Remove(file);

            //prevent duplication on recent list
            _mruList.Insert(0, file); //insert given path into list

            if (_mruList.Count > _mruSize) //keep list number not exceeded given value
                _mruList.RemoveRange(_mruSize, (_mruList.Count - _mruSize));

            foreach (string item in MRUlist)
            {
                ToolStripMenuItem fileRecent = new ToolStripMenuItem(item, null, RecentFile_click);  //create new menu for each item in list
                AddMenuItem(fileRecent); //add the menu to "recent" menu
            }
            //writing menu list to file
            StreamWriter stringToWrite = new StreamWriter(_mruListFile); //create file called "Recent.txt" located on app folder
            foreach (string item in MRUlist)
            {
                stringToWrite.WriteLine(item); //write list to stream
            }
            stringToWrite.Flush(); //write stream to file
            stringToWrite.Close(); //close the stream and reclaim memory            
        }


        /// <summary>
        /// try to load file. If file isn't found, do nothing
        /// </summary>
        private void LoadList()
        {
            _mruList.Clear();

            ExceptionHider.DoSilently(() =>
                {
                    if (File.Exists(_mruListFile))
                    {
                        StreamReader listToRead = new StreamReader(_mruListFile); //read file stream
                        string line;
                        while ((line = listToRead.ReadLine()) != null) //read each line until end of file
                            _mruList.Add(line); //insert to list
                        listToRead.Close(); //close the stream
                    }
                    else
                        logger.WarnFormat("MRU file {0} not exists.", _mruListFile);
                });
        }
    }
}
