using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Web.Services;
using System.Web.Services.Protocols;
using ConfirmIt.PortalLib.Logger;
using Core.DB;

using UlterSystems.PortalLib;
using UlterSystems.PortalLib.BusinessObjects;
using AuthHeader=UlterSystems.PortalLib.WebServiceSupport.AuthHeader;

namespace WebFileService
{
    /// <summary>
    /// Class for providing file properties like width, height etc.
    /// </summary>
    public class FileProperties
    {
        #region Fields

        private int m_Width = 0;
        private int m_Height = 0;
        private bool m_IsImage = false;
        private String m_fileName = String.Empty;

        #endregion

        #region Constructors

        public FileProperties ()
        {}

        #endregion

        #region Properties

        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        public bool IsImageFile
        {
            get { return m_IsImage; }
            set { m_IsImage = value; }
        }

        public string FileName
        {
            get { return m_fileName; }
            set { m_fileName = value; }
        }

        #endregion
    }

    /// <summary>
    /// Service for working with file store.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebFileService : WebService
    {
        #region Fields
	    
        private AuthHeader m_AuthHeader;
	
        #endregion

        #region Constructors

	/// <summary>
	/// Конструктор.
	/// </summary>
    public WebFileService()
	{
        // Инициализировать логгер.
        log4net.Config.XmlConfigurator.Configure();
        //Logger.Log.Info("WebFileService starting.");

	    // Инициализировать соединение с базой данных.
	    ConnectionManager.ConnectionTypeResolve += ConnectionTypeResolver;
	    ConnectionManager.DefaultConnectionString = ConfigurationManager.ConnectionStrings["DBConnStr"].ConnectionString;
	}

    #endregion

	    #region Методы
	    
        /// <summary>
	    /// Процедура привязки соединения к типу сервера.
	    /// </summary>
	    /// <param name="kind">Тип соединения.</param>
	    /// <returns>Тип сервера.</returns>
        protected ConnectionType ConnectionTypeResolver(ConnectionKind kind)
        {
            return ConnectionType.SQLServer;
        }

        private string getFileName(string fileAbsoluteURI)
        {
            String folderPath = uploadFolderPath;

            if (folderPath.StartsWith("~"))
                folderPath = folderPath.Replace("~/", String.Empty);

            return fileAbsoluteURI.Substring(fileAbsoluteURI.IndexOf(folderPath) + folderPath.Length);
        }

        #endregion

	    #region Properties
	    
        /// <summary>
	    /// Header for authentication.
	    /// </summary>
        public AuthHeader AuthenticationHeader
        {
            get { return m_AuthHeader; }
            set { m_AuthHeader = value; }
        }

        /// <summary>
        /// Get file upload folder virtual path.
        /// </summary>
        private string uploadFolderPath
        {
            get
            {
                return ConfigurationManager.AppSettings["FileUploadFolder"];
            }
        }

        #endregion

        #region Web-Methods

        /// <summary>
        /// Get file properties.
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public FileProperties GetFilePropertiesFromURI(string fileAbsoluteURI)
        {
            return GetFileProperties(getFileName(fileAbsoluteURI));
        }

        /// <summary>
        /// Get file properties.
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public FileProperties GetFileProperties(string fileName)
        {
            String path = Path.Combine(uploadFolderPath, fileName);
            path = Server.MapPath(path);

            FileProperties prop = new FileProperties
                                      {
                                          FileName = fileName
                                      };

            try
            {
                Bitmap map = new Bitmap(path);

                prop.Width = map.Width;
                prop.Height = map.Height;
                prop.IsImageFile = true;

                return prop;
            }
            catch (ArgumentException)
            {
                prop.IsImageFile = false;
                return prop;
            }
        }

        /// <summary>
        /// Get file URL by name.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns></returns>
        [WebMethod]
        public string GetFileURL(string fileName)
        {
            String path = Path.Combine(uploadFolderPath, fileName);
            path = Server.MapPath(path);

            if (!File.Exists(path))
                return String.Empty;

            string filePath = uploadFolderPath.Replace("~/", String.Empty);
            filePath += fileName;

            return Context.Request.Url.Scheme 
                + "://" 
                + Context.Request.Url.Authority 
                + "/"
                + filePath;
        }

        /// <summary>
        /// delete file by name.
        /// </summary>
        /// <param name="fileName">File name.</param>
        [WebMethod]
        [SoapHeader("AuthenticationHeader")]
        public void DeleteFile(string fileName)
        {
            try
            {
                Person currentUser = new Person();
                if (!currentUser.LoadByDomainName(AuthenticationHeader.UserDomainName))
                {
                    Logger.Instance.Error(String.Format("Could not load an user - {0}",
                                                   AuthenticationHeader.UserDomainName));
                    return;
                }

                if (!currentUser.IsInRole("Employee"))
                {
                    Logger.Instance.Error(String.Format("User - {0}, cant delete files",
                               currentUser.FullName));

                    return;
                }

                String path = Path.Combine(uploadFolderPath, fileName);
                path = Server.MapPath(path);

                Logger.Instance.Info(String.Format("User - {0}, delete file - {1}",
                                              currentUser.FullName,
                                              fileName));

                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("DELETE FILE", ex);
            }
        }

        /// <summary>
        /// Delete file by uri.
        /// </summary>
        /// <param name="fileAbsoluteURI">File uri.</param>
        [WebMethod]
        [SoapHeader("AuthenticationHeader")]
        public void DeleteFileByURI(string fileAbsoluteURI)
        {
            DeleteFile(getFileName(fileAbsoluteURI));
        }

        /// <summary>
        /// Save file in upload folder.
        /// </summary>
        /// <param name="file">File stream.</param>
        /// <param name="fileName">File name.</param>
        /// <returns></returns>
        [WebMethod]
        public bool SaveFile(byte[] file, string fileName)
        {
            string path = Path.Combine(uploadFolderPath, fileName);
            path = Server.MapPath(path);
            
            if (!UploadFolderPathIsCorrect())
                return false;

            FileStream fileStream = File.Create(path);

            try
            {
                // Need to checking!
                fileStream.Write(file, 0, file.Length);
                fileStream.Close();

                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("SAVE FILE", ex);
                return false; 
            }
        }

        /// <summary>
        /// Check if folder exist or not. If it doesn't exist - he create it.
        /// </summary>
        /// <returns>Boolean.</returns>
        public bool UploadFolderPathIsCorrect()
        {
            try
            {
                if (!Directory.Exists(Server.MapPath(uploadFolderPath)))
                    Directory.CreateDirectory(Server.MapPath(uploadFolderPath));
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("CREATE UPLOAD FOLDER", ex);
                return false;
            }

            return true;
        }

        #endregion
    }
}
