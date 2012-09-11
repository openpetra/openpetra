using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevAge.Windows.Forms
{
    /// <summary>
    /// Used for subscript and superscript
    /// </summary>
    public enum EffectType
    {
        Normal,
        Subscript,
        Superscript,
    }

    /// <summary>
    /// Specifies the style of underline that should be
    /// applied to the text.
    /// </summary>
    public enum UnderlineStyle
    {
        /// <summary>
        /// No underlining.
        /// </summary>
        None = 0,

        /// <summary>
        /// Standard underlining across all words.
        /// </summary>
        Normal = 1,

        /// <summary>
        /// Standard underlining broken between words.
        /// </summary>
        Word = 2,

        /// <summary>
        /// Double line underlining.
        /// </summary>
        Double = 3,

        /// <summary>
        /// Dotted underlining.
        /// </summary>
        Dotted = 4,

        /// <summary>
        /// Dashed underlining.
        /// </summary>
        Dash = 5,

        /// <summary>
        /// Dash-dot ("-.-.") underlining.
        /// </summary>
        DashDot = 6,

        /// <summary>
        /// Dash-dot-dot ("-..-..") underlining.
        /// </summary>
        DashDotDot = 7,

        /// <summary>
        /// Wave underlining (like spelling mistakes in MS Word).
        /// </summary>
        Wave = 8,

        /// <summary>
        /// Extra thick standard underlining.
        /// </summary>
        Thick = 9,

        /// <summary>
        /// Extra thin standard underlining.
        /// </summary>
        HairLine = 10,

        /// <summary>
        /// Double thickness wave underlining.
        /// </summary>
        DoubleWave = 11,

        /// <summary>
        /// Thick wave underlining.
        /// </summary>
        HeavyWave = 12,

        /// <summary>
        /// Extra long dash underlining.
        /// </summary>
        LongDash = 13
    }

    /// <summary>
    /// Specifies the color of underline that should be
    /// applied to the text.
    /// </summary>
    public enum UnderlineColor
    {
        /// <summary>Black.</summary>
        Black = 0x00,

        /// <summary>Blue.</summary>
        Blue = 0x10,

        /// <summary>Cyan.</summary>
        Cyan = 0x20,

        /// <summary>Lime green.</summary>
        LimeGreen = 0x30,

        /// <summary>Magenta.</summary>
        Magenta = 0x40,

        /// <summary>Red.</summary>
        Red = 0x50,

        /// <summary>Yellow.</summary>
        Yellow = 0x60,

        /// <summary>White.</summary>
        White = 0x70,

        /// <summary>DarkBlue.</summary>
        DarkBlue = 0x80,

        /// <summary>DarkCyan.</summary>
        DarkCyan = 0x90,

        /// <summary>Green.</summary>
        Green = 0xA0,

        /// <summary>Dark magenta.</summary>
        DarkMagenta = 0xB0,

        /// <summary>Brown.</summary>
        Brown = 0xC0,

        /// <summary>Olive green.</summary>
        OliveGreen = 0xD0,

        /// <summary>Dark gray.</summary>
        DarkGray = 0xE0,

        /// <summary>Gray.</summary>
        Gray = 0xF0
    }

    /// <summary>
    /// Class which contains a rich text string.
    /// Used to distinguish between string and rich text
    /// as normally rich text is also a string.
    /// </summary>
    [Serializable]
    public class RichText : IComparable, IComparable<RichText>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rtf"></param>
        public RichText(String rtf)
        {
            m_Rtf = rtf;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public RichText(RichText other)
        {
            Rtf = other.Rtf;
        }

        #endregion

        #region Members

        /// <summary>
        /// RichText
        /// </summary>
        private String m_Rtf = String.Empty;
        public String Rtf
        {
            get
            {
                return m_Rtf;
            }
            set
            {
                m_Rtf = value;
            }
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// Compare this instance with a specified RichText object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            RichText rtf = obj as RichText;
            return CompareTo(rtf);
        }

        #endregion

        #region IComparable<RichText> Members

        /// <summary>
        /// Compare this instance with a specified RichText.
        /// </summary>
        /// <returns></returns>
        public int CompareTo(RichText other)
        {
            // convert rich text first to plain text and compare that string
            String txt = RichTextConversion.RichTextToString(this);
            String otherTxt = RichTextConversion.RichTextToString(other);
            return txt.CompareTo(otherTxt);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Return richtext as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Rtf;
        }

        #endregion
    }

    /// <summary>
    /// RichText conversion methods
    /// </summary>
    public static class RichTextConversion
    {
        /// <summary>
        /// Convert plain text to rtf
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static DevAge.Windows.Forms.RichText StringToRichText(String txt)
        {
            return StringToRichText(txt, FontStyle.Regular);
        }

        /// <summary>
        /// Convert plain text to rtf with font style
        /// </summary>
        /// <returns></returns>
        public static DevAge.Windows.Forms.RichText StringToRichText(String txt, FontStyle fontStyle)
        {
            String rtf = String.Empty;

            // dont use the static richtextbox member, because the richtextbox class has a problem
            // if a value is assigned multiple times. so use a local member
            RichTextBox richTextBox = new RichTextBox();
            try
            {
                richTextBox.Text = txt;
                if (fontStyle != FontStyle.Regular)
                {
                    richTextBox.Font = new Font(richTextBox.Font, fontStyle);
                }
                rtf = richTextBox.Rtf;
            }
            catch (Exception)
            {

                // text is not convertable
                // return empty rtf string
                richTextBox.Text = String.Empty;
                rtf = richTextBox.Rtf;
            }
            richTextBox.Dispose();

            return new DevAge.Windows.Forms.RichText(rtf);
        }

        /// <summary>
        /// Convert rtf to plain text
        /// </summary>
        /// <returns></returns>
        public static String RichTextToString(DevAge.Windows.Forms.RichText rtf)
        {

            String txt = String.Empty;

            RichTextBox richTextBox = new RichTextBox();

            try
            {
                richTextBox.Rtf = rtf.Rtf;
                txt = richTextBox.Text;
            }
            catch (Exception)
            {
                // rtf is not convertable
                // return empty text string
            }
            richTextBox.Dispose();

            return txt;
        }

        public static String RichTextToStringStripWhitespaces(DevAge.Windows.Forms.RichText rtf)
        {
            return System.Text.RegularExpressions.Regex.Replace(RichTextToString(rtf), @"[\t\n\r\f\v]", string.Empty);
        }
    }

    /// <summary>
    /// A RichTextBox that allows to set the type of value to edit,
    /// then you can use the Value property to read and write the specific type.
    /// Furthermore, it is possible to format single characters.
    /// </summary>
    public class DevAgeRichTextBox : System.Windows.Forms.RichTextBox
    {
        #region Win32 API

        protected const int CFM_BOLD = 1;
        protected const int CFM_ITALIC = 2;
        protected const int CFM_UNDERLINE = 4;
        [CLSCompliant(false)]
        protected const uint CFM_FACE = 0x20000000;
        [CLSCompliant(false)]
        protected const uint CFM_SIZE = 0x80000000;
        [CLSCompliant(false)]
        protected const uint CFM_SUPERSCRIPT = 0x00030000;
        [CLSCompliant(false)]
        protected const uint CFE_SUPERSCRIPT = 0x00020000;
        [CLSCompliant(false)]
        protected const uint CFM_SUBSCRIPT = 0x00030000;
        [CLSCompliant(false)]
        protected const uint CFE_SUBSCRIPT = 0x00010000;
        protected const int CFM_UNDERLINETYPE = 8388608;
        protected const int EM_SETCHARFORMAT = 1092;
        protected const int EM_GETCHARFORMAT = 1082;
        protected const int SCF_SELECTION = 1;
        protected const int EM_FORMATRANGE = 1081;
        protected const int WM_USER = 0x0400;
        protected const int EM_SETEVENTMASK = 1073;
        protected const int EM_GETPARAFORMAT = 1085;
        protected const int EM_SETPARAFORMAT = 1095;
        protected const int EM_SETTYPOGRAPHYOPTIONS = 1226;
        protected const int WM_SETREDRAW = 11;
        protected const int TO_ADVANCEDTYPOGRAPHY = 1;

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(HandleRef hWnd, int msg,
            int wParam, int lParam);

        [StructLayout(LayoutKind.Sequential)]
        protected struct CHARFORMAT
        {
            public int cbSize;
            [CLSCompliant(false)]
            public uint dwMask;
            [CLSCompliant(false)]
            public uint dwEffects;
            public int yHeight;
            public int yOffset;
            public int crTextColor;
            public byte bCharSet;
            public byte bPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szFaceName;

            // CHARFORMAT2 from here onwards.
            public short wWeight;
            public short sSpacing;
            public int crBackColor;
            public int LCID;
            [CLSCompliant(false)]
            public uint dwReserved;
            public short sStyle;
            public short wKerning;
            public byte bUnderlineType;
            public byte bAnimation;
            public byte bRevAuthor;
        }

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(HandleRef hWnd,
                int msg, int wParam, ref CHARFORMAT lp);

        protected void SetCharFormatMessage(ref CHARFORMAT fmt)
        {
            SendMessage(new HandleRef(this, Handle),
                  EM_SETCHARFORMAT, SCF_SELECTION, ref fmt);
        }


        /// <summary>
        /// Gets or sets the underline style to apply to the
        /// current selection or insertion point.
        /// </summary>
        /// <remarks>
        /// Underline styles can be set to any value of the
        /// <see cref="UnderlineStyle"/> enumeration.
        /// </remarks>
        public UnderlineStyle SelectionUnderlineStyle
        {
            get
            {
                CHARFORMAT fmt = new CHARFORMAT();
                fmt.cbSize = Marshal.SizeOf(fmt);

                // Get the underline style.
                SendMessage(new HandleRef(this, Handle), EM_GETCHARFORMAT,
                             SCF_SELECTION, ref fmt);

                // Default to no underline.
                if ((fmt.dwMask & CFM_UNDERLINETYPE) == 0)
                    return UnderlineStyle.None;

                byte type = (byte)(fmt.bUnderlineType & 0x0F);

                return (UnderlineStyle)type;
            }

            set
            {
                // Ensure we don't alter the color by accident.
                UnderlineColor color = SelectionUnderlineColor;

                // Ensure we don't show it if it shouldn't be shown.
                if (value == UnderlineStyle.None)
                    color = UnderlineColor.Black;

                CHARFORMAT fmt = new CHARFORMAT();
                fmt.cbSize = Marshal.SizeOf(fmt);
                fmt.dwMask = CFM_UNDERLINETYPE;
                fmt.bUnderlineType = (byte)((byte)value | (byte)color);

                // Set the underline type.
                SendMessage(new HandleRef(this, Handle), EM_SETCHARFORMAT,
                             SCF_SELECTION, ref fmt);
            }
        }

        /// <summary>
        /// Gets or sets the underline color to apply to the
        /// current selection or insertion point.
        /// </summary>
        /// <remarks>
        /// Underline colors can be set to any value of the
        /// <see cref="UnderlineColor"/> enumeration.
        /// </remarks>
        public UnderlineColor SelectionUnderlineColor
        {
            get
            {
                CHARFORMAT fmt = new CHARFORMAT();
                fmt.cbSize = Marshal.SizeOf(fmt);

                // Get the underline color.
                SendMessage(new HandleRef(this, Handle), EM_GETCHARFORMAT,
                             SCF_SELECTION, ref fmt);

                // Default to black.
                if ((fmt.dwMask & CFM_UNDERLINETYPE) == 0)
                    return UnderlineColor.Black;

                byte style = (byte)(fmt.bUnderlineType & 0xF0);

                return (UnderlineColor)style;
            }

            set
            {
                // Ensure we don't alter the style.
                UnderlineStyle style = SelectionUnderlineStyle;

                // Ensure we don't show it if it shouldn't be shown.
                if (style == UnderlineStyle.None)
                    value = UnderlineColor.Black;

                CHARFORMAT fmt = new CHARFORMAT();
                fmt.cbSize = Marshal.SizeOf(fmt);
                fmt.dwMask = CFM_UNDERLINETYPE;
                fmt.bUnderlineType = (byte)((byte)style | (byte)value);

                // Set the underline color.
                SendMessage(new HandleRef(this, Handle), EM_SETCHARFORMAT,
                             SCF_SELECTION, ref fmt);
            }
        }

        /// <summary>
        /// Set the effect of the selected text
        /// </summary>
        public EffectType SelectionEffect
        {
            set
            {
                switch (value)
                {
                    case EffectType.Subscript:
                        SetSelectionSub();
                        break;

                    case EffectType.Superscript:
                        SetSelectionSuper();
                        break;

                    default:
                        SetSelectionNormal();
                        break;
                }
            }
        }

        /// <summary>
        /// Set the selection to superscript
        /// </summary>
        private void SetSelectionSuper()
        {
            CHARFORMAT fmt = new CHARFORMAT();
            fmt.cbSize = Marshal.SizeOf(fmt);
            fmt.dwMask = CFM_SUPERSCRIPT;

            fmt.dwEffects |= CFE_SUPERSCRIPT;
            SetCharFormatMessage(ref fmt);
        }

        /// <summary>
        /// Set the selection to subscript
        /// </summary>
        private void SetSelectionSub()
        {
            CHARFORMAT fmt = new CHARFORMAT();
            fmt.cbSize = Marshal.SizeOf(fmt);
            fmt.dwMask = CFM_SUBSCRIPT;

            fmt.dwEffects |= CFE_SUBSCRIPT;
            SetCharFormatMessage(ref fmt);
        }

        /// <summary>
        /// Set the selection to normal
        /// </summary>
        private void SetSelectionNormal()
        {
            CHARFORMAT fmt = new CHARFORMAT();
            fmt.cbSize = Marshal.SizeOf(fmt);
            fmt.dwMask = CFM_SUBSCRIPT | CFM_SUPERSCRIPT;

            fmt.dwEffects &= ~CFE_SUPERSCRIPT;
            fmt.dwEffects &= ~CFE_SUBSCRIPT;
            SetCharFormatMessage(ref fmt);
        }



        #endregion

        #region Generic validation methods
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            DevAge.Windows.Forms.RichText val;
            if (IsValidValue(out val) == false)
            {
                e.Cancel = true;
            }
        }

        private DevAge.ComponentModel.Validator.IValidator m_Validator = null;
        /// <summary>
        /// Gets or sets the Validator class useded to validate the value
        /// and convert the text when using the Value property.
        /// You can use the ApplyValidatorRules method to apply the settings of
        /// the Validator directly to the ComboBox, for example the list of values.
        /// </summary>
        [DefaultValue(null)]
        public DevAge.ComponentModel.Validator.IValidator Validator
        {
            get { return m_Validator; }
            set
            {
                if (m_Validator != value)
                {
                    if (m_Validator != null)
                        m_Validator.Changed -= m_Validator_Changed;

                    m_Validator = value;
                    m_Validator.Changed += m_Validator_Changed;
                    ApplyValidatorRules();
                }
            }
        }

        void m_Validator_Changed(object sender, EventArgs e)
        {
            ApplyValidatorRules();
        }

        /// <summary>
        /// Apply the current Validator rules. This method is automatically
        /// fired when the Validator changed.
        /// </summary>
        protected virtual void ApplyValidatorRules()
        {

        }

        /// <summary>
        /// Check if the selected value is valid based on the
        /// current validator and returns the value.
        /// </summary>
        /// <param name="convertedValue"></param>
        /// <returns></returns>
        public bool IsValidValue(out DevAge.Windows.Forms.RichText convertedValue)
        {
            if (Validator != null)
            {
                object convertedRichTextValue = null;
                bool success = false;
                if (Validator.IsValidObject(new RichText(this.Rtf), out convertedRichTextValue))
                {
                    success = true;
                }

                convertedValue = convertedRichTextValue as DevAge.Windows.Forms.RichText;
                return success;
            }
            else
            {
                convertedValue = new RichText(this.Rtf);
                return true;
            }
        }

        /// <summary>
        /// Gets or sets the typed value for the control, using the Validator class.
        /// If the Validator is null the Text property is used.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DevAge.Windows.Forms.RichText Value
        {
            get
            {
                DevAge.Windows.Forms.RichText val;
                if (IsValidValue(out val))
                    return val;
                else
                    throw new ArgumentOutOfRangeException("Text");
            }
            set
            {
                if (Validator != null)
                {
                    DevAge.Windows.Forms.RichText richText = (DevAge.Windows.Forms.RichText)
                        Validator.ValueToObject(value, typeof(DevAge.Windows.Forms.RichText));

                    if (richText == null)
                        Text = String.Empty;
                    else
                        Rtf = richText.Rtf;
                }
                else
                {
                    if (value == null)
                        Text = String.Empty;
                    else
                        Text = value.ToString();
                }
            }
        }

        #endregion

        private int updating = 0;
        private int oldEventMask = 0;

        /// <summary>
        /// Maintains performance while updating.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is recommended to call this method before doing
        /// any major updates that you do not wish the user to
        /// see. Remember to call EndUpdate when you are finished
        /// with the update. Nested calls are supported.
        /// </para>
        /// <para>
        /// Calling this method will prevent redrawing. It will
        /// also setup the event mask of the underlying richedit
        /// control so that no events are sent.
        /// </para>
        /// </remarks>
        public void BeginUpdate()
        {
            // Deal with nested calls.
            ++updating;

            if (updating > 1)
                return;

            // Prevent the control from raising any events.
            oldEventMask = SendMessage(new HandleRef(this, Handle),
                EM_SETEVENTMASK, 0, 0);

            // Prevent the control from redrawing itself.
            SendMessage(new HandleRef(this, Handle),
                WM_SETREDRAW, 0, 0);
        }

        /// <summary>
        /// Resumes drawing and event handling.
        /// </summary>
        /// <remarks>
        /// This method should be called every time a call is made
        /// made to BeginUpdate. It resets the event mask to it's
        /// original value and enables redrawing of the control.
        /// </remarks>
        public void EndUpdate()
        {
            // Deal with nested calls.
            --updating;

            if (updating > 0)
                return;

            // Allow the control to redraw itself.
            SendMessage(new HandleRef(this, Handle),
                WM_SETREDRAW, 1, 0);

            // Allow the control to raise event messages.
            SendMessage(new HandleRef(this, Handle),
                EM_SETEVENTMASK, 0, oldEventMask);

            // is needed, as otherwise last action has not been performed
            Refresh();
        }

        /// <summary>
        /// Returns true when the control is performing some 
        /// internal updates
        /// </summary>
        public bool InternalUpdating
        {
            get
            {
                return (updating != 0);
            }
        }

        /// <summary>
        /// Append a text with the specified fontstyle to the rich text box
        /// </summary>
        /// <param name="text">plain text to append</param>
        /// <param name="fontStyle">the fontstyle for the appended text</param>
        public void AppendText(string text, FontStyle fontStyle)
        {
            int startPosition = this.Text.Length;
            this.AppendText(text);
            this.Select(startPosition, text.Length);
            this.SelectionFont = new Font(this.Font, fontStyle);
        }

        /// <summary>
        /// Remove all formatting, optionally with tabs and line breaks
        /// </summary>
        /// <param name="withWhitespaces">true with tabs and line breaks</param>
        public void RemoveFormats(bool withWhitespaces)
        {
            string rtfText = Rtf;
            Text = string.Empty;

            if (withWhitespaces)
            {
                Text = RichTextConversion.RichTextToStringStripWhitespaces(new RichText(rtfText));
            }
            else
            {
                Text = RichTextConversion.RichTextToString(new RichText(rtfText));
            }
        }

        /// <summary>
        /// Measure the content of the text box
        /// </summary>
        /// <returns></returns>
        public System.Drawing.SizeF MeasureTextBoxContent(Font font)
        {
            System.Drawing.Graphics e = CreateGraphics();
            int charactersFitted;
            int linesFilled;
            System.Drawing.SizeF mySize = e.MeasureString(Text, font, new SizeF(this.Width, 400.0F), new StringFormat(), out charactersFitted, out linesFilled);
            return mySize;
        }
    }
}
