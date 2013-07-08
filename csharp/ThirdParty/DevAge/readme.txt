based on the source code of SourceGrid as of July 16, 2012
(found here: http://bitbucket.org/dariusdamalakas/sourcegrid/)


We have made three important modifications:
Please manually adjust the files in the downloaded source code of SourceGrid, based on the diff files included in this directory.

==== Include 'Fixed Rows' in the calculation of the Column's AutoSize ====
The file ColumnInfoCollection.cs contains a modification (by ChristianK) for Class 'ColumnInfoCollection'. 
This modification changes the calculation of the Column's AutoSize so that it is done in the way as SourceGrid 4.11 did it.
We want that behaviour so that the Column Header Texts are also taken into consideration,
ie. the Column Width can never be AutoSized smaller than the Header Text. This prevents
Columns from 'collapsing' to a few pixels width if there is no data in a particular column
(eg. Partner Find screen). 
From SourceGrid 4.20 onwards, Fixed Rows are excluded from the AutoSize calculation algorithm, which causes the mentioned problem of Columns collapsing if there is no data in them.

==== Fix a bug in the display or otherwise of the scrollbars ====
The file CustomScrollControl.cs contains a modification (by AlanP) that fixes a bug where the grid does not display the scrollbars correctly as the content of the grid changes.  
For example, if the grid already has a horizontal scrollbar but no need for a vertical one, and then you add rows, you reach a stage where at least one row is 'hidden' behind the horizontal scrollbar.

==== Fix a bug with SHIFT selection of multiple rows (8 July 2013) ====
The file GridVirtual.cs contains a modification (by AlanP) that fixes a bug whereby the simple action of pressing SHIFT caused a cascade of row/selection change events.  The consequence of this was that pressing SHIFT on its own changed the selection and using SHIFT with a left mouse click failed to select multiple rows.


Other changes
-------------

==== A previous fix relating to Selection.Focus on leaving the Grid is no longer required ====
The file GridVirtual.cs (that we previously modified) no longer needs any change.


File SourceGrid.dll:
--------------------
Built from the downloaded source code of SourceGrid, plus our changes described above. This is used with OpenPetra!
The latest version is 4.40.4937.16700, dated 8 July 2013, size 536KB

(Version number 4.40.4681.15711, dated 25 Oct 2012, size 536KB - this fixed bugs 1 and 2 above)



File frmSample17DataGrid.cs: 
----------------------------
frmSample17_Load loads a DataTable into a DataGrid
