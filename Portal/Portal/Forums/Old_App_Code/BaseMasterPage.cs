using System;
using System.Web.UI;

/// <summary>
/// Summary description for BaseMasterPage
/// </summary>
public abstract class BaseMasterPage : MasterPage
{
    public abstract ScriptManager ScriptManager { get; }
}
