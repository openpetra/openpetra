based on the source code of SourceGrid as of August 30th, 2010
(found here: http://bitbucket.org/dariusdamalakas/sourcegrid/changeset/042fd63664ad)


We have made two important modifications:
Please manually adjust the files in the downloaded source code of SourceGrid, based on the diff files included in this directory.

==== Includes a fix for Selection.Focus on leaving the Grid ====
The file GridVirtual.cs contains a modification (by ChristopherT) for Class SourceGird.GridVirtual. This modification in Method 'OnValidated' modifies a behaviour of the grid: it fired row-level events when the grid itself loses focus even though the current Row had not changed. (See http://sourceforge.net/apps/mediawiki/openpetraorg/index.php?title=SourceGrid_specification_and_testing#Event_-_Keyboard_handling for details.)


==== Include 'Fixed Rows' in the calculation of the Column's AutoSize ====
The file ColumnInfoCollection.cs contains a modification (by ChristianK) for Class 'ColumnInfoCollection'. This modification changes the calculation of the Column's AutoSize so that it is done in the way as SourceGrid 4.11 did it.
We want that behaviour so that the Column Header Texts are also taken into consideration,
ie. the Column Width can never be AutoSized smaller than the Header Text. This prevents
Columns from 'collapsing' to a few pixels width if there is no data in a particular column
(eg. Partner Find screen). 
From SourceGrid 4.20 onwards, Fixed Rows are excluded from the AutoSize calculation algorithm, which causes the mentioned problem of Columns collapsing if there is no data in them.



File SourceGrid.dll:
--------------------
Built from the downloaded source code of SourceGrid, plus our changes described above. This is used with OpenPetra!



File frmSample17DataGrid.cs: 
----------------------------
frmSample17_Load loads a DataTable into a DataGrid
