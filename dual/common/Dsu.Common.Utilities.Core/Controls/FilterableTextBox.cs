using System;
using System.Linq;
using System.Windows.Forms;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// see http://www.thecoolestdolphin.be/?p=38
    /// TextBox 에서 invalid 문자를 filtering 함
    /// sister classes : Dsu.Common.Utilities.NumericTextBox
    /// </summary>
    public class FilterableTextBox : TextBox
    {
        /// <summary>
        /// 허용 문자 set.
        /// <para/> - empty 이면, ForbiddenCharacterSet 이외의 모든 문자 허용
        /// <para/> - empty 가 아니면 ForbiddenCharacterSet 는 의미가 없음
        /// </summary>
        public string AllowedCharacterSet { get; set; }

        /// <summary>
        /// 금칙 문자 set.  AllowedCharacterSet 가 empty 인 경우에만 의미를 가짐
        /// </summary>
        public string ForbiddenCharacterSet { get; set; }

        public bool ShowBalloonTips { get { return _showBalloonTips; } set { _showBalloonTips = value; }}
        private bool _showBalloonTips = true;
        private ToolTip _balloonTips;

        public FilterableTextBox()
        {
            // default 는 alpha numeric 만 받음.   필요시 override.
            SetAlphaNumericFilter();

            _balloonTips = new ToolTip()
            {
                ToolTipTitle = "Invalid character.",
                ToolTipIcon = ToolTipIcon.Warning,
                AutoPopDelay = 0,
                ShowAlways = false,
            };
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            _balloonTips.Hide(this);

            if ( Char.IsControl(e.KeyChar) || e.KeyChar == 46 )     // 8==backspace, 13=enter, 46=DEL
                return;

            bool valid = true;
            if (String.IsNullOrEmpty(AllowedCharacterSet)) // 허용 문자를 지정하지 않은 경우.  금칙 문자에 포함되지 않으면 OK
            {
                if (!String.IsNullOrEmpty(ForbiddenCharacterSet) && ForbiddenCharacterSet.Contains(e.KeyChar))
                    valid = false;
            }
            else // 허용 문자를 지정한 경우
                valid = AllowedCharacterSet.Contains(e.KeyChar);

            
            e.Handled = ! valid;

            if ( ! valid && _showBalloonTips)
                _balloonTips.Show(String.Format("The character \"{0}\" is invalid.", e.KeyChar), this);
        }



        /// <summary>
        /// Alpha-numeric 문자열 + addition 이 아니면 filtering 한다.
        /// </summary>
        /// <param name="addition"></param>
        public void SetAlphaNumericFilter(string addition="")
        {
            AllowedCharacterSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" + addition;
            ForbiddenCharacterSet = String.Empty;
        }

        public void SetFileNameFilter()
        {
            AllowedCharacterSet = String.Empty;
            ForbiddenCharacterSet = @"\/:*?""<>|";
        }

        public void SetSymbolFilter(string addition="")
        {
            SetAlphaNumericFilter("@_?" + addition);
        }
    }
}
