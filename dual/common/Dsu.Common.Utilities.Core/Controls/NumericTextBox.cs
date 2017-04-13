//
// http://www.mediafire.com/download/fidd3mm85dmq9tb/%281392.03.10%29+Numerical+TextBox+Sample.rar
// 이거도 참고로 볼 것... 비슷하지만, 현재 것이 나아보임 http://www.codeproject.com/Articles/30812/Simple-Numeric-TextBox
// http://www.thecoolestdolphin.be/?p=38 [Allow only specific characters in textBox in C#]

using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class NumericTextBox : TextBox
    {
        private bool _negative;
        private bool _dot;
        private bool _exponent;
        private int _decimalNumber;
        private int _cursorPositionPlus;
        private char _discriminant;
        private double _maxValue;
        private double _minValue;
        private bool _maxCheck;
        private bool _minCheck;
        private string _oldText;

        public NumericTextBox()
        {
            _decimalNumber = 4;
            _negative = true;
            _dot = true;
            _exponent = true;
            _discriminant = ',';
            _maxValue = 0;
            _minValue = 0;
            _maxCheck = false;
            _minCheck = false;
            _oldText = "";

            _balloonTips = new ToolTip()
            {
                ToolTipTitle = "Invalid character.",
                ToolTipIcon = ToolTipIcon.Warning,
                AutoPopDelay = 0,
                ShowAlways = false,
            };
        }
        public NumericTextBox(int decimalNumber)
            : this()
        {
            _decimalNumber = decimalNumber;
        }
        public NumericTextBox(char discriminant)
            : this(4)
        {
            if (discriminant == '\'' || discriminant == '/' || discriminant == '`')
                _discriminant = discriminant;
            else
                _discriminant = ',';
        }
        public NumericTextBox(int decimalNumber, char discriminant)
            : this(discriminant)
        {
            _decimalNumber = decimalNumber;
        }

        [Description("1000 단위 comma 허용 여부")]
        public int DecimalNumber
        {
            get { return _decimalNumber; }
            set { _decimalNumber = value; OnTextChanged(new EventArgs()); }
        }

        [Description("음수 허용 여부")]
        public bool Negative
        {
            get { return _negative; }
            set { _negative = value; OnTextChanged(new EventArgs()); }
        }
        [Description("Period 허용 여부")]
        public bool Dot
        {
            get { return _dot; }
            set { _dot = value; OnTextChanged(new EventArgs()); }
        }
        [Description("Scientific notation 허용 여부")]
        public bool Exponent
        {
            get { return _exponent; }
            set { _exponent = value; OnTextChanged(new EventArgs()); }
        }
        public char Discriminant
        {
            get { return _discriminant; }
            set 
            {
                if (value == '\'' || value == '/' || value == '`')
                    _discriminant = value;
                else
                    _discriminant = ',';
                OnTextChanged(new EventArgs()); 
            }
        }
        public double MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = (!MinCheck || value >= _minValue) ? value : _maxValue;
                if (_maxCheck && new NumericalString(this.Text) > _maxValue)
                    this.Text = _maxValue.ToString();
            }
        }
        public double MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = (!MaxCheck || value <= _maxValue) ? value : _minValue;
                if (_minCheck && new NumericalString(this.Text) < _minValue)
                    this.Text = _minValue.ToString();
            }
        }
        public bool MaxCheck
        {
            get { return _maxCheck; }
            set 
            { 
                _maxCheck = value;
                if (_maxCheck && new NumericalString(this.Text) > _maxValue)
                    this.Text = _maxValue.ToString();
            }
        }
        public bool MinCheck
        {
            get { return _minCheck; }
            set
            {
                _minCheck = value;
                if (_minCheck && new NumericalString(this.Text) < _minValue)
                    this.Text = _minValue.ToString();
            }
        }
        public NumericalString NumericalText
        {
            get { return new NumericalString(this.Text); }
        }


        public double GetDoubleValue() 
        {
            double value;
            if (!Double.TryParse(NumericalText.Text, out value))
                return 0;
            return value;
        }
        public int GetIntValue() 
        {
            int value;
            if (!Int32.TryParse(NumericalText.Text, out value))
                return 0;
            return value;
        }



        #region Baloon Tips
        [Description("오류 문자 입력시 풍선 도움말 표시 여부")]
        public bool ShowBalloonTips { get { return _showBalloonTips; } set { _showBalloonTips = value; } }
        private bool _showBalloonTips = true;
        private ToolTip _balloonTips;
        #endregion


        protected override void OnTextChanged(EventArgs e)
        {
            _balloonTips.Hide(this);
            _cursorPositionPlus = 0;
            int SelectionStart = this.SelectionStart;
            int TextLength = this.Text.Length;
            int CursorPositionPlus;
            string Text = NormalTextToNumericString();
            CursorPositionPlus = _cursorPositionPlus;
            if ((!_maxCheck || new NumericalString(this.Text) <= _maxValue) && (!_minCheck || new NumericalString(this.Text) >= _minValue))
            {
                this.Text = Text;
                this.SelectionStart = SelectionStart + CursorPositionPlus;
                _oldText = this.Text;
            }
            else
            {
                this.Text = _oldText;
                this.SelectionStart = SelectionStart + _oldText.Length - TextLength;
            }
            base.OnTextChanged(e);
        }
        protected string NormalTextToNumericString()
        {
            string Text = this.Text;
            string TextTemp1 = "", TextTemp2 = "";
            #region Lowering Characters
            for (int i = 0; i < Text.Length; i++)
                TextTemp1 += char.ToLower(Text[i]);
            #endregion
            
            
            #region Remove Unknown Characters
            int FloatNumber = 0;
            for (int i = 0; i < TextTemp1.Length; i++)
                if (_negative && TextTemp1[i] == '-' && i == 0)
                    TextTemp2 += TextTemp1[i];
                else if (TextTemp1[i] == '-' && TextTemp2.IndexOf('e') >= 0 && TextTemp2.Length == TextTemp2.IndexOf('e') + 1)
                    TextTemp2 += TextTemp1[i];
                else if (char.IsDigit(TextTemp1[i]))
                {
                    TextTemp2 += TextTemp1[i];
                    if (TextTemp2.IndexOf('.') > -1 && TextTemp2.IndexOf('e') < 0 && i < this.SelectionStart)
                    {
                        FloatNumber++;
                        if (FloatNumber > _decimalNumber && i < this.SelectionStart)
                            _cursorPositionPlus--;
                    }
                }
                else if (_dot && _decimalNumber > 0 && TextTemp1[i] == '.' && TextTemp2.IndexOf('.') < 0 && (TextTemp2.IndexOf('e') < 0 || TextTemp2.Length < TextTemp2.IndexOf('e')))
                    TextTemp2 += TextTemp1[i];
                else if (_exponent && TextTemp1[i] == 'e' && TextTemp2.IndexOf('e') < 0 && TextTemp2.Length >= TextTemp2.IndexOf('.') + 1)
                    TextTemp2 += TextTemp1[i];
                else if (i < this.SelectionStart)
                {
                    bool skip = _decimalNumber != 0 && TextTemp1[i] == ',';
                    if ( ! skip && ShowBalloonTips )
                        _balloonTips.Show(String.Format("The character \"{0}\" is invalid.", TextTemp1[i]), this);

                    _cursorPositionPlus--;                    
                }
            #endregion





            #region Get Integer Number
            string INTEGER = "";
            int IntegerIndex = (TextTemp2.IndexOf('.') >= 0) ? TextTemp2.IndexOf('.') : (TextTemp2.IndexOf('e') >= 0) ? TextTemp2.IndexOf('e') : TextTemp2.Length;
            for (int i = 0; i < IntegerIndex; i++)
                if (char.IsDigit(TextTemp2[i]) || TextTemp2[i] == '-' && INTEGER.IndexOf('-') < 0)
                    INTEGER += TextTemp2[i];
            #endregion
            #region Get Float Number
            string FLOAT = "";
            if (TextTemp2.IndexOf('.') >= 0)
                for (int i = TextTemp2.IndexOf('.') + 1; i < ((TextTemp2.IndexOf('e') >= 0) ? TextTemp2.IndexOf('e') : TextTemp2.Length); i++)
                    if (char.IsDigit(TextTemp2[i]))
                        FLOAT += TextTemp2[i];
            #endregion
            #region Put '/' Character in Integer Number
            string T = "";
            int n = 0;
            for (int i = INTEGER.Length - 1; i >= 0; i--)
            {
                T += INTEGER[i];
                n++;
                if (n == 3 && i > 0 && INTEGER[i - 1] != '-')
                {
                    if (i - _cursorPositionPlus < this.SelectionStart)
                        _cursorPositionPlus++;
                    T += _discriminant.ToString();
                    n = 0;
                }
            }
            char[] charArray = T.ToCharArray();
            Array.Reverse(charArray);
            T = new string(charArray);
            #endregion
            #region Put '.' Character
            if (TextTemp2.IndexOf('.') >= 0)
            {
                T += ('.').ToString();
                for (int i = 0; i < DecimalNumber && i < FLOAT.Length; i++)
                    T += FLOAT[i];
            }
            #endregion
            #region Put 'e' Character
            if (TextTemp2.IndexOf('e') >= 0)
            {
                T += ('e').ToString();
                for (int i = TextTemp2.IndexOf('e') + 1; i < TextTemp2.Length; i++)
                    T += TextTemp2[i];
            }
            #endregion
            return T;
        }
    }
    public class NumericalString
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        public NumericalString()
        {
            _text = "";
        }
        public NumericalString(string Text)
        {
            string Temp = "";
            for (int i = 0; i < Text.Length; i++)
                if (char.IsDigit(Text[i]) || Text[i] == '-' || Text[i] == '.' || Text[i] == 'e')
                    Temp += Text[i];
            _text = Temp;
        }
        public override string ToString()
        {
            return (this.Text == "") ? "0" : this.Text;
        }
        public static implicit operator NumericalString(string Text)
        {
            return new NumericalString(Text); 
        }
        public static explicit operator string(NumericalString Text)
        {
            return (Text.Text == "") ? "0" : Text.Text;
        }
        public static implicit operator double(NumericalString Text)
        {
            double value;
            if (Text.Text == "")
                return 0;
            if (Text.Text == "-")
                return 0;
            if (Text.Text == "-.")
                return 0;
            if (Text.Text.StartsWith("-e"))
                return 0;
            if (Text.Text.StartsWith("e"))
                return 0;
            if (Text.Text.EndsWith("e") || Text.Text.EndsWith("e-"))
                return Convert.ToDouble(Text.Text.Substring(0, Text.Text.IndexOf('e')));
            if (!Double.TryParse(Text.Text, out value))
                return Convert.ToDouble(Regex.Replace(Text.Text, @"\D", ""));

            return Convert.ToDouble(Text.Text);
        }
        public static string operator +(NumericalString Text1, NumericalString Text2)
        {
            return Text1.Text + Text2.Text;
        }
        public static string operator +(NumericalString Text1, string Text2)
        {
            return Text1.Text + Text2;
        }
        public static string operator +(NumericalString Text1, char ch)
        {
            return Text1.Text + ch.ToString();
        }
    }
}


