using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using ConfirmIt.PortalLib.FilesManagers;

using Core;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.NewsTape
{
    /// <summary>
    ///  ����� ���������� �������.
    /// </summary>
    [Serializable]
    [DBTable("NewsAttachments")]
    public class NewsAttachment : BasePlainObject
    {
        #region Fields
        
        private string m_fileName;
        private int m_newsID;

        #endregion

        #region Properties

        /// <summary>
        /// ��� �� ��������� ������� ��� "���������"
        /// </summary>
        public bool IsDeleted
        {
            get { return m_isDeleted; }
            set { m_isDeleted = value; }
        }
        private bool m_isDeleted = false;

        /// <summary>
        /// ������ ��� �����, �������������� � �������, �� �������.
        /// </summary>
        [DBRead("FileName")]
        public string FileName
        {
            get { return m_fileName; }
            set { m_fileName = value; }
        }

        /// <summary>
        /// �������� ��� �����, �������������� � �������, �� �������.
        /// </summary>
        public string ShortFileName
        {
            get
            {
                String fileName = FileName;
                int p = fileName.IndexOf('_');
                fileName = fileName.Substring(p + 1);
                return fileName;
            }
        }

        /// <summary>
        /// ID �������, � ������� ���������� ����.
        /// </summary>
        [DBRead("NewsID")]
        public int NewsID
        {
            get { return m_newsID; }
            set { m_newsID = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete attachment file.
        /// </summary>
        public override void Delete()
        {
            NewsAttachmentManager fileManager = new NewsAttachmentManager();
            fileManager.DeleteAttachFile(this);
            base.Delete();
        }

        #endregion
    }

        /// <summary>
        /// ��������� ����������� � ��������.
        /// </summary>
        [Serializable]
        public class NewsAttachmentCollection : BaseObjectCollection<NewsAttachment>
        {
        }
    }
 