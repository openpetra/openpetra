using System;
using System.Windows.Forms;

namespace SourceGrid.Controllers
{
	/// <summary>
	/// A controller used to support clipboard copy, cut and paste.
    /// You can use this controller with a code like this:
    /// grid.GridController.AddController(SourceGrid.Controllers.SelectionClipboard.Default);
	/// </summary>
	public class SelectionClipboard : GridBase
	{
        /// <summary>
        /// A controllers to support copy, cut and paste operations
        /// </summary>
        public static SelectionClipboard Default = new SelectionClipboard(ClipboardMode.All);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pMode"></param>
		public SelectionClipboard(ClipboardMode pMode)
		{
            Mode = pMode;
		}

        private ClipboardMode mMode = ClipboardMode.All;
		/// <summary>
        /// Gets or sets the clipboard mode. Default = ClipboardMode.All
		/// </summary>
        public ClipboardMode Mode
		{
            get { return mMode; }
            set { mMode = value; }
		}

		private bool mExpandSelection = true;

		/// <summary>
		/// Gets or sets if expand the destination selection based on the copied range. If false the destination is only the user selected range.
		/// </summary>
		public bool ExpandSelection
		{
			get{return mExpandSelection;}
			set{mExpandSelection = value;}
		}

		protected override void OnAttach(GridVirtual grid)
		{
			grid.KeyDown += new GridKeyEventHandler(grid_KeyDown);
		}

		protected override void OnDetach(GridVirtual grid)
		{
			grid.KeyDown -= new GridKeyEventHandler(grid_KeyDown);
		}

        private bool CutEnabled
        {
            get { return (Mode & ClipboardMode.Cut) == ClipboardMode.Cut; }
        }
        private bool PasteEnabled
        {
            get { return (Mode & ClipboardMode.Paste) == ClipboardMode.Paste; }
        }
        private bool CopyEnabled
        {
            get { return (Mode & ClipboardMode.Copy) == ClipboardMode.Copy; }
        }

		private void grid_KeyDown(GridVirtual sender, KeyEventArgs e)
		{
			if (e.Handled)
				return;

#warning da fare

            //Range rng = sender.Selection.BorderRange;
            //if (rng.IsEmpty())
            //    return;

            ////Paste
            //if (e.Control && e.KeyCode == Keys.V && PasteEnabled)
            //{
            //    RangeData rngData = RangeData.ClipboardGetData();
			
            //    if (rngData != null)
            //    {
            //        Range destinationRange = rngData.FindDestinationRange(sender, rng.Start);
            //        if (ExpandSelection == false)
            //            destinationRange = destinationRange.Intersect(rng);

            //        rngData.WriteData(sender, destinationRange);
            //        e.Handled = true;
            //        sender.Selection.Clear();
            //        sender.Selection.Add(destinationRange);
            //    }
            //}
            ////Copy
            //else if (e.Control && e.KeyCode == Keys.C && CopyEnabled)
            //{
            //    RangeData data = new RangeData();
            //    data.LoadData(sender, rng, rng.Start, CutMode.None);
            //    RangeData.ClipboardSetData(data);

            //    e.Handled = true;
            //}
            ////Cut
            //else if (e.Control && e.KeyCode == Keys.X && CutEnabled)
            //{
            //    RangeData data = new RangeData();
            //    data.LoadData(sender, rng, rng.Start, CutMode.CutImmediately);
            //    RangeData.ClipboardSetData(data);
				
            //    e.Handled = true;
            //}
		}
	}
}