/*

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            numericalTextBox.DecimalNumber = (int)DecimalNumberNumericUpDown.Value;
            numericalTextBox.Negative = NegativeSignCheckBox.Checked;
            numericalTextBox.Dot = DotCheckBox.Checked;
            numericalTextBox.Exponent = ExponentCheckBox.Checked;
            numericalTextBox.MaxValue = System.Convert.ToDouble(MaximumTextBox.Text);
            numericalTextBox.MinValue = System.Convert.ToDouble(MinimumTextBox.Text);
            numericalTextBox.MaxCheck = MaximumCheckBox.Checked;
            numericalTextBox.MinCheck = MinimumCheckBox.Checked;
            numericalTextBox.Discriminant = ',';
        }
        private void DecimalNumberNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            numericalTextBox.DecimalNumber = (int)DecimalNumberNumericUpDown.Value;
        }
        private void NegativeSignCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            numericalTextBox.Negative = NegativeSignCheckBox.Checked;
        }
        private void DotCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            numericalTextBox.Dot = DotCheckBox.Checked;
        }
        private void ExponentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            numericalTextBox.Exponent = ExponentCheckBox.Checked;
        }
        private void MaximumTextBox_TextChanged(object sender, EventArgs e)
        {
            numericalTextBox.MaxValue = MaximumTextBox.NumericalText;
        }
        private void MinimumTextBox_TextChanged(object sender, EventArgs e)
        {
            numericalTextBox.MinValue = MinimumTextBox.NumericalText;
        }
        private void MaximumCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            numericalTextBox.MaxCheck = MaximumCheckBox.Checked;
            MaximumTextBox.Enabled = MaximumCheckBox.Checked;
        }
        private void MinimumCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            numericalTextBox.MinCheck = MinimumCheckBox.Checked;
            MinimumTextBox.Enabled = MinimumCheckBox.Checked;
        }
        private void GroupSeparatorCharacterTextBox_TextChanged(object sender, EventArgs e)
        {
            if (GroupSeparatorCharacterTextBox.Text != "" && GroupSeparatorCharacterTextBox.Text.Length == 1)
                numericalTextBox.Discriminant = System.Convert.ToChar(GroupSeparatorCharacterTextBox.Text);
            else
                numericalTextBox.Discriminant = ',';
        }
        private void numericalTextBox_TextChanged(object sender, EventArgs e)
        {
            NumericalString NS = "Reza";
            string s = numericalTextBox.NumericalText.ToString();
            TextTextBox.Text = (string)numericalTextBox.NumericalText + " Reza";
            DoubleTextBox.Text = (numericalTextBox.NumericalText + 3).ToString();
            ConditionTextBox.Text = (numericalTextBox.NumericalText < 100) ? (string)numericalTextBox.NumericalText : "Over 100";
        }
    }
*/
