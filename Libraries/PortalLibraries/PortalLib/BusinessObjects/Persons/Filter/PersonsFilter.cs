using System;
using System.Collections;
using System.Collections.Generic;

using ConfirmIt.PortalLib.FiltersSupport;
using Core;

namespace ConfirmIt.PortalLib.BusinessObjects.Persons.Filter
{
    public class PersonsFilter : IFilter
    {
        #region Fields

        private int m_RoleID = -1;
        private int m_OfficeID = -1;
        private int m_ProjectID = -1;

        #endregion

        #region Properties

        public MLString FirstName { get; set; }
        public MLString LastName { get; set; }

        public List<int> Events { get; set; }

        public int RoleID
        {
            get { return m_RoleID;}
            set { m_RoleID = value; }
        }
        
        public int OfficeID
        {
            get { return m_OfficeID; }
            set { m_OfficeID = value; }
        }
        
        public int ProjectID
        {
            get { return m_ProjectID; }
            set { m_ProjectID = value; }
        }

        #endregion

        #region Method

        public bool IsContainsDataForFiltering()
        {
            if (RoleID == -1 && OfficeID == -1 && ProjectID == -1 && FirstName.RussianValue == "" &&
                FirstName.EnglishValue == ""
                && LastName.RussianValue == "" && LastName.EnglishValue == "" && Events.Count == 0)
                return false;

            return true;
        }

        public bool IsChanged(object filter)
        {
            PersonsFilter pFilter = filter as PersonsFilter;
            if (pFilter == null)
                return false;

            return !(OfficeID == pFilter.OfficeID
                     && RoleID == pFilter.RoleID
                     && ProjectID == pFilter.ProjectID
                     && FirstName.Equals(pFilter.FirstName)
                     && LastName.Equals(pFilter.LastName)
                     && equalsCollections(Events, pFilter.Events));
        }

        private bool equalsCollections(IList fList, IList sList)
        {
            if (fList.Count != sList.Count)
                return false;

            foreach (object obj in fList)
            {
                if (!sList.Contains(obj))
                    return false;
            }
            return true;
        }

        #endregion
    }
}
