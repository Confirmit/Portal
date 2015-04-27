using System;
using System.Web.UI;
using System.ComponentModel;
using System.Drawing.Design;

namespace SmartControls.Web
{
  //[Editor(typeof(TabPageCollectionEditor) , typeof(UITypeEditor))]
  public class TabPageCollection :  ControlCollection
  {
    public TabPageCollection(TabsView owner) : base(owner)
    {
    }
        

    private void VerifyChild(Control ctrl)
    {
      if (ctrl is TabPage)
      {
        return;
      }

      throw new Exception("Invalid Child Object");
    }

    public override void Add(Control child)
    {
      VerifyChild(child);
      base.Add(child);
    }

    public override void AddAt(int index, Control child)
    {
      VerifyChild(child);
      base.AddAt(index, child);


    }

    public override void Remove(Control value)
    {
      base.Remove(value);
    }

    public override void RemoveAt(int index)
    {
      base.RemoveAt(index);
    }

    public override void Clear()
    {
      base.Clear();
    }
  }  
}
