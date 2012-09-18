//using System;

//namespace DevAge.Windows.Forms
//{
//    /// <summary>
//    /// A class to send keystrokes to the active application using System.Windows.Forms.SendKeys.Send method. The characters sended are exactly the same used in the parameters, converting the special characters like upper case letters and \n, \t and similar characters with the corresponding send keys codes.
//    /// </summary>
//    public class SendCharExact
//    {
//        //Characters code map as defined in MSDN SendKey method
////			Key 	Code
////			BACKSPACE 		{BACKSPACE}, {BS}, or {BKSP}
////			BREAK 			{BREAK}
////			CAPS LOCK 		{CAPSLOCK}
////			DEL or DELETE 	{DELETE} or {DEL}
////			DOWN ARROW 		{DOWN}
////			END 			{END}
////			ENTER 			{ENTER}or ~
////			ESC 			{ESC}
////			HELP 			{HELP}
////			HOME 			{HOME}
////			INS or INSERT 	{INSERT} or {INS}
////			LEFT ARROW 		{LEFT}
////			NUM LOCK 		{NUMLOCK}
////			PAGE DOWN 		{PGDN}
////			PAGE UP 		{PGUP}
////			PRINT SCREEN 	{PRTSC} (reserved for future use)
////			RIGHT ARROW 	{RIGHT}
////			SCROLL LOCK 	{SCROLLLOCK}
////			TAB 			{TAB}
////			UP ARROW 		{UP}
////			F1 				{F1}
////			F2				{F2}
////			F3				{F3}
////			F4				{F4}
////			F5				{F5}
////			F6				{F6}
////			F7 				{F7}
////			F8			 	{F8}
////			F9			 	{F9}
////			F10 			{F10}
////			F11 			{F11}
////			F12 			{F12}
////			F13 			{F13}
////			F14				{F14}
////			F15				{F15}
////			F16				{F16}
////			Keypad add		{ADD}
////			Keypad subtract	{SUBTRACT}
////			Keypad multiply	{MULTIPLY}
////			Keypad divide	{DIVIDE}
////
////			To specify keys combined with any combination of the SHIFT, CTRL, and ALT keys, precede the key code with one or more of the following codes.
////			Key 	Code
////			SHIFT			+
////			CTRL 			^
////			ALT 			%

//        /// <summary>
//        /// Send keystrokes to the active application using System.Windows.Forms.SendKeys.Send method. The characters sended are exactly the same used in the parameters, converting the special characters like upper case letters and \n, \t and similar characters with the corresponding send keys codes.
//        /// Here some examples:
//        /// s -> s
//        /// S -> +s
//        /// + -> {+}
//        /// \n -> {ENTER}
//        /// </summary>
//        /// <param name="key"></param>
//        public static void Send(char key)
//        {
//            string s;
//            if ( char.IsUpper(key) && 
//                (System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Shift) != System.Windows.Forms.Keys.Shift)
//                s = "{CAPSLOCK}" + key;
//            else if ( key == '+')
//                s = "{" + key + "}";
//            else if ( key == ']') //NOTE: Seems that in same cases this charecters are not allowed if without {}
//                s = "{" + key + "}";
//            else if ( key == '[') //NOTE: Seems that in same cases this charecters are not allowed if without {}
//                s = "{" + key + "}";
//            else if ( key == '{')
//                s = "{" + key + "}";
//            else if ( key == '}')
//                s = "{" + key + "}";
//            else if ( key == '^') //NOTE: Seems to be a .NET bug that when using {^} the character printed is &
//                s = "{" + key + "}";
//            else if ( key == '%')
//                s = "{" + key + "}";
//            else if ( key == '~')
//                s = "{" + key + "}";
//            else if ( key == '(')
//                s = "{" + key + "}";
//            else if ( key == ')')
//                s = "{" + key + "}";
//            else
//            {
//                s = "";
//                s += key;
//            }

//            System.Windows.Forms.SendKeys.Send(s);
//        }

//        //public static void Send(char key)
//        //{
//        //    string keyString = GetSingleChar(key);

//        //    System.Diagnostics.Debug.WriteLine("SendKeys: " + keyString);

//        //    System.Windows.Forms.SendKeys.Send(keyString);
//        //}

//        //private static string GetSingleChar(char key)
//        //{
//        //    char[] specials = new char[] { '+', ']', '[', '{', '}', '^', '%', '~', '(', ')' };

//        //    for (int i = 0; i < specials.Length; i++)
//        //        if (specials[i] == key)
//        //            return "{" + key + "}";

//        //    return key.ToString();
//        //}

//    }
//}
