using System;
using System.Configuration;
using System.Web;

using UlterSystems.PortalLib;
using ConfirmIt.PortalLib.WebServiceSupport;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.FilesManagers
{
    /// <summary>
    /// Class for providing adding and deleteing operation with files.
    /// </summary>
    public class FileManager
    {
        #region Fileds

        private WebFileService m_service = null;

        #endregion

        #region Constructors

        public FileManager()
        {
            m_service = new WebFileService(ConfigurationManager.AppSettings["FileManagerWebServiceURL"]);
            string domainName = HttpContext.Current.User.Identity.Name.ToLowerInvariant();

            m_service.AuthHeaderValue = new AuthHeader
                                            {
                                                UserDomainName = domainName
                                            };
        }

        public FileManager(int userID)
        {
            m_service = new WebFileService(ConfigurationManager.AppSettings["FileManagerWebServiceURL"]);
            string domainName = Person.GetPersonByID(userID).DomainNames[0];

            m_service.AuthHeaderValue = new AuthHeader
            {
                UserDomainName = domainName
            };
        }

        public FileManager(string serviceFileManagerURL)
        {
            m_service = new WebFileService(serviceFileManagerURL);

            string domainName = HttpContext.Current.User.Identity.Name.ToLowerInvariant();
            m_service.AuthHeaderValue = new AuthHeader
                                            {
                                                UserDomainName = domainName
                                            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Save files.
        /// </summary>
        /// <param name="fileCollection">Collection of posted files.</param>
        public void SaveFilesCollection(HttpFileCollection fileCollection)
        {
            foreach (HttpPostedFile file in fileCollection)
            {
                SaveFile(file);
            }
        }

        /// <summary>
        /// Save file.
        /// </summary>
        /// <param name="file">Posted file.</param>
        public virtual void SaveFile(HttpPostedFile file)
        {
            if (file.FileName == null || String.IsNullOrEmpty(file.FileName))
                return;

            String uniqueName = Guid.NewGuid() + "_" + GetFileName(file.FileName);
            SaveFile(file, uniqueName);
        }

        /// <summary>
        /// Save file.
        /// </summary>
        /// <param name="file">Posted file.</param>
        /// <param name="uniqueFileName">Unique file name.</param>
        protected virtual void SaveFile(HttpPostedFile file, String uniqueFileName)
        {
            if (file.FileName == null 
                || String.IsNullOrEmpty(file.FileName)
                || String.IsNullOrEmpty(uniqueFileName))
                return;

            try
            {
                byte[] buffer = new byte[file.InputStream.Length];
                file.InputStream.Read(buffer, 0, buffer.Length);
                m_service.SaveFile(buffer, uniqueFileName);
            }
            catch (Exception ex)
            {
                Logger.Logger.Instance.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Delete file.
        /// </summary>
        /// <param name="fileName">File name to delete.</param>
        public void DeleteFile(String fileName)
        {
            try
            {
                m_service.DeleteFile(fileName);
            }
            catch (Exception ex)
            {
                Logger.Logger.Instance.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Delete file by URI.
        /// </summary>
        /// <param name="fileURI">File URI to delete.</param>
        public void DeleteFileByURI(String fileURI)
        {
            try
            {
                m_service.DeleteFileByURI(fileURI);
            }
            catch (Exception ex)
            {
                Logger.Logger.Instance.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get url of file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns></returns>
        public string GetFileURL(string fileName)
        {
            try
            {
                return m_service.GetFileURL(fileName);
            }
            catch (Exception ex)
            {
                Logger.Logger.Instance.Error(ex.Message, ex);
                return String.Empty;
            }
        }

        /// <summary>
        /// Fet file properties.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns></returns>
        public FileProperties GetFileProperties(string fileName)
        {
            try
            {
                return m_service.GetFileProperties(fileName);
            }
            catch (Exception ex)
            {
                Logger.Logger.Instance.Error(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Fet file properties.
        /// </summary>
        /// <param name="fileUri">File uri.</param>
        /// <returns></returns>
        public FileProperties GetFilePropertiesByURI(string fileUri)
        {
            try
            {
                return m_service.GetFilePropertiesFromURI(fileUri);
            }
            catch (Exception ex)
            {
                Logger.Logger.Instance.Error(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Return file name.
        /// </summary>
        /// <param name="fullyFileName">Fully file name.</param>
        /// <returns>File name.</returns>
        public String GetFileName(String fullyFileName)
        {
            return fullyFileName.Replace("/", "\\").Substring(fullyFileName.LastIndexOf("\\") + 1);
        }

        #endregion
    }
}