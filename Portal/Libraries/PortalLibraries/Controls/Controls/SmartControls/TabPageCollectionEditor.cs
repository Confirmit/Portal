using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace SmartControls.Web
{
  
  class TabPageCollectionEditor : CollectionEditor 
  {
    public TabPageCollectionEditor(Type type) : base(type)
    {
    }

    protected override Type[] CreateNewItemTypes()
    {
      return new Type[] { typeof(TabPage) };
    }

    protected override Type CreateCollectionItemType()
    {
      return typeof(TabPage);
    }
  }
}
