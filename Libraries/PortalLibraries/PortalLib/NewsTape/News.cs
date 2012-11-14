using System;

using Core;
using Core.ORM.Attributes;

using UlterSystems.PortalLib.BusinessObjects;
using ConfirmIt.PortalLib.NewsTape;


namespace UlterSystems.PortalLib.NewsTape
{
    /// <summary>
    /// Класс новости.
    /// </summary>
    [DBTable("News")]
    public class News : BasePlainObject
    {
        #region Поля

        private string m_caption = String.Empty;
        private string m_text = String.Empty;
        private int m_authorID = 0;
        private string m_authorName = String.Empty;
        private DateTime m_createTime;
        private DateTime m_expireTime = DateTime.Now;
        private int m_officeID = 0;
        private String m_officeName = String.Empty;
        private int? m_postID = null;
        private NewsAttachmentCollection m_attachments;

        #endregion

        #region Свойства

        /// <summary>
        /// Заголовок новости.
        /// </summary>
        [DBRead("Caption")]
        public string Caption
        {
            get { return m_caption; }
            set { m_caption = value; }
        }
        /// <summary>
        /// Текст новости.
        /// </summary>
        [DBRead("Text")]
        public string Text
        {
            get { return m_text; }
            set { m_text = value; }
        }
        /// <summary>
        /// ID автора новости.
        /// </summary>
        [DBRead("AuthorID")]
        public int AuthorID
        {
            get { return m_authorID; }
            set { m_authorID = value; }
        }
        /// <summary>
        /// Имя и Фамилия автора новости.
        /// </summary>
        public string AuthorName
        {
            get
            {
                if (m_authorName == string.Empty)
                {
                    UlterSystems.PortalLib.BusinessObjects.Person u = new UlterSystems.PortalLib.BusinessObjects.Person();
                    u.Load(m_authorID);
                    m_authorName = u.FirstName.ToString() + " " + u.LastName.ToString();
                }
                return m_authorName;
            }
        }
        /// <summary>
        /// Время добавления новости.
        /// </summary>
        [DBRead("CreateTime")]
        public DateTime CreateTime
        {
            get { return m_createTime; }
            set { m_createTime = value; }
        }

        /// <summary>
        /// Дата устаревания новости.
        /// </summary>
        [DBRead("ExpireTime")]
        public DateTime ExpireTime
        {
            get { return m_expireTime; }
            set { m_expireTime = value; }
        }

        /// <summary>
        /// ID офиса, к которому относится новость.
        /// </summary>
        [DBRead("OfficeID")]
        public int OfficeID
        {
            get { return m_officeID; }
            set { m_officeID = value; }
        }

        /// <summary>
        /// Имя офиса, к которому относится новость.
        /// </summary>

        public String OfficeName
        {
            get
            {
                if (m_officeName == string.Empty)
                {
                    Office office = new Office();
                    office.Load(m_officeID);
                    m_officeName = office.OfficeName;
                }
                return m_officeName;
            }
            set { m_officeName = value; }
        }

        /// <summary>
        /// ID сообщения на форуме, которое соответствует новости.
        /// </summary>
        [DBRead("PostID")]
        [DBNullable]
        public int? PostID
        {
            get { return m_postID; }
            set { m_postID = value; }
        }

        /// <summary>
        /// Коллекция аттачментов к новости.
        /// </summary>
        public NewsAttachmentCollection Attachments
        {
            get
            {
                if (m_attachments == null)
                {
                    if (ID.HasValue)
                        m_attachments = NewsManager.NewsManager.GetNewsAttachments(ID.Value);
                    else
                        m_attachments = new NewsAttachmentCollection();
                }
                return m_attachments;
            }
           
        }
        #endregion

        #region Конструктор

        public News()
        {
        }

        public News(int newsID)
        {
            this.Load(newsID);
        }

        public News(
            string caption,
            string text,
            int authorID,
            DateTime createTime,
            DateTime expTime,
            int officeID
            )
        {
            this.m_caption = caption;
            this.m_text = text;
            this.m_authorID = authorID;
            this.m_createTime = createTime;
            this.m_expireTime = expTime;
            this.m_officeID = 0;
            this.m_officeName = String.Empty;
            this.m_postID = null;

        }
        #endregion

        #region Методы.

        /// <summary>
        /// Сохраняет новость в БД, а также ее аттачменты. 
        /// </summary>
        public override void Save()
        {
            base.Save();

            NewsAttachment[] collection = new NewsAttachment[Attachments.Count];
            Attachments.CopyTo(collection);

            // создание аттачментов
            foreach (NewsAttachment attach in collection)
            {
                attach.NewsID = ID.Value;
                attach.Save();

                if (attach.IsDeleted)
                {
                    try
                    {
                        Attachments.DeleteObject((int) attach.ID);
                        attach.Delete();
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Error(ex.Message, ex);
                    }
                }
            }
        }

        /// <summary>
        /// Delete news with the attachments.
        /// </summary>
        public override void Delete()
        {
            foreach (NewsAttachment attach in Attachments)
            {
                attach.Delete();
            }

            base.Delete();
        }

        #endregion
    }

    /// <summary>
    /// Коллекция новостей.
    /// </summary>
    public class NewsCollection : BaseObjectCollection<News>
    {
    }
}
