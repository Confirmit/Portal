using System.Windows;
using System.Windows.Controls;

namespace SilverlightMenu
{
	public class MenuVisualStateManager : VisualStateManager
	{
		protected override bool GoToStateCore(Control control, FrameworkElement templateRoot, string stateName, VisualStateGroup group, VisualState state, bool useTransitions)
		{
			MenuItem mi = (MenuItem)control;
			if (mi.IsEnabled == false)
			{
				//force the control to have a disabled appearence
				stateName = "Disabled";
				if (group != null)
					for (int i = 0; i < group.States.Count; i++)
						if ((group.States[i] is VisualState) && (((VisualState)(group.States[i])).Name == stateName))
							state = group.States[i] as VisualState;
			}
			if (state != null)
				return base.GoToStateCore(control, templateRoot, stateName, group, state, useTransitions);

		    return true;
		}
	}
}
