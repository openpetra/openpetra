using System.Windows.Forms;

namespace Ict.Common.Controls
{
    /// <summary>
    /// This is just an ordinary checkbox, but unlike the standard .NET one, 
    /// the user can see when it has focus.
    /// </summary>
    public class TchkVisibleFocus : System.Windows.Forms.CheckBox
    {

        /// <summary>
        /// This is just an ordinary checkbox, but unlike the standard .NET one, 
        /// the user can see when it has focus.
        /// </summary>
        public TchkVisibleFocus():base()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.GotFocus += TchkClearFocus_GotFocus;
            this.LostFocus += TchkClearFocus_LostFocus;
        }

        private void TchkClearFocus_GotFocus(object sender, System.EventArgs e)
        {
            this.FlatStyle = FlatStyle.Standard;
        }

        private void TchkClearFocus_LostFocus(object sender, System.EventArgs e)
        {
            this.FlatStyle = FlatStyle.Flat;
        }
    }
}