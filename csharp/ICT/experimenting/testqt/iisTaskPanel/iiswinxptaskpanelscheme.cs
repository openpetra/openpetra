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
public class iisWinXPTaskPanelScheme : iisTaskPanelScheme
{
    private static iisWinXPTaskPanelScheme myDefaultXPScheme = null;

    public iisWinXPTaskPanelScheme(QObject parent)
        : base(parent)
    {
        QLinearGradient panelBackgroundGrd = new QLinearGradient(0, 0, 0, 300);

        panelBackgroundGrd.SetColorAt(0, new QColor(0x7ba2e7));
        panelBackgroundGrd.SetColorAt(1, new QColor(0x6375d6));
        panelBackground = panelBackgroundGrd;

        headerBackground = new QBrush(new QColor(0x225aca));

        headerBorder = new QColor(0x225aca);
        headerSize = 25;
        headerAnimation = false;

        headerLabelScheme.text = new QColor(0xffffff);
        headerLabelScheme.textOver = new QColor(0x428eff);
        headerLabelScheme.iconSize = 22;

        headerButtonFold = new QPixmap(":/Resources/headerButtonFold_XPBlue1.png");
        headerButtonFoldOver = new QPixmap(":/Resources/headerButtonFoldOver_XPBlue1.png");
        headerButtonUnfold = new QPixmap(":/Resources/headerButtonUnfold_XPBlue1.png");
        headerButtonUnfoldOver = new QPixmap(":/Resources/headerButtonUnfoldOver_XPBlue1.png");
        headerButtonSize = new QSize(17, 17);

        groupBackground = new QBrush(new QColor(0xeff3ff));
        groupBorder = new QColor(new QColor(0xffffff));

        taskLabelScheme.text = new QColor(0x215dc6);
        taskLabelScheme.textOver = new QColor(0x428eff);
    }

    public new iisTaskPanelScheme defaultScheme(QObject AParent)
    {
        if (myDefaultXPScheme == null)
        {
            myDefaultXPScheme = new iisWinXPTaskPanelScheme(AParent);
        }

        return myDefaultXPScheme;
    }
}

public class iisWinXPTaskPanelScheme2 : iisTaskPanelScheme
{
    static iisWinXPTaskPanelScheme2 myDefaultXPScheme = null;

    public iisWinXPTaskPanelScheme2(QObject parent)
        : base(parent)
    {
        QLinearGradient panelBackgroundGrd = new QLinearGradient(0, 0, 0, 300);

        panelBackgroundGrd.SetColorAt(0, new QColor(0x7ba2e7));
        panelBackgroundGrd.SetColorAt(1, new QColor(0x6375d6));
        panelBackground = panelBackgroundGrd;

        QLinearGradient headerBackgroundGrd = new QLinearGradient(0, 0, 300, 0);
        headerBackgroundGrd.SetColorAt(0, new QColor(0xffffff));
        headerBackgroundGrd.SetColorAt(1, new QColor(0xc6d3f7));
        headerBackground = headerBackgroundGrd;

        headerBorder = new QPen(PenStyle.NoPen);
        headerSize = 25;
        headerAnimation = false;

        headerLabelScheme.text = new QColor(0x215dc6);
        headerLabelScheme.textOver = new QColor(0x428eff);
        headerLabelScheme.iconSize = 22;

        headerButtonFold = new QPixmap(":/Resources/headerButtonFold_XPBlue2.png");
        headerButtonFoldOver = new QPixmap(":/Resources/headerButtonFoldOver_XPBlue2.png");
        headerButtonUnfold = new QPixmap(":/Resources/headerButtonUnfold_XPBlue2.png");
        headerButtonUnfoldOver = new QPixmap(":/Resources/headerButtonUnfoldOver_XPBlue2.png");
        headerButtonSize = new QSize(17, 17);

        groupBackground = new QBrush(new QColor(0xd6dff7));
        groupBorder = new QColor(new QColor(0xffffff));

        taskLabelScheme.text = new QColor(0x215dc6);
        taskLabelScheme.textOver = new QColor(0x428eff);
    }

    public new iisTaskPanelScheme defaultScheme(QObject AParent)
    {
        if (myDefaultXPScheme != null)
        {
            myDefaultXPScheme = new iisWinXPTaskPanelScheme2(AParent);
        }

        return myDefaultXPScheme;
    }
}
}