SourceGrid\SourceGrid\Grids\GridVirtual.cs

		protected override void OnValidated(EventArgs e)
		{
			base.OnValidated(e);

			//NOTE: I use OnValidated and not OnLostFocus because is not called when the focus is on another child control (for example an editor control) or OnLeave because before Validating event and so the validation can still be stopped

			if ((Selection.FocusStyle & FocusStyle.RemoveFocusCellOnLeave) == FocusStyle.RemoveFocusCellOnLeave)
			{
				//Changed by CT - 2012-07-03
				//Selection.Focus(Position.Empty, false);
			}

			if ((Selection.FocusStyle & FocusStyle.RemoveSelectionOnLeave) == FocusStyle.RemoveSelectionOnLeave)
			{
				//Changed by CT - 2012-07-03
				//Selection.ResetSelection(true);
			}
		}
