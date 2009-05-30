/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       >>>> Put your full name or just a shortname here <<<<
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
using Qyoto;

namespace iisTaskPanelNamespace
{
public class iisTaskHeader : QFrame
{
    iisTaskPanelScheme myScheme;
    iisIconLabelScheme myLabelScheme;

    bool myExpandable;
    bool m_over, m_buttonOver, m_fold;
    double m_opacity;

    iisIconLabel myTitle;
    QLabel myButton;

    public iisTaskHeader(QIcon icon, string title, bool expandable, QWidget parent)
        : base(parent)
    {
        myExpandable = expandable;
        myButton = null;
        m_over = false;
        m_fold = true;
        m_buttonOver = false;
        m_opacity = 0.1;

        myTitle = new iisIconLabel(icon, title, this);
        myTitle.SetSizePolicy(QSizePolicy.Policy.Minimum, QSizePolicy.Policy.Preferred);

        Connect(myTitle, SIGNAL("activated()"), this, SLOT("fold()"));

        QHBoxLayout hbl = new QHBoxLayout();
        hbl.Margin = 2;
        SetLayout(hbl);

        hbl.AddWidget(myTitle);

        SetSizePolicy(QSizePolicy.Policy.Preferred, QSizePolicy.Policy.Maximum);

        setScheme(iisTaskPanelScheme.defaultScheme(this));
        myTitle.SetSchemePointer(myLabelScheme);

        if (myExpandable)
        {
            myButton = new QLabel(this);
            hbl.AddWidget(myButton);
            myButton.InstallEventFilter(this);
            myButton.SetFixedWidth(myScheme.headerButtonSize.Width());
            changeIcons();
        }
    }

    bool eventFilter(QObject obj, QEvent AEvent)
    {
        switch (AEvent.type())
        {
            case QEvent.TypeOf.MouseButtonPress:
                fold();
                return true;

            case QEvent.TypeOf.Enter:
                m_buttonOver = true;
                changeIcons();
                return true;

            case QEvent.TypeOf.Leave:
                m_buttonOver = false;
                changeIcons();
                return true;
        }

        return base.EventFilter(obj, AEvent);
    }

    public void setScheme(iisTaskPanelScheme scheme)
    {
        if (scheme != null)
        {
            myScheme = scheme;
            myLabelScheme = scheme.headerLabelScheme;

            if (myExpandable)
            {
                Cursor = (myLabelScheme.cursorOver ? new QCursor(CursorShape.PointingHandCursor) : new QCursor());
                changeIcons();
            }

            SetFixedHeight(scheme.headerSize);

            Update();
        }
    }

    void paintEvent(QPaintEvent AEvent)
    {
        QPainter p = new QPainter(this);

        if (myScheme.headerAnimation)
        {
            p.SetOpacity(m_opacity + 0.7);
        }

        p.SetPen(myScheme.headerBorder);
        p.SetBrush(myScheme.headerBackground);

        if (myScheme.headerBorder.Style() == PenStyle.NoPen)
        {
            p.DrawRect(Rect);
        }
        else
        {
            p.DrawRect(Rect.Adjusted(0, 0, -1, -1));
        }
    }

    void animate()
    {
        if (!myScheme.headerAnimation)
        {
            return;
        }

        if (!Enabled)
        {
            m_opacity = 0.1;
            Update();
            return;
        }

        if (m_over)
        {
            if (m_opacity >= 0.3)
            {
                m_opacity = 0.3;
                return;
            }

            m_opacity += 0.05;
        }
        else
        {
            if (m_opacity <= 0.1)
            {
                m_opacity = 0.1;
                return;
            }

            m_opacity = QMax(0.1, m_opacity - 0.05);
        }

        QTimer.singleShot(100, this, SLOT("animate()"));
        Update();
    }

    void enterEvent(QEvent AEvent)
    {
        m_over = true;

        if (Enabled)
        {
            QTimer.singleShot(100, this, SLOT("animate()"));
        }

        Update();
    }

    void leaveEvent(QEvent AEvent)
    {
        m_over = false;

        if (Enabled)
        {
            QTimer.singleShot(100, this, SLOT("animate()"));
        }

        Update();
    }

    void fold()
    {
        if (myExpandable)
        {
            Emit.Activated();

            m_fold = !m_fold;
            changeIcons();
        }
    }

    void changeIcons()
    {
        if (myButton == null)
        {
            return;
        }

        if (m_buttonOver)
        {
            if (m_fold)
            {
                myButton.SetPixmap(myScheme.headerButtonFoldOver.Pixmap(myScheme.headerButtonSize));
            }
            else
            {
                myButton.SetPixmap(myScheme.headerButtonUnfoldOver.Pixmap(myScheme.headerButtonSize));
            }
        }
        else
        {
            if (m_fold)
            {
                myButton.SetPixmap(myScheme.headerButtonFold.Pixmap(myScheme.headerButtonSize));
            }
            else
            {
                myButton.SetPixmap(myScheme.headerButtonUnfold.Pixmap(myScheme.headerButtonSize));
            }
        }
    }

    void mouseReleaseEvent(QMouseEvent AEvent)
    {
        if (AEvent.Button() == MouseButton.LeftButton)
        {
            Emit.Activated();
        }
    }

    protected new ITaskHeaderSignals Emit
    {
        get
        {
            return (ITaskHeaderSignals)Q_EMIT;
        }
    }
}

public interface ITaskHeaderSignals : IQWidgetSignals
{
    [Q_SIGNAL]
    void Activated();
}
}