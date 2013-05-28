using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NUnit.Extensions.Forms;

namespace Ict.Testing.NUnitForms
{
    /// <summary>
    /// A class derived from the standard ButtonTester.  The only difference is that the button takes the focus before the click event is fired.
    /// This makes it behave more like a real button.
    /// </summary>
    public class ButtonWithFocusTester : ButtonTester
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of button</param>
        public ButtonWithFocusTester(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tester"></param>
        /// <param name="index"></param>
        public ButtonWithFocusTester(ButtonTester tester, int index)
            : base(tester, index)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="form"></param>
        public ButtonWithFocusTester(string name, Form form)
            : base(name, form)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="formName"></param>
        public ButtonWithFocusTester(string name, string formName)
            : base(name, formName)
        {
        }

        /// <summary>
        /// The click event is  fired after the button has been given the focus
        /// </summary>
        public override void Click()
        {
            Properties.Focus();
            base.Click();
        }
    }
}
