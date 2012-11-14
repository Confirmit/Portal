using System;
using System.Collections.Generic;
using System.Reflection;

using Core;
using UlterSystems.PortalLib.BusinessObjects;
using Core.ORM.Attributes;

namespace Confirmit.PortalLib.BusinessObjects.RequestObjects
{
    public static class RequestObjectType
    {
        public enum ObjectType
        {
            Book = 0,
            Disk = 1,
            Card = 2
        }

        public static Dictionary<ObjectType, Type> ObjectTypes = new Dictionary<ObjectType, Type> 
        { 
            {ObjectType.Book, typeof(Book)} ,
            {ObjectType.Disk, typeof(Disk)} ,
            {ObjectType.Card, typeof(Card)} 
        };
    }

    [DBTable("RequestObject")]
    public abstract class RequestObject: BasePlainObject
    {
        #region Fields

        private string m_OwnerName = string.Empty;
        private string m_OfficeName = string.Empty;

        #endregion

        #region Properties

        [DBRead("Title")]
        public string Title { get; set; }

        [DBRead("OfficeID")]
        public int OfficeID { get; set; }

        [DBNullable]
        [DBRead("OwnerID")]
        public int? OwnerID { get; set; }

        public string OfficeName
        {
            get
            {
                if (!string.IsNullOrEmpty(m_OfficeName))
                    return m_OfficeName;

                ConfirmIt.PortalLib.BAL.Office office = ConfirmIt.PortalLib.BAL.Office.GetOfficeByID(OfficeID);
                if (office == null)
                    return string.Empty;

                m_OfficeName = office.OfficeName;

                return m_OfficeName;
            }
        }

        public string OwnerName
        {
            get
            {
                if (!string.IsNullOrEmpty(m_OwnerName))
                    return m_OwnerName;

                m_OwnerName = OwnerID != null ? Person.GetPersonByID(OwnerID.Value).FullName : OfficeName;

                return m_OwnerName;
            }
        }

        #endregion

        public abstract IList<RequestObject> GetAllRequestObjects();

        public static IList<RequestObject> GetAllRequestObjects(RequestObjectType.ObjectType objectType)
        {
            Type objType = RequestObjectType.ObjectTypes[objectType];
            ConstructorInfo constructor = objType.GetConstructor(new Type[] { });
            if (constructor == null)
                throw new Exception(string.Format("Cannot found empty constructor of type: {0}", objType.ToString()));

            RequestObject reqObject = (RequestObject)constructor.Invoke(null);
            return reqObject.GetAllRequestObjects();
        }
    }
}