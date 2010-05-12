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
using Qyoto;

namespace iisTaskPanelNamespace
{
public class iisTaskGroup : QFrame
{
    iisTaskPanelScheme myScheme;
    iisIconLabelScheme myLabelScheme;

    bool myHasHeader;

    public iisTaskGroup(QWidget parent, bool hasHeader)
        : base(parent)
    {
        myHasHeader = hasHeader;

        //setMinimumHeight(32);

        setScheme(iisTaskPanelScheme.defaultScheme(this));

        QVBoxLayout vbl = new QVBoxLayout();
        vbl.Margin = 4;
        vbl.Spacing = 0;
        SetLayout(vbl);
    }

    public QBoxLayout groupLayout()
    {
        return (QBoxLayout)Layout();
    }

    public void setScheme(iisTaskPanelScheme scheme)
    {
        if (scheme != null)
        {
            myScheme = scheme;
            myLabelScheme = scheme.taskLabelScheme;
            Update();
        }
    }

    public void addIconLabel(iisIconLabel label, bool addToLayout)
    {
        if (label == null)
        {
            return;
        }

        if (addToLayout)
        {
            Layout().AddWidget(label);
        }

        label.SetSchemePointer(myLabelScheme);
    }

    void paintEvent(QPaintEvent AEvent)
    {
        QPainter p = new QPainter(this);

        //p.setOpacity(/*m_opacity+*/0.7);
        //p.fillRect(rect(), myScheme.groupBackground);

        p.SetBrush(myScheme.groupBackground);
        p.SetPen(myScheme.groupBorder);
        p.DrawRect(Rect.Adjusted(0, -(myHasHeader ? 1 : 0), -1, -1));
    }
}
}