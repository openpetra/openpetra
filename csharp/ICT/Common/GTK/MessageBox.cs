/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using Gtk;
using System;
using Mono.Unix;

// this namespace is named GTK in capital letters on purpose,
// so that we are still able to refer to Gtk.Window etc
namespace Ict.Common.GTK
{
    /// <summary>
    /// to make usage similar to Winforms Messagebox
    /// </summary>
    public enum MessageBoxButtons
    {
        /// <summary>
        /// ok button
        /// </summary>
        OK,

        /// <summary>
        /// close button
        /// </summary>
        Close
    };

    /// <summary>
    /// to make usage similar to Winforms Messagebox
    /// </summary>
    public enum MessageBoxIcon
    {
        /// <summary>
        /// Stop, same to error
        /// </summary>
        Stop,

        /// <summary>
        /// error
        /// </summary>
        Error,

        /// <summary>
        /// information
        /// </summary>
        Information
    };

    /// <summary>
    /// some wrappers around MessageDialog to provide easy to use Message boxes
    /// </summary>
    public class MessageBox
    {
        /// <summary>
        /// a GTK messagebox, using the names for buttons and icon from Windows.Forms
        /// </summary>
        /// <param name="AMessage"></param>
        /// <param name="ATitle"></param>
        /// <param name="AButtons"></param>
        /// <param name="AIcon"></param>
        public static void Show(String AMessage, String ATitle, MessageBoxButtons AButtons, MessageBoxIcon AIcon)
        {
            Gtk.ButtonsType buttons = ButtonsType.Ok;

            if (AButtons == MessageBoxButtons.OK)
            {
                buttons = ButtonsType.Ok;
            }
            else if (AButtons == MessageBoxButtons.Close)
            {
                buttons = ButtonsType.Close;
            }

            Gtk.MessageType mtype = MessageType.Info;

            if (AIcon == MessageBoxIcon.Error)
            {
                mtype = MessageType.Error;
            }
            else if (AIcon == MessageBoxIcon.Information)
            {
                mtype = MessageType.Info;
            }
            else if (AIcon == MessageBoxIcon.Stop)
            {
                mtype = MessageType.Error;
            }

            MessageDialog md = new MessageDialog(null,
                DialogFlags.DestroyWithParent,
                mtype, buttons, AMessage);
            md.Title = ATitle;
            md.Run();
            md.Destroy();
        }

        /// <summary>
        /// overload, just shows an error message box with Ok button, and window title Error
        /// </summary>
        /// <param name="msg"></param>
        public static void Show(String msg)
        {
            Show(msg, Catalog.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// simple Message box; with info icon
        /// </summary>
        /// <param name="msg">the message to display</param>
        /// <param name="title">title of the message box</param>
        public static void Show(String msg, String title)
        {
            Show(msg, title, MessageBoxButtons.Close, MessageBoxIcon.Information);
        }
    }
}