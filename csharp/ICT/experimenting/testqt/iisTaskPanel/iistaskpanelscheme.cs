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
public class iisTaskPanelScheme : QObject
{
    static iisTaskPanelScheme myDefaultScheme = null;

    public QBrush panelBackground;

    public QBrush headerBackground;
    public iisIconLabelScheme headerLabelScheme;
    public QPen headerBorder;

    public int headerSize;

    public bool headerAnimation;

    public QIcon headerButtonFold, headerButtonFoldOver, headerButtonUnfold, headerButtonUnfoldOver;
    public QSize headerButtonSize;

    public QBrush groupBackground;
    public QPen groupBorder;
    public int groupFoldSteps, groupFoldDelay;
    public iisIconLabelScheme taskLabelScheme;

    public iisTaskPanelScheme(QObject parent)
        : base(parent)
    {
        QLinearGradient panelBackgroundGrd = new QLinearGradient(0, 0, 0, 300);

        panelBackgroundGrd.SetColorAt(0, new QColor(0x006181));
        panelBackgroundGrd.SetColorAt(1, new QColor(0x00A99D));
        panelBackground = panelBackgroundGrd;

        QLinearGradient headerBackgroundGrd = new QLinearGradient(0, 0, 0, 30);
        headerBackgroundGrd.SetColorAt(0, new QColor(0xFAFDFD));
        headerBackgroundGrd.SetColorAt(0.3, new QColor(0xC8EAE9));
        headerBackgroundGrd.SetColorAt(0.31, new QColor(0x79A8A6));
        headerBackgroundGrd.SetColorAt(1, new QColor(0x85DEA9));
        headerBackground = headerBackgroundGrd;

        headerLabelScheme = new iisIconLabelScheme();
        headerLabelScheme.text = new QColor(0x00736A);
        headerLabelScheme.textOver = new QColor(0x044F49);
        headerLabelScheme.textOff = new QColor(0x4F4F4F);
        headerLabelScheme.focusPen = new QPen(new QColor(0x006181), 1, PenStyle.DotLine);
        QFont f = new QFont();
        f.SetWeight(1);
        headerLabelScheme.font = f;
        headerLabelScheme.iconSize = 24;
        headerLabelScheme.underlineOver = false;
        headerLabelScheme.cursorOver = true;

        headerSize = 28;

        headerAnimation = true;

        headerBorder = new QColor(0x79A8A6);

        Q_INIT_RESOURCE("iisTaskPanel");

        headerButtonFold = new QPixmap(":/Resources/headerButtonFold.png");
        headerButtonFoldOver = new QPixmap(":/Resources/headerButtonFoldOver.png");
        headerButtonUnfold = new QPixmap(":/Resources/headerButtonUnfold.png");
        headerButtonUnfoldOver = new QPixmap(":/Resources/headerButtonUnfoldOver.png");
        headerButtonSize = new QSize(18, 18);


        QLinearGradient groupBackgroundGrd = new QLinearGradient(0, 0, 300, 0);
        groupBackgroundGrd.SetColorAt(1, new QColor(0xB8FFD9));
        groupBackgroundGrd.SetColorAt(0, new QColor(0xFAFDFD));
        groupBackground = groupBackgroundGrd;

        groupBorder = new QColor(0x79A8A6);

        groupFoldSteps = 20; groupFoldDelay = 15;

        taskLabelScheme = new iisIconLabelScheme();
        taskLabelScheme.text = new QColor(0x00736A);
        taskLabelScheme.textOver = new QColor(0x044F49);
        taskLabelScheme.textOff = new QColor(0xb0b0b0);
        taskLabelScheme.focusPen = new QPen(new QColor(0x006181), 1, PenStyle.DotLine);
        taskLabelScheme.iconSize = 16;
        taskLabelScheme.underlineOver = true;
        taskLabelScheme.cursorOver = true;
    }

    static public iisTaskPanelScheme defaultScheme(QObject AParent)
    {
        if (myDefaultScheme == null)
        {
            myDefaultScheme = new iisTaskPanelScheme(AParent);
        }

        return myDefaultScheme;
    }
}
}