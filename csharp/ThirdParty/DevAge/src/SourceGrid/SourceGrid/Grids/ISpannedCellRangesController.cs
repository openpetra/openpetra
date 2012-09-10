using System;

namespace SourceGrid
{
	/// <summary>
	/// This interfaces defines the logic how spanned cell ranges
	/// are handled.
	/// At the moment this interface is bloated, since it was
	/// copy pasted from implementation straight from Grid.
	/// 
	/// In the future it would be nice to reduce it
	/// so that separate implementations would be possible
	/// </summary>
	public interface ISpannedCellRangesController
	{
		ISpannedRangesCollection SpannedRangesCollection{get;}
		
		void MoveLeftSpannedRanges(int startIndex, int moveCount);
		void MoveUpSpannedRanges(int startIndex, int moveCount);
		void MoveDownSpannedRanges(int startIndex, int moveCount);
		void MoveRightSpannedRanges(int startIndex, int moveCount);
		void ExpandSpannedColumns(int startIndex, int count);
		void ExpandSpannedRows(int startIndex, int count);
		void ShrinkOrRemoveSpannedRows(int startIndex, int count);
		void ShrinkOrRemoveSpannedColumns(int startIndex, int count);
	
		void RemoveSpannedCellReferencesInRows(int startIndex, int count);
		void RemoveSpannedCellReferencesInColumns(int startIndex, int count);
		
		
		void Swap(int rowIndex1, int rowIndex2);
		/// <summary>
		/// Adds or updates given range.
		/// Updates range only when existing range with given start position is found
		/// </summary>
		/// <param name="newRange"></param>
		void UpdateOrAdd(Range newRange);
		void Update(Range newRange);
	}
}
