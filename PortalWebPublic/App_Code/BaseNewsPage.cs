using System;

namespace UlterSystems.PortalLib.NewsTape
{
    /// <summary>
    /// ������� ����� ��� ��������� ��������, � ������� ���� ������� ������� � �� �������������.
    /// </summary>

    public class BaseNewsPage : BaseWebPage
    {
        private News m_currentNews;
        /// <summary>
        /// ������� �������.
        /// </summary>
        virtual public News CurrentNews
        {
            get
            {
                if (m_currentNews == null)
                {
                    m_currentNews = new News();
                    if (NewsID.HasValue)
                    {
                        if (!m_currentNews.Load(NewsID.Value))
                            Response.Redirect("AccessDenied.aspx");
                        int[] officeIDs = UlterSystems.PortalLib.NewsManager.NewsManager.GetUserOfficesIDs(CurrentUser.ID.Value);
                        if (Array.IndexOf(officeIDs, m_currentNews.OfficeID) == -1)
                            Response.Redirect("AccessDenied.aspx");
                    }

                }
                return m_currentNews;
            }
            set
            {
                m_currentNews = value;
            }
        }

        /// <summary>
        /// ������������� ������� �������.
        /// </summary>
        virtual public int? NewsID
        {
            get
            {
                int value;
                if (Int32.TryParse(Request.Params["id"], out value))
                {
                    return value;
                }
                else
                {
                    
                    return null;
                }
            }
        }

        //private UlterSystems.PortalLib.BusinessObjects.Person m_currentUser;

        ///// <summary>
        ///// ������� ������������.
        ///// </summary>
        //public UlterSystems.PortalLib.BusinessObjects.Person CurrentUser
        //{
        //    get
        //    {
        //        if (m_currentUser == null)
        //        {
        //            if (Session["UserID"] != null)
        //            {
        //                m_currentUser = new UlterSystems.PortalLib.BusinessObjects.Person();
        //                m_currentUser.Load((int)Session["UserID"]);
        //            }
        //        }
        //        return m_currentUser;
        //    }
        //    set
        //    {
        //        m_currentUser = value;
        //    }
        //}
    }
}
