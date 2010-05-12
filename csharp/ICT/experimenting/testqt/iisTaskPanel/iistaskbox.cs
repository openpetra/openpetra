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
public class iisTaskBox : QFrame
{
    double m_foldStep, m_foldDelta, m_fullHeight, m_tempHeight;
    int m_foldDirection;

    QPixmap m_foldPixmap;

    iisTaskHeader myHeader;
    iisTaskGroup myGroup;
    QWidget myDummy;

    iisTaskPanelScheme myScheme;

    public iisTaskBox(string title, bool expandable, QWidget parent)
        : base(parent)
    {
        myHeader = new iisTaskHeader(new QPixmap(), title, expandable, this);
        init();
    }

    public iisTaskBox(QPixmap icon, string title, bool expandable, QWidget parent)
        : base(parent)
    {
        myHeader = new iisTaskHeader(icon, title, expandable, this);
        init();
    }

    void init()
    {
        m_foldStep = 0;

        myScheme = iisTaskPanelScheme.defaultScheme(this);

        QVBoxLayout vbl = new QVBoxLayout();
        vbl.Margin = 0;
        vbl.Spacing = 0;
        SetLayout(vbl);

        vbl.AddWidget(myHeader);

        myGroup = new iisTaskGroup(this, true);
        vbl.AddWidget(myGroup);

        myDummy = new QWidget(this);
        vbl.AddWidget(myDummy);
        myDummy.Hide();

        Connect(myHeader, SIGNAL("Activated()"), this, SLOT("ShowHide()"));
    }

    public void SetScheme(iisTaskPanelScheme pointer)
    {
        myScheme = pointer;
        myHeader.setScheme(pointer);
        myGroup.setScheme(pointer);
        Update();
    }

    public QBoxLayout GroupLayout()
    {
        return myGroup.groupLayout();
    }

    public void AddIconLabel(iisIconLabel label, bool addToLayout)
    {
        myGroup.addIconLabel(label, addToLayout);
    }

    void ShowHide()
    {
        if (m_foldStep != 0.0)
        {
            return;
        }

        m_foldPixmap = QPixmap.GrabWidget(myGroup, myGroup.Rect);

        if (myGroup.IsVisible())
        {
            m_tempHeight = m_fullHeight = myGroup.Height();
            m_foldDelta = m_fullHeight / myScheme.groupFoldSteps;
            m_foldStep = myScheme.groupFoldSteps;
            m_foldDirection = -1;

            myGroup.Hide();
            myDummy.SetFixedSize(myGroup.Size);
            myDummy.Show();

            QTimer.singleShot(myScheme.groupFoldDelay, this, SLOT("processHide()"));
        }
        else
        {
            m_foldStep = myScheme.groupFoldSteps;
            m_foldDirection = 1;
            m_tempHeight = 0;

            QTimer.singleShot(myScheme.groupFoldDelay, this, SLOT("processShow()"));
        }

        myDummy.Show();
    }

    void processHide()
    {
        if ((--m_foldStep) == 0)
        {
            myDummy.SetFixedHeight(0);
            myDummy.Hide();
            m_foldPixmap = new QPixmap();
            SetFixedHeight(myHeader.Height());
            SetSizePolicy(QSizePolicy.Policy.Preferred, QSizePolicy.Policy.Preferred);
            return;
        }

        UpdatesEnabled = false;

        m_tempHeight -= m_foldDelta;
        myDummy.SetFixedHeight((int)m_tempHeight);
        SetFixedHeight(myDummy.Height() + myHeader.Height());

        QTimer.singleShot(myScheme.groupFoldDelay, this, SLOT("processHide()"));

        UpdatesEnabled = true;
    }

    void processShow()
    {
        if ((--m_foldStep) == 0)
        {
            myDummy.Hide();
            m_foldPixmap = new QPixmap();
            myGroup.Show();
            SetFixedHeight((int)(m_fullHeight + myHeader.Height()));
            SetSizePolicy(QSizePolicy.Policy.Preferred, QSizePolicy.Policy.Preferred);
            MaximumHeight = System.Int32.MaxValue;
            MinimumHeight = 0;
            return;
        }

        UpdatesEnabled = false;

        m_tempHeight += m_foldDelta;
        myDummy.SetFixedHeight((int)m_tempHeight);
        SetFixedHeight(myDummy.Height() + myHeader.Height());

        QTimer.singleShot(myScheme.groupFoldDelay, this, SLOT("processShow()"));

        UpdatesEnabled = true;
    }

    void paintEvent(QPaintEvent AEvent)
    {
        QPainter p = new QPainter(this);

        if (myDummy.IsVisible())
        {
            if (m_foldDirection < 0)
            {
                p.SetOpacity((double)m_foldStep / myScheme.groupFoldSteps);
            }
            else
            {
                p.SetOpacity((double)(myScheme.groupFoldSteps - m_foldStep) / myScheme.groupFoldSteps);
            }

            p.DrawPixmap(myDummy.X, myDummy.Y, m_foldPixmap);

            return;
        }
    }
}
}