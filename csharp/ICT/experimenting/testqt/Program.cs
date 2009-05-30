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
using System;
using Qyoto;
using iisTaskPanelNamespace;

/// see http://websvn.kde.org/trunk/KDE/kdebindings/csharp/qyoto/examples/tutorial/t4/t4.cs
public class MyWindow : QMainWindow
{
    public MyWindow() : this(null)
    {
    }

    public MyWindow(QWidget parent) : base(parent)
    {
        SetFixedSize(700, 620);

        QPushButton quit = new QPushButton(Tr("Quit"), this);
        quit.SetGeometry(62, 40, 75, 30);
        quit.Font = new QFont("Times", 18, (int)QFont.Weight.Bold);

        Connect(quit, SIGNAL("clicked()"), qApp, SLOT("quit()"));

        iisTaskBox tb1 = new iisTaskBox(new QPixmap(":/images/win/filenew.png"), "Group of Tasks", true, this);

//      iisIconLabel i1 = new iisIconLabel(new QPixmap(":/images/win/zoomin.png"), "Do Task 1", tb1);
//	  tb1.AddIconLabel(i1, true);

        // TODO: connect(i1, SIGNAL(activated()), this, SLOT(task1()));

//      iisIconLabel i2 = new iisIconLabel(new QPixmap(":/images/win/zoomout.png"), "Do Task 2", tb1);
//	  tb1.AddIconLabel(i2, true);
    }

    public static int Main(String[] args)
    {
        new QApplication(args);

//		try
        {
            MyWindow w = new MyWindow();
            w.Show();
            return QApplication.Exec();
        }

/*		catch (Exception e)
 *              {
 *                QMessageBox.Warning(null, "Exception", e.Message);
 *                return 0;
 *              }
 */
    }
}