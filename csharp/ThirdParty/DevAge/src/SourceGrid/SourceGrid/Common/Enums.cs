using System;

namespace SourceGrid
{
	/// <summary>
	/// Selection Mode
	/// </summary>
	public enum GridSelectionMode
	{
		Cell = 1,
		Row = 2,
		Column = 3
	}

	/// <summary>
	/// ContextMenuStyle (Flags)
	/// </summary>
	[Flags]
	public enum ContextMenuStyle
	{
		None = 0,
		ColumnResize = 1,
		RowResize = 2,
		AutoSize = 4,
		ClearSelection = 8,
		CopyPasteSelection = 16,
		CellContextMenu = 32
	}

	/// <summary>
	/// EditableMode Cell mode (Flags)
	/// </summary>
	[Flags]
	public enum EditableMode
	{
		/// <summary>
		/// No edit support
		/// </summary>
		None = 0,
		/// <summary>
		/// Edit the cell with F2 key ( 1 )
		/// </summary>
		F2Key = 1,
		/// <summary>
		/// Edit the cell with a double click (2)
		/// </summary>
		DoubleClick = 2,
		/// <summary>
		/// Edit a cell with a single Key (4)
		/// </summary>
		SingleClick = 4,
		/// <summary>
		/// Edit the cell pressing any keys (8 + F2Key)
		/// </summary>
		AnyKey = 9,
		/// <summary>
		/// Edit the cell when it receive the focus (16)
		/// </summary>
		Focus = 16,
		/// <summary>
		/// DoubleClick + F2Key
		/// </summary>
		Default = DoubleClick | F2Key | AnyKey
	}

	/// <summary>
	/// Type of resize of the cells (Flags)
	/// </summary>
	[Flags]
	public enum CellResizeMode
	{
		None = 0,
		Height = 1,
		Width = 2,
		Both = 3
	}

	/// <summary>
	/// Special keys that the grid can handle. You can change this enum to block or allow some special keys function. (Flags)
	/// </summary>
	[Flags]
	public enum GridSpecialKeys
	{
		/// <summary>
		/// No keys
		/// </summary>
		None = 0,
//		/// <summary>
//		/// Ctrl+C for Copy selection operation
//		/// </summary>
//		Ctrl_C = 1,
//		/// <summary>
//		/// Ctrl+V for paste selection operation
//		/// </summary>
//		Ctrl_V = 2,
//		/// <summary>
//		/// Ctrl+X for cut selection operation
//		/// </summary>
//		Ctrl_X = 4,
//		/// <summary>
//		/// Delete key, for Clear selection operation
//		/// </summary>
//		Delete = 8,
		/// <summary>
		/// Arrows keys, for moving focus cell operation
		/// </summary>
		Arrows = 16,
		/// <summary>
		/// Tab and Shift+Tab keys, for moving focus cell operation
		/// </summary>
		Tab = 32,
		/// <summary>
		/// PageDown and PageUp keys, for page operation
		/// </summary>
		PageDownUp = 64,
		/// <summary>
		/// Enter key, for apply editing operation
		/// </summary>
		Enter = 128,
		/// <summary>
		/// Escape key, for cancel editing operation
		/// </summary>
		Escape = 256,
		/// <summary>
		/// Control key, for selection operations. Enables the selection of non adjacent cells
		/// </summary>
		Control = 512,
		/// <summary>
		/// Shift key, for selection operations. Enables the selection of the range from the focused cell to the selected cells.
		/// </summary>
		Shift = 1024,
		/// <summary>
		/// Default: Arrows|Tab|PageDownUp|Enter|Escape|Control|Shift
		/// </summary>
		Default = Arrows|Tab|PageDownUp|Enter|Escape|Control|Shift
	}


	/// <summary>
	/// Position type of the cell. Look at the .vsd diagram for details.
	/// </summary>
	public enum CellPositionType
	{
		/// <summary>
		/// Empty Cell
		/// </summary>
		Empty = 0,
		/// <summary>
		/// Fixed Top+Left Cell
		/// </summary>
		FixedTopLeft = 1,
		/// <summary>
		/// Fixed Top Cell
		/// </summary>
		FixedTop = 2,
		/// <summary>
		/// Fixed Left cell
		/// </summary>
		FixedLeft = 3,
		/// <summary>
		/// Scrollable Cell
		/// </summary>
		Scrollable = 4
	}

	/// <summary>
	/// SelectionChangeEventType
	/// </summary>
	public enum SelectionChangeEventType
	{
		/// <summary>
		/// Add
		/// </summary>
		Add = 1,
		/// <summary>
		/// Remove
		/// </summary>
		Remove = 2,
		/// <summary>
		/// Clear
		/// </summary>
		Clear = 3
	}

	/// <summary>
	/// FocusStyle (Flags). Used to customize the style of the focus.
	/// </summary>
	[Flags]
	public enum FocusStyle
	{
		None = 0,
		/// <summary>
		/// Remove the focus cell when the grid lost the focus
		/// </summary>
		RemoveFocusCellOnLeave = 1,
		/// <summary>
		/// Remove the selection when the grid lost the focus
		/// </summary>
		RemoveSelectionOnLeave = 2,
		/// <summary>
		/// Set the focus on the first cell when the grid receive the focus and there isnt' an active cell. Use the FocusFirstCell method.
		/// </summary>
		FocusFirstCellOnEnter = 4,
		/// <summary>
		/// The default value for this flags: FocusStyle.FocusFirstCellOnEnter | FocusStyle.RemoveFocusCellOnLeave
		/// </summary>
		Default = FocusStyle.FocusFirstCellOnEnter | FocusStyle.RemoveFocusCellOnLeave
	}

	/// <summary>
	/// AutoSizeMode (Flags)
	/// </summary>
	[Flags]
	public enum AutoSizeMode
	{
		None = 0,
		/// <summary>
		/// Enable the AutoSize
		/// </summary>
		EnableAutoSize = 1,
		/// <summary>
		/// Enable the AutoSize only for visible view
		/// </summary>
		EnableAutoSizeView = 8 | EnableAutoSize,
		/// <summary>
		/// Enable Stretch operation
		/// </summary>
		EnableStretch = 2,
		/// <summary>
		/// If this flag is selected the Measure function returns always the minimum column/row size and don't calculate the real required size. This flag can be used to don't consider the content of a column/row
		/// </summary>
		MinimumSize = 4,
		/// <summary>
		/// Default: EnableAutoSize, EnableStretch
		/// </summary>
		Default = EnableAutoSize | EnableStretch
	}

	/// <summary>
	/// SelectionBorderMode. Used with Grid.Selection.BorderMode property
	/// </summary>
	public enum SelectionBorderMode
	{
		/// <summary>
		/// Don't draw a border around each selection range
		/// </summary>
		None = 0,
		/// <summary>
		/// Draw a border around the range that contains the focus.
		/// </summary>
		FocusRange = 1,
		/// <summary>
		/// Draw a border around the focusl cell
		/// </summary>
		FocusCell = 2,
		/// <summary>
		/// Draw a border around the selection range only is there is only one range selected
		/// </summary>
		UniqueRange = 3,
		/// <summary>
		/// Like the UniqueRange enum but when there is more then one range set the range only on the FocusCell
		/// </summary>
		Auto = 4
	}


	/// <summary>
	/// SelectionmaskStyle, used to customize the visual style of the selection mask. (Flags)
	/// </summary>
	[Flags]
	public enum SelectionMaskStyle
	{
		/// <summary>
		/// None. No special flags
		/// </summary>
		None = 0,
		/// <summary>
		/// Used to draw only initialized cells. If you have uninitialized cell (you don't create a cell for a specific position: grid[0,0] = null;) the selection mask is not drawed on this cell. Consider that with this flags the drawing method is more complex and slow.If you use this flag the border can only be set to FocusCell or None.
		/// </summary>
		DrawOnlyInitializedCells = 1,
		/// <summary>
		/// Used to draw the selection over the the cells, usually used with a transparent backcolor drawed over the normal cell. If not set, each cells (View) draw the selection or focus backcolor inside the normal drawing code.
		/// </summary>
		DrawSeletionOverCells = 2,
		/// <summary>
		/// Default value: None
		/// </summary>
		Default = DrawSeletionOverCells
	}


	/// <summary>
	/// Enum used to specify the cut operation style. Used with the RangeData class.
	/// </summary>
	public enum CutMode
	{
		/// <summary>
		/// Cut disabled.
		/// </summary>
		None = 0,
		/// <summary>
		/// Cut enabled, the data are removed from the source only when pasting it on the destination. Used usually with the drag and drop operations.
		/// </summary>
		[Obsolete("If you want to cut data before paste, then" +
		          " do so explicitly from code before pasting data")]
		CutOnPaste = 1,
		/// <summary>
		/// Cut enabled, the data are removed immediately when cutting the data source.
		/// </summary>
		CutImmediately = 2
	}

	/// <summary>
	/// String trimming mode
	/// </summary>
	public enum TrimmingMode
	{
		None = 0,
		Char = 1,
		Word = 2
	}

	[Flags]
	public enum ClipboardMode
	{
		None = 0,
		/// <summary>
		/// Copy using Ctrl+C
		/// </summary>
		Copy = 1,
		/// <summary>
		/// Cut using Ctrl+X
		/// </summary>
		Cut = 2,
		/// <summary>
		/// Paste using Ctrl+V
		/// </summary>
		Paste = 4,
		/// <summary>
		/// Delete using Del key
		/// </summary>
		Delete = 8,
		All = 15
	}

	/// <summary>
	/// Optimize mode used when constructing the grid control.
	/// </summary>
	public enum CellOptimizeMode
	{
		/// <summary>
		/// Optimize the grid for many rows
		/// </summary>
		ForRows = 1,
		/// <summary>
		/// Optimize the grid for many columns
		/// </summary>
		ForColumns = 2
	}
}