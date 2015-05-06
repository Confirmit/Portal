using System.Diagnostics;

using Core;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BAL
{
    /// <summary>
    /// Project class.
    /// </summary>   
    [DBTable("Projects")]
    public class Project : BasePlainObject 
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_Name;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_Description;

        #region Properties

        /// <summary>
        /// Name of Project.
        /// </summary>
        [DBRead("Name")]
        public string Name
        {
            [DebuggerStepThrough]
            get { return m_Name; }
            [DebuggerStepThrough]
            set { m_Name = value; }
        }

        /// <summary>
        /// Description of Project.
        /// </summary>
        [DBNullable]
        [DBRead("Description")]
        public string Description
        {
            [DebuggerStepThrough]
            get { return m_Description; }
            [DebuggerStepThrough]
            set { m_Description = value; }
        }

        #endregion
    }
}