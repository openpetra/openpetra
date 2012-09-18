using System;
using System.Windows.Forms;

namespace SourceGrid.Cells.Editors
{
	/// <summary>
	/// The base class for all the editors that have a control
	/// </summary>
	[System.ComponentModel.ToolboxItem(false)]
	public abstract class EditorControlBase : EditorBase
	{
		#region Constructor
		/// <summary>
		/// Construct a Model. Based on the Type specified the constructor populate AllowNull, DefaultValue, TypeConverter, StandardValues, StandardValueExclusive
		/// </summary>
		/// <param name="p_Type">The type of this model</param>
		public EditorControlBase(Type p_Type):base(p_Type)
		{
			mControl = CreateControl();
			if (Control == null)
				throw new SourceGridException("Control cannot be null");
			Control.Hide();
		}
		#endregion

		#region Editor Control
		private Control mControl;
		/// <summary>
		/// The Control used to edit the Cell.
		/// </summary>
		public Control Control
		{
			get{return mControl;}
		}

		private GridVirtual mGrid;
		/// <summary>
		/// The grid used by this editor. Null if the editor is not attached on a grid.
		/// </summary>
		public GridVirtual Grid
		{
			get { return mGrid; }
		}

		/// <summary>
		/// Abstract method that must create a new Control used for the editing operations.
		/// </summary>
		/// <returns></returns>
		protected abstract Control CreateControl();
		/// <summary>
		/// Check if the Control is attached to a grid.
		/// </summary>
		private bool IsControlAttached()
		{
			return mGrid != null;
		}

		private LinkedControlValue mLinkedControl;

		/// <summary>
		/// Add the Control to the specified grid. Consider that a Control can only be a child of one Container.
		/// </summary>
		private void AttachControl(GridVirtual grid)
		{
			mGrid = grid;
			mLinkedControl = new LinkedControlValue(Control, Position.Empty);
			Control.Hide();
			grid.LinkedControls.Add(mLinkedControl);
			Control.Validated += new EventHandler(InnerControl_Validated);
			Control.KeyPress += new KeyPressEventHandler(InnerControl_KeyPress);
		}
		#endregion

		/// <summary>
		/// Start editing the cell passed. Do not call this method for start editing a cell, you must use CellContext.StartEdit.
		/// </summary>
		/// <param name="cellContext">Cell to start edit</param>
		internal override void InternalStartEdit(CellContext cellContext)
		{
			base.InternalStartEdit(cellContext);

			if (Control == null)
				throw new SourceGridException("Control cannot be null");

			if (IsEditing == false && EnableEdit)
			{
				//verifico di non avere ancora una cella associata
				if (EditCell!=null)
					throw new SourceGridException("There is already a Cell in edit state");

				if (IsControlAttached() == false)
					AttachControl(cellContext.Grid);

				mLinkedControl.Position = cellContext.Position;

				//aggiorno la posizione
				cellContext.Grid.ArrangeLinkedControls();

				OnStartingEdit(cellContext, Control);

				//With this method the edit start
				SetEditCell(cellContext);

				//Set the control value
				SafeSetEditValue(cellContext.Cell.Model.ValueModel.GetValue(cellContext));

				//Show the control
				ShowControl(Control);
			}
		}

		/// <summary>
		/// This method is called just before the edit start. You can override this method to customize the editor with the cell informations.
		/// </summary>
		/// <param name="cellContext"></param>
		/// <param name="editorControl"></param>
		protected virtual void OnStartingEdit(CellContext cellContext, Control editorControl)
		{
			//Default properties
			if (UseCellViewProperties)
			{
				editorControl.BackColor = cellContext.Cell.View.BackColor;
				editorControl.ForeColor = cellContext.Cell.View.ForeColor;
				editorControl.Font = cellContext.Cell.View.Font;
			}
		}

		protected virtual void ShowControl(Control editorControl)
		{
			editorControl.Show();
			editorControl.BringToFront();
			editorControl.Focus();
		}

		/// <summary>
		/// Apply edited value without stop the editing.
		/// </summary>
		/// <returns></returns>
		public override bool ApplyEdit()
		{
			//Note: I don't use SetFocusCells on this method because this methed can be called also inside some event of the control like TextChanged.

			bool bSuccess;

			if ( IsEditing )
			{
				try
				{
					bSuccess = SetCellValue(EditCellContext, GetEditedValue());
				}
				catch(Exception err)
				{
					OnEditException(new ExceptionEventArgs(err));
					bSuccess = false;
				}
			}
			else
				bSuccess = true;

			return bSuccess;
		}

		/// <summary>
		/// Variable that indicate if the InternalEndEdit method is already called. Is used because the InternalEndEdit can be called by a the Control_Validated or directly by the user.
		/// </summary>
		private bool mIsInsideEndEdit = false;

		/// <summary>
		/// Terminate the edit action. Do not call this method directly, use the CellContext.EndEdit instead.
		/// </summary>
		/// <param name="cancel">True to cancel the editing and return to normal mode, false to call automatically ApplyEdit and terminate editing</param>
		/// <returns>Returns true if the cell terminate the editing mode</returns>
		internal override bool InternalEndEdit(bool cancel)
		{
			if (IsEditing == false)
				return true;

			if (mIsInsideEndEdit)
				return false;

			mIsInsideEndEdit = true;
			try
			{
				bool bSuccess = true;

				if (cancel)
					UndoEditValue();

				if (Control.ContainsFocus )
				{
					//Change the focus to force a validate of the editor Control
					if (EditCellContext.Grid.Focus() == false)
						bSuccess = false;
				}

				//Apply edit value
				if (bSuccess && cancel == false)
					bSuccess = ApplyEdit();

				if (bSuccess)
				{
					HideControl();
					mLinkedControl.Position = Position.Empty;

					//di fatto mettendo questa property a null termina logicamente l'edit
					SetEditCell(CellContext.Empty);
				}
				else //editing failed
				{
					if (Control.ContainsFocus == false)
						Control.Focus();
				}

				return bSuccess;
			}
			finally
			{
				mIsInsideEndEdit = false;
			}
		}
		
		private void HideControl()
		{
			try
			{
				// invoke "Hide", method with some reflection stuff.
				// See test SourceGrid.Tests.net_2_0.TestOverride_With_New
				// why this is necessary
				Control.GetType().GetMethod("Hide").Invoke(Control, null);
			}
			catch
			{
				// if something goes wrong, just call Hide normally
				Control.Hide();
			}
		}

		/// <summary>
		/// Validated of the editor control
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void InnerControl_Validated(object sender, EventArgs e)
		{
			try
			{
				if (IsEditing)
					EditCellContext.EndEdit(false);
			}
			catch(Exception err)
			{
				OnEditException(new ExceptionEventArgs(err));
			}
		}

		private void InnerControl_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.Handled)
				return;

			OnKeyPress(e);
		}

		/// <summary>
		/// Undo the edit value of the control to the initial value of the cell, usually used when pressing Esc key or terminate the edit operation with Cancel true.
		/// </summary>
		public virtual void UndoEditValue()
		{
			if (EditCell == null)
				throw new SourceGridException("Not in edit state");

			SafeSetEditValue(EditCell.Model.ValueModel.GetValue(EditCellContext));
		}

		/// <summary>
		/// Set the specified value in the current editor control calling the SetEditValue method. If an exception is throwed calls the OnUserException method and set the default value.
		/// </summary>
		/// <param name="editValue"></param>
		public void SafeSetEditValue(object editValue)
		{
			try
			{
				//Can throw an exception if the original value is not valid for the editor
				SetEditValue(editValue);
			}
			catch(Exception ex)
			{
				EditCellContext.Grid.OnUserException( new ExceptionEventArgs( new EditingCellException( ex ) ) );
				//On exception try to put the default value
				SetEditValue(DefaultValue);
			}
		}

		/// <summary>
		/// Returns the value inserted with the current editor control
		/// </summary>
		/// <returns></returns>
		public override abstract object GetEditedValue();

		/// <summary>
		/// Set the specified value in the current editor control.
		/// </summary>
		/// <param name="editValue"></param>
		public abstract void SetEditValue(object editValue);

		/// <summary>
		/// KeyPress event is fired in 2 cases: when the Control.KeyPress event is executed and when the user start to editing the cell by pressing a key (calling SendCharToEditor)
		/// Set the e.Handled property to true to block the event
		/// </summary>
		public event KeyPressEventHandler KeyPress;
		protected virtual void OnKeyPress(KeyPressEventArgs e)
		{
			if (e.Handled)
				return;

			if (KeyPress != null)
				KeyPress(this, e);
		}

		#region Send Keys
		/// <summary>
		/// Used to send some keyboard keys to the active editor. It is only valid when there is an active edit operations.
		/// </summary>
		/// <param name="key"></param>
		public override sealed void SendCharToEditor(char key)
		{
			//if (IsEditing && Control.ContainsFocus)
			//{
			//    //DevAge.Windows.Forms.SendCharExact.Send(key); //Doesn't work well on french keyboard or in response to CAPS LOCK/shift keys combination
			//}

			KeyPressEventArgs arg = new KeyPressEventArgs(key);
			OnKeyPress(arg);

			if (arg.Handled)
				return;

			OnSendCharToEditor(arg.KeyChar);
		}

		/// <summary>
		/// Method used to precess the key sended.
		/// Abstract method that must be override in the derived class
		/// </summary>
		/// <param name="key"></param>
		protected abstract void OnSendCharToEditor(char key);
		#endregion

		#region Minimum Size
		/// <summary>
		/// Calculate the minimum required size for the specified editor cell.
		/// </summary>
		/// <param name="cellContext"></param>
		/// <returns></returns>
		public override System.Drawing.Size GetMinimumSize(CellContext cellContext)
		{
			return Control.GetPreferredSize(System.Drawing.Size.Empty);
		}
		#endregion
	}
}