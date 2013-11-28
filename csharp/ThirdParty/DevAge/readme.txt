based on the source code of SourceGrid as of July 16, 2012
(found here: http://bitbucket.org/dariusdamalakas/sourcegrid/)

==== Our version history ====

File SourceGrid.dll:
--------------------
Built from the downloaded source code of SourceGrid, plus our changes described above. This is used with OpenPetra!
The latest version is 4.40.5080.17704, dated 28 November 2013, size 536KB - this fixes all bugs and features below

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

Other changes
-------------


File frmSample17DataGrid.cs: 
----------------------------
frmSample17_Load loads a DataTable into a DataGrid
