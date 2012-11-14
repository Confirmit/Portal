using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

using Core;
using Core.ORM.Attributes;

using UlterSystems.PortalLib.BusinessObjects;

using ConfirmIt.PortalLib;
using ConfirmIt.PortalLib.DAL;
using Confirmit.PortalLib.BusinessObjects.RequestObjects.Filters;
using ConfirmIt.PortalLib.DAL.SqlClient;

namespace Confirmit.PortalLib.BusinessObjects.RequestObjects
{
    /// <summary>
    /// Disk class.
    /// </summary>
    [DBTable("Disks", true)]
    public class Disk : RequestObject
    {
        #region Properties

        /// <summary>
        /// Manufacturer of Disk.
        /// </summary>
        [DBRead("Manufacturers")]
        public string Manufacturers { get; set; }

        /// <summary>
        /// Publishing year of Disk.
        /// </summary>
        [DBRead("PublishingYear")]
        public int PublishingYear { get; set; }

        /// <summary>
        /// Annotation of Disk.
        /// </summary>
        [DBRead("Annotation")]
        public string Annotation { get; set; }

        #endregion

        #region Methods

        public override IList<RequestObject> GetAllRequestObjects()
        {
            return ((BaseObjectCollection<Disk>)Disk.GetObjects(typeof(Disk))).ToArray();
        }
        
        #endregion
    }
}