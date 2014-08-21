based on the source code of SourceGrid as of July 16, 2012
(found here: http://bitbucket.org/dariusdamalakas/sourcegrid/)

==== Our version history ====

File SourceGrid.dll:
--------------------
Built from the downloaded source code of SourceGrid, plus our changes described above. This is used with OpenPetra!
The latest version is 4.40.5336.16408, dated 11 August 2014, size 536KB - this fixes all bugs and features below

(Version number 4.40.5283.24660, dated 19 June 2014, size 536KB - this fixes all bugs 1 to 10 below)
(Version number 4.40.5210.20614, dated 07 April 2014, size 536KB - this fixes all bugs 1 to 9 below)
(Version number 4.40.5123.25924, dated 10 January 2014, size 536KB - this fixed all bugs 1 to 8 below)
(Version number 4.40.5099.15393, dated 17 December 2013, size 536KB - this fixed all bugs 1 to 7 below)
(Version number 4.40.5081.39414, dated 29 November 2013, size 536KB - this fixed bugs 1 to 6 below
(Version number 4.40.5049.27439, dated 28 October 2013, size 536KB - this fixed bugs 1 to 5 below
(Version number 4.40.5029.25553, dated 8 October 2013, size 536KB - this fixed bugs 1 to 4 below)
(Version number 4.40.4937.16700, dated 8 July 2013, size 536KB - this fixed bugs 1 to 3 below)
(Version number 4.40.4681.15711, dated 25 Oct 2012, size 536KB - this fixed bugs 1 and 2 below)

Over time we have made several important modifications:
Please manually adjust the files in the downloaded source code of SourceGrid, based on the complete (or partial diff) files included in this directory.

==== 1.  Include 'Fixed Rows' in the calculation of the Column's AutoSize ====
The file ColumnInfoCollection.cs contains a modification (by ChristianK) for Class 'ColumnInfoCollection'. 
This modification changes the calculation of the Column's AutoSize so that it is done in the way as SourceGrid 4.11 did it.
We want that behaviour so that the Column Header Texts are also taken into consideration, ie. the Column Width can never be AutoSized smaller than the Header Text. This prevents Columns from 'collapsing' to a few pixels width if there is no data in a particular column
(eg. Partner Find screen). 
From SourceGrid 4.20 onwards, Fixed Rows are excluded from the AutoSize calculation algorithm, which causes the mentioned problem of Columns collapsing if there is no data in them.

==== 2.  Fix a bug in the display or otherwise of the scrollbars ====
The file CustomScrollControl.cs contains a modification (by AlanP) that fixes a bug where the grid does not display the scrollbars correctly as the content of the grid changes.  
For example, if the grid already has a horizontal scrollbar but no need for a vertical one, and then you add rows, you reach a stage where at least one row is 'hidden' behind the horizontal scrollbar.

==== 3.  Fix a bug with SHIFT selection of multiple rows (8 July 2013) ====
The file GridVirtual.cs contains a modification (by AlanP) that fixes a bug whereby the simple action of pressing SHIFT caused a cascade of row/selection change events.  The consequence of this was that pressing SHIFT on its own changed the selection and using SHIFT with a left mouse click failed to select multiple rows.

==== 4.  More fixes relating to multi-selection using mouse and keyboard, as well as some issues with our validation when controls lose focus (September 2013) ====
It became apparent that a number of bugs were occurring that could not be fixed efficiently using the methods inherent in the grid code.
Alan made the decision to 'fork' the SourceGrid for OpenPetra purposes and make the event handling suit our purposes better.
This meant that we wanted only one event to be fired for a given occurrence (instead of multiple events in the standard grid).
It meant that we could use the grid with a FocusStyle of None, that Selection.ActivePosition would always be valid, that we could use FocusRowLeaving for validation purposes (and only need to handle one event) and Selection_Changed as the means of showing new details.

==== 5.  A few bugs came to light with the new grid that had fixed issues 1-4. (October 2013)
Some Finance screens used events that I had not checked before and these needed to have event suppression added because otherwise we got a stack overflow.
Some Finance screens needed to ensure that specific columns could be fosussed rather than assuming it was column 0 for the whole row.
Most important of all was the realisation that we absolutely needed to have our SelectRowInGrid code in Open Petra be able to fire off the SelectionChanged event even when before the grid is displayed.  Some Finance screens call SelectRowInGrid as part of the early code that runs before activation and then rely on the event having set up the details for the row.  The October 2013 version implements these changes.

==== 6.  Changes made to the Special Grid Key Handler
There was a bug in the key handler which meant that the TAB, ENTER and ESC keys did not work as intended when using edit-in-place. This bug was simple to fix, but then additional functionality was added so that ENTER when pressed with SHIFT or CTRL had additional functionality.  SHIFT+ENTER completes the edit and moves down one row in the same column.  CTRL+ENTER completes the edit and moves to the start of the row below.  ENTER moves the focus to the next editable cell in the row, so we can cope with grids with multiple editable columns, even when they are separated by non-editable columns.

==== 7.  Change to fix a bug when we use our DoubleClickHeader event
Made a small change to the grid that fixed an issue when we use this event.  Added an 'if' clause to the grid so that it does not sort a column that is being resized.  Without this change we could not double click a header to auto-Resize it without also sorting it!  The grid handled this in a different way but our use of the new event kind of exposed this grid 'bug'.

==== 8.  (Jan 2014) Change to fix a strange behaviour whereby when you repeatedly maximized and restored the Open Petra screen the resizing response took progressively longer and longer!  The grid code method PerformStretch() was calling the column autosize method un-necessarily (since it had just been called already by the calling method) and this was taking far too long, so it has been commented out.  Also in this check-in I have made a tweak to a previous fix that always interpreted a mouse click as occurring in column 1.  It now only does that when the selection mode is by row.  We occasionally use the grid selection mode of 'by column' and for that we don't want to force the click to always be column 1!!  Finally, I enabled the use of left and right arrows - but only when in 'by column' selection mode.  In fact this is not required by those screens because they always set the active position to -1,-1 whic renders these keys inoperative.  But one day it might be nice to change OP to support them.

==== 9.  (April 2014)  Just another tweak to the code that can 'force' a mouse click to be on the first column.  It is now forced to be the first VISIBLE column - in the traditional sense of visible rather than view-port visible.  The AP screens make use of setting column 0 visible=false when it is desired to hide the check box column.  This had the unfortuantae result of preventing mouse clicks working at all!

==== 10.  (June 2014)  Fixed a bug that was apparent on the partner/Find screen that has three fixed columns.  The row highlight overlapped with one of the fixed columns.  The change was to one method call in one line of the Draw code for the highlight decorator.  To be honest I don't understand how it was supposed to work!  In addition I have added code that highlights the fixed columns on the left of a selected row.

==== 11.  (August 2014)  1: Improved the way in which Edit-in-place works when using the mouse to move away from a cell being edited.  In effect there was a line of code missing that called BeginEdit() when the mouse was used to re-enter an edit cell.  2: Also had to modify the edit-in-place code that I had used for TAB and ENTER so that it is ignored for a screen like LocalData in Partner/Edit.  This uses controllers rather than editors and was lifted from Petra 2.3.  I achieved what I wanted by adding a new special key enum which only gets set for that screen but then makes Tab and Enter work the way it always used to.


All the changes made for this are commented with // AlanP:

Files affected:
/Cells/Controllers/MouseSelection.cs
/Cells/CellContext.cs
/Common/ColumnInfoCollection.cs
/Common/CustomScrollControl.cs
/Grids/GridVirtual.cs
/Selection/IGridSelection.cs
/Selection/RowSelection.cs
/Selection/SelectionBase.cs
/Cells/Controllers/Resizable.cs
/Cells/Controllers/SortableHeader.cs
/Decorators/DecoratorSelection.cs
/Common/Enums.cs

Other changes
-------------


File frmSample17DataGrid.cs: 
----------------------------
frmSample17_Load loads a DataTable into a DataGrid
