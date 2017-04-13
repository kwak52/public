using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities.DX
{
    /// <summary>
    /// DevXpress gridview 에서 자주 사용하는 option 들을 저장하는 class.
    /// <br/> 본 class 객체는 static 으로 선언하고, gridview 가 변경될 때에 이전에 설정된 option 을 재사용하는 형태로 사용한다.
    /// 
    /// 기능
    /// <br/> Column 별 show/hide 상태
    /// <br/> Column 별 width
    /// <br/> Column 배열 순서
    /// <br/> Top-level band 배열 순서
    /// 
    /// FormViewSymbolTable = "MultiSelect=True;MultiSelectMode=CellSelect;ShowAutoFilterRow=False;ShowGroupPanel=True;VisibleColumns=colName=75,colAddress=75,colPlcId=75,colValue=75,colDataType=75,colOBRAPs=75,colOBRAAs=75,colOBRAEs=75,colOBFAPs=75,colOBFAAs=75,colOBFAEs=75,colTaskScriptRead=75,colIBRSNs=75,colIBRSFs=75,colIBRTSs=75,colIBRTFs=75,colIBFSNs=75,colIBFSFs=75,colIBFTSs=75,colIBFTFs=75,colTaskScriptWritten=75,colNote=75,colCategory=75;VisibleBands=Symbol,OB,IB,Etc;"
    /// </summary>
    public class DXGridViewOptionHolder
    {
        private Form _containerForm;
        private GridView _gridView;
        private BandedGridView BandedGridView { get {  return _gridView as BandedGridView; } }

        private bool _showAutoFilterRow;
        private bool _showGroupPanel;
        private bool _multiSelect;
        private GridMultiSelectMode _multiSelectMode;

        /// <summary>
        /// visible column : name, => {position, width} pair
        /// </summary>
        private Dictionary<string, KeyValuePair<int, int>> _visibleColumnWidths = new Dictionary<string, KeyValuePair<int, int>>();
        private List<string> _visibleBands = new List<string>();

        private void ToGridView()
        {
            _gridView.OptionsSelection.MultiSelect = _multiSelect;
            _gridView.OptionsSelection.MultiSelectMode = _multiSelectMode;
            _gridView.OptionsView.ShowAutoFilterRow = _showAutoFilterRow;
            _gridView.OptionsView.ShowGroupPanel = _showGroupPanel;
            _gridView.Columns.ForEach(c => c.Visible = _visibleColumnWidths.IsNullOrEmpty() || _visibleColumnWidths.ContainsKey(c.Name));
            _gridView.Columns.ForEach(c =>
            {
                if ( ! c.Visible )
                    c.HideCascading();
            });
            _visibleColumnWidths.ForEach(pr =>
            {
                try
                {
                    var col = _gridView.Columns.FirstOrDefault(c => c.Name == pr.Key);
                    if ( col != null )
                    {
                        col.VisibleIndex = pr.Value.Key;
                        col.Width = pr.Value.Value;
                    }
                }
                catch (Exception ex)
                {
                    CommonApplication.Logger.ErrorFormat("Exception on DXGridViewOptionHolder.ToGridView() method : {0}", ex.Message);
                }
            });

            for (int i = 0; i < _visibleBands.Count; i++)
                BandedGridView.Bands.MoveTo(i, BandedGridView.Bands[_visibleBands[i]]);
        }

        private void FromGridView()
        {
            _visibleColumnWidths.Clear();
            _multiSelect = _gridView.OptionsSelection.MultiSelect;
            _multiSelectMode = _gridView.OptionsSelection.MultiSelectMode;
            _showAutoFilterRow = _gridView.OptionsView.ShowAutoFilterRow;
            _showGroupPanel = _gridView.OptionsView.ShowGroupPanel;
            _gridView.Columns.Where(c => c.Visible).ForEach(c =>
            {
                _visibleColumnWidths.Add(c.Name, new KeyValuePair<int, int>(c.VisibleIndex, c.Width));
            });

            _visibleBands.Clear();
            if (BandedGridView != null)
            {
                BandedGridView.Bands.ForEach(b => _visibleBands.Add(b.Name));
            }
        }
        public void SetGridView(Form containerForm, GridView gridView)
        {
            _gridView = gridView;
            _containerForm = containerForm;

            ToGridView();

            _containerForm.Closing += (sender, args) => { FromGridView(); };
        }

        private string _registryLocation;
        public DXGridViewOptionHolder(Form containerForm, GridView gridView, string registryLocation=null)
        {
            _registryLocation = registryLocation;
            _gridView = gridView;
            _containerForm = containerForm;

            try { FromRegistry(); } catch (Exception) { }
            _containerForm.Closing += (sender, args) => { FromGridView(); };
        }


        private string ConvertToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("MultiSelect={0};", _multiSelect);
            sb.AppendFormat("MultiSelectMode={0};", _multiSelectMode);
            sb.AppendFormat("ShowAutoFilterRow={0};", _showAutoFilterRow);
            sb.AppendFormat("ShowGroupPanel={0};", _showGroupPanel);

            if ( _visibleColumnWidths.Any())
            {
                sb.AppendFormat("VisibleColumns={0};", String.Join(",",
                    _visibleColumnWidths.OrderBy(pr => pr.Value.Key)        // column visible index 순 정렬
                    .Select(pr => String.Format("{0}={1}", pr.Key, pr.Value.Value))));
            }

            if (_visibleBands.Any())
                sb.AppendFormat("VisibleBands={0};", String.Join(",", _visibleBands));

            return sb.ToString();
        }

        public void ToRegistry(string key=null)
        {
            FromGridView();
            key = key ?? _containerForm.Name;
            using(var registryKey = RegistryHelper.OpenSubKey(_registryLocation))      // e.g "Software\UDMTEK\SharpSimulator\Configuration\ViewSetting\"
            {
                registryKey.SetValue(key, ConvertToString());
                registryKey.Close();
            }
        }

        private void FromRegistry(string key = null)
        {
            if (_registryLocation.NonNullAny())
            {
                string contents = null;
                key = key ?? _containerForm.Name;
                using(var registryKey = RegistryHelper.OpenSubKey(_registryLocation))      // e.g "Software\UDMTEK\SharpSimulator\Configuration\ViewSetting\"
                {
                    contents = (string)registryKey.GetValue(key);
                    registryKey.Close();
                }

                if (contents.IsNullOrEmpty())
                    return;

                Dictionary<string, string> map = new Dictionary<string, string>();
                contents.Split(new []{";"}, StringSplitOptions.RemoveEmptyEntries).ForEach(s =>
                {
                    var match = Regex.Match(s, @"([^=]+)=(.*)");
                    if (match.Groups.Count == 3)
                        map.Add(match.Groups[1].Value, match.Groups[2].Value);
                });

                _multiSelect = Boolean.Parse(map["MultiSelect"]);
                _multiSelectMode = (GridMultiSelectMode)Enum.Parse(typeof(GridMultiSelectMode), map["MultiSelectMode"]);
                _showAutoFilterRow = Boolean.Parse(map["ShowAutoFilterRow"]);
                _showGroupPanel = Boolean.Parse(map["ShowGroupPanel"]);

                _visibleColumnWidths.Clear();
                int visibleIndex = 0;
                if (map.ContainsKey("VisibleColumns"))
                {
                    map["VisibleColumns"].Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries).ForEach(cw =>
                    {
                        var match = Regex.Match(cw, @"([^=]+)=(.*)");
                        if (match.Groups.Count == 3)
                        {
                            var pr = new KeyValuePair<int, int>(visibleIndex++, Int32.Parse(match.Groups[2].Value));
                            _visibleColumnWidths.Add(match.Groups[1].Value, pr);
                        }
                    });
                }

                if (map.ContainsKey("VisibleBands"))
                {
                    visibleIndex = 0;
                    map["VisibleBands"].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ForEach(b =>
                    {
                        BandedGridView.Bands.MoveTo(visibleIndex++, BandedGridView.Bands[b]);
                    });
                }

                ToGridView();
            }
        }
    }
}
