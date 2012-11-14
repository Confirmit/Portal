using System.Diagnostics;

using Core;
using Core.ORM.Attributes;


namespace ConfirmIt.PortalLib.BAL
{
	/// <summary>
	/// Ability class.
	/// </summary>
    [DBTable( "Ability" )]
	public class Ability : BasePlainObject 
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_Name;
		
        #region Properties

        /// <summary>
        /// Name of ability.
        /// </summary>
        [DBRead("Name")]
        public string Name
        {
            [DebuggerStepThrough]
            get { return m_Name; }
            [DebuggerStepThrough]
            set { m_Name = value; }
        }

        #endregion
    }
}
