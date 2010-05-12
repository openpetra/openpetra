//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       >>>> Put your full name or just a shortname here <<<<
//
// Copyright 2004-2010 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System.Collections.Generic;
using Qyoto;

namespace iisTaskPanelNamespace
{
public class iisTaskPanel : QWidget
{
    iisTaskPanelScheme myScheme;

    public iisTaskPanel(QWidget parent) :
        base(parent)
    {
        myScheme = iisTaskPanelScheme.defaultScheme(this);

        QVBoxLayout vbl = new QVBoxLayout();
        vbl.Margin = 8;
        vbl.Spacing = 8;
        SetLayout(vbl);
    }

    void setScheme(iisTaskPanelScheme scheme)
    {
        if (scheme != null)
        {
            myScheme = scheme;

            // set scheme for children
            List <QObject>list = Children();

            foreach (QObject obj in list)
            {
                if (obj is iisTaskBox)
                {
                    ((iisTaskBox)obj).SetScheme(scheme);
                    continue;
                }

                if (obj is iisTaskGroup)
                {
                    ((iisTaskGroup)obj).setScheme(scheme);
                    continue;
                }
            }

            Update();
        }
    }

    void paintEvent(QPaintEvent AEvent)
    {
        QPainter p = new QPainter(this);

        //p.setOpacity(0.5);
        p.FillRect(Rect, myScheme.panelBackground);
    }

    void addWidget(QWidget w)
    {
        if (w != null)
        {
            Layout().AddWidget(w);
        }
    }

    void addStretch(int s)
    {
        ((QVBoxLayout)Layout()).AddStretch(s);
    }
}
}