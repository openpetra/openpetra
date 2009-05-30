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
public class iisIconLabel : QWidget
{
    QIcon myPixmap;
    string myText;

    QColor myColor, myColorOver, myColorDisabled;
    QFont myFont;
    QPen myPen;

    iisIconLabelScheme mySchemePointer;

    bool m_over, m_pressed;

    bool m_changeCursorOver, m_underlineOver;

    public iisIconLabel(QIcon icon, string title, QWidget parent)
        : base(parent)
    {
        myPixmap = icon;
        myText = title;
        m_over = false;
        m_pressed = false;
        m_changeCursorOver = true;
        m_underlineOver = true;
        mySchemePointer = null;
        FocusPolicy = FocusPolicy.StrongFocus;

        myFont = new QFont();
        myFont.SetWeight(0);
        myPen = new QPen();
        myPen.SetStyle(PenStyle.NoPen);

        myColor = myColorOver = myColorDisabled = new QColor();
    }

    public void SetSchemePointer(iisIconLabelScheme pointer)
    {
        mySchemePointer = pointer;
        Update();
    }

    public void SetColors(QColor color, QColor colorOver, QColor colorOff)
    {
        myColor = color;
        myColorOver = colorOver;
        myColorDisabled = colorOff;
        Update();
    }

    public void SetFont(QFont font)
    {
        myFont = font;
        Update();
    }

    public void SetFocusPen(QPen pen)
    {
        myPen = pen;
        Update();
    }

    public override QSize SizeHint()
    {
        return MinimumSize;
    }

    public override QSize MinimumSizeHint()
    {
        int s = (mySchemePointer != null) ? (mySchemePointer).iconSize : 16;
        QPixmap px = myPixmap.Pixmap(s, s,
            Enabled ? QIcon.Mode.Normal : QIcon.Mode.Disabled);

        int h = 4 + px.Height();
        int w = 8 + px.Width();

        if (myText.Length != 0)
        {
            QFontMetrics fm = new QFontMetrics(myFont);
            w += fm.Width(myText);
            h = QMax(h, 4 + fm.Height());
        }

        return new QSize(w + 2, h + 2);
    }

    protected override void PaintEvent(QPaintEvent AEvent)
    {
        QPainter p = new QPainter(this);

        QRect textRect = Rect.Adjusted(0, 0, -1, 0);

        int x = 2;

        if (!myPixmap.IsNull())
        {
            int s = (mySchemePointer != null) ? (mySchemePointer).iconSize : 16;
            QPixmap px = myPixmap.Pixmap(s, s,
                Enabled ? QIcon.Mode.Normal : QIcon.Mode.Disabled);
            p.DrawPixmap(x, 0, px);
            x += px.Width() + 4;
        }

        if (myText.Length != 0)
        {
            QColor text = myColor, textOver = myColorOver, textOff = myColorDisabled;
            QFont fnt = myFont;
            QPen focusPen = myPen;
            bool underline = m_underlineOver, cursover = m_changeCursorOver;

            if (mySchemePointer != null)
            {
                if (!text.IsValid())
                {
                    text = (mySchemePointer).text;
                }

                if (!textOver.IsValid())
                {
                    textOver = (mySchemePointer).textOver;
                }

                if (!textOff.IsValid())
                {
                    textOff = (mySchemePointer).textOff;
                }

                if (fnt.weight() == 0)
                {
                    fnt = (mySchemePointer).font;
                }

                if (focusPen.Style() == PenStyle.NoPen)
                {
                    focusPen = (mySchemePointer).focusPen;
                }

                underline = (mySchemePointer).underlineOver;
                cursover = (mySchemePointer).cursorOver;
            }

            p.SetPen(Enabled ? m_over ? textOver : text : textOff);

            if (Enabled && underline && m_over)
            {
                fnt.SetUnderline(true);
            }

            p.SetFont(fnt);

            textRect.SetLeft(x);

            // TODO? boundingRect was not initialized
            QRect boundingRect = this.Rect;

            QFontMetrics fm = new QFontMetrics(fnt);
            string txt = fm.ElidedText(myText, TextElideMode.ElideRight, textRect.Width());

            p.DrawText(textRect, (int)(AlignmentFlag.AlignLeft | AlignmentFlag.AlignVCenter), txt, boundingRect);

            if (this.Focus)
            {
                p.SetPen(focusPen);
                p.DrawRect(boundingRect.Adjusted(-2, -1, 0, 0));
            }
        }
    }

    protected override void EnterEvent(QEvent AEvent)
    {
        m_over = true;

        if (m_changeCursorOver)
        {
            QApplication.SetOverrideCursor(new QCursor(CursorShape.PointingHandCursor));
        }

        Update();
    }

    protected override void LeaveEvent(QEvent AEvent)
    {
        m_over = false;
        Update();

        if (m_changeCursorOver)
        {
            QApplication.RestoreOverrideCursor();
        }
    }

    protected override void MousePressEvent(QMouseEvent AEvent)
    {
        if (AEvent.Button() == MouseButton.LeftButton)
        {
            m_pressed = true;
            Emit.Pressed();
        }
        else
        {
            if (AEvent.Button() == MouseButton.RightButton)
            {
                Emit.ContextMenu();
            }
        }

        Update();
    }

    protected override void MouseReleaseEvent(QMouseEvent AEvent)
    {
        if (AEvent.Button() == MouseButton.LeftButton)
        {
            m_pressed = false;
            Emit.Released();

            if (Rect.Contains(AEvent.Pos()))
            {
                Emit.Clicked(true);
                Emit.Activated(true);
            }
        }

        Update();
    }

    protected override void KeyPressEvent(QKeyEvent AEvent)
    {
        switch ((Key)AEvent.Key())
        {
            case Key.Key_Space:
            case Key.Key_Return:
                Emit.Activated();
                break;
        }

        base.KeyPressEvent(AEvent);
    }

    protected new IIconLabelSignals Emit
    {
        get
        {
            return (IIconLabelSignals)Q_EMIT;
        }
    }
}

public interface IIconLabelSignals : IQWidgetSignals
{
    [Q_SIGNAL]
    void Pressed();

    [Q_SIGNAL]
    void Activated(bool AActive);

    [Q_SIGNAL]
    void Activated();

    [Q_SIGNAL]
    void Clicked(bool AClicked);

    [Q_SIGNAL]
    void Released();

    [Q_SIGNAL]
    void ContextMenu();
}
}