using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CpCommon.ExtensionMethods;
using DevExpress.XtraBars.Ribbon;
using static CpCommon.ExceptionHandler;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities;

namespace CpTesterPlatform.CpTester
{
    partial class FormAppSs
    {
        private const string _layoutFilePath = "layout.txt";

        /// <summary>
        /// Tester 환경 설정을 저장
        /// 1. Main form 의 위치, 크기 저장
        /// 1. Check button 상태 저장
        /// 1. Main ribbon control 의 선택 page 저장
        /// </summary>
        private void LoadLayout()
        {
            TryAction(() =>
            {
                if (!Globals._isSkipLoadLayout)
                {
                    GetDockManagers().ForEach(tpl =>
                  {
                      var name = tpl.Item1;
                      var dm = tpl.Item2;
                      dm.RestoreLayoutFromRegistry($"CpTesterApp\\Layout\\{name}");
                  });
                }

                if (!File.Exists(_layoutFilePath))
                    return;
                
                // line : "<CheckStatus>	SelectedMainTab = ribbonPageTsOption"
                var lines = File.ReadAllLines(_layoutFilePath);
                EmFormLayoutSaveLoad.LoadLayoutFromLines(this, lines);
                

                var selectedTabDictionary =
                        (from l in lines
                                    .Where(l => l.StartsWith("<SelectedTab>"))
                                    .Select(l => l.Replace("<SelectedTab>\t", ""))
                         let regex = Regex.Match(l, @"([^=]*)=(.*)")
                         let name = regex.Groups[1].ToString()
                         let selectedPageName = regex.Groups[2].ToString()
                         select new { name, selectedPageName })
                            .ToDictionary(info => info.name, info => info.selectedPageName)
                    ;
                   

                var pages = ribbonControlCPTester.Pages.Cast<RibbonPage>().ToArray();
                var selectedPage = pages.FirstOrDefault(p => p.Name == selectedTabDictionary["SelectedMainTab"]);
                if (selectedPage != null)
                    ribbonControlCPTester.SelectedPage = selectedPage;


                var checkStatusDictionary = (from l in lines
                    .Where(l => l.StartsWith("<CheckStatus>"))
                    .Select(l => l.Replace("<CheckStatus>\t", ""))
                                  let regex = Regex.Match(l, @"([^=]*)=(.*)")
                                  let name = regex.Groups[1].ToString()
                                  let isChecked = Boolean.Parse(regex.Groups[2].ToString())
                                  select new { name, isChecked })
                    .ToDictionary(info => info.name, info => info.isChecked)
                    ;

                //barCheckItem1.Checked = dictionary["barCheckItem1"];
                barCheckItemDebugConsoleOut.Checked = checkStatusDictionary["barCheckItemDebugConsoleOut"];
                barCheckItemLogCANMsg.Checked = checkStatusDictionary["barCheckItemLogCANMsg"];
                barCheckItemLogSaveMeasuringLog.Checked = checkStatusDictionary["barCheckItemLogSaveMeasuringLog"];        // not working. why???
                barCheckItemSaveConsoleLog.Checked = checkStatusDictionary["barCheckItemSaveConsoleLog"];
                barCheckItemShowColComment.Checked = checkStatusDictionary["barCheckItemShowColComment"];
                barCheckItemShowColGate.Checked = checkStatusDictionary["barCheckItemShowColGate"];
                barCheckItemShowColParameter.Checked = checkStatusDictionary["barCheckItemShowColParameter"];
                barCheckItemShowColPosition.Checked = checkStatusDictionary["barCheckItemShowColPosition"];
                barCheckItemShowColReturn.Checked = checkStatusDictionary["barCheckItemShowColReturn"];
                barCheckItemShowColVariant.Checked = checkStatusDictionary["barCheckItemShowColVariant"];
                barCheckItemShowDeactivateStep.Checked = checkStatusDictionary["barCheckItemShowDeactivateStep"];
                barCheckItemShowColInf.Checked = checkStatusDictionary.ContainsKey("barCheckItemShowColInf") ? checkStatusDictionary["barCheckItemShowColInf"] : false;
                barCheckItemShowColMP.Checked = checkStatusDictionary.ContainsKey("barCheckItemShowColMP") ? checkStatusDictionary["barCheckItemShowColMP"] : false;

                if (Size.Width < 200 || Size.Height < 200)
                    Size = new System.Drawing.Size(1000, 500);
                if (!Location.X.InClosedRange(0, 500) || !Location.Y.InClosedRange(0, 300))
                    Location = new System.Drawing.Point(100, 100);
            });
        }

     

        private void SaveLayout()
        {
            TryAction(() =>
            {
                GetDockManagers().ForEach(tpl =>
                {
                    var name = tpl.Item1;
                    var dm = tpl.Item2;
                    // HKCU\CpTesterApp\Layout
                    dm.SaveLayoutToRegistry($"CpTesterApp\\Layout\\{name}");
                });

                string layout = EmFormLayoutSaveLoad.SaveLayout(this);
                layout += $"\r\n<SelectedTab>\tSelectedMainTab={ribbonControlCPTester.SelectedPage.Name}";

                //layout += $"\r\n<CheckStatus>\barCheckItem1={barCheckItem1.Checked}";
                layout += $"\r\n<CheckStatus>\tbarCheckItemDebugConsoleOut={barCheckItemDebugConsoleOut.Checked}";
                layout += $"\r\n<CheckStatus>\tbarCheckItemLogCANMsg={barCheckItemLogCANMsg.Checked}";
                layout += $"\r\n<CheckStatus>\tbarCheckItemLogSaveMeasuringLog={barCheckItemLogSaveMeasuringLog.Checked}";
                layout += $"\r\n<CheckStatus>\tbarCheckItemSaveConsoleLog={barCheckItemSaveConsoleLog.Checked}";
                layout += $"\r\n<CheckStatus>\tbarCheckItemShowColComment={barCheckItemShowColComment.Checked}";
                layout += $"\r\n<CheckStatus>\tbarCheckItemShowColGate={barCheckItemShowColGate.Checked}";
                layout += $"\r\n<CheckStatus>\tbarCheckItemShowColParameter={barCheckItemShowColParameter.Checked}";
                layout += $"\r\n<CheckStatus>\tbarCheckItemShowColPosition={barCheckItemShowColPosition.Checked}";
                layout += $"\r\n<CheckStatus>\tbarCheckItemShowColReturn={barCheckItemShowColReturn.Checked}";
                layout += $"\r\n<CheckStatus>\tbarCheckItemShowColVariant={barCheckItemShowColVariant.Checked}";
                layout += $"\r\n<CheckStatus>\tbarCheckItemShowDeactivateStep={barCheckItemShowDeactivateStep.Checked}";
                layout += $"\r\n<CheckStatus>\tbarCheckItemShowColInf={barCheckItemShowColInf.Checked}";
                layout += $"\r\n<CheckStatus>\tbarCheckItemShowColMP={barCheckItemShowColMP.Checked}";


                File.WriteAllText(_layoutFilePath, layout);
            });
        }
    }
}
