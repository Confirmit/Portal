using System;
using System.Reflection;
using System.Collections.Generic;

using Castle.DynamicProxy;
using ConfirmIt.PortalLib.BAL.Settings.Interceptors;
using office = UlterSystems.PortalLib.BusinessObjects.Office;

namespace ConfirmIt.PortalLib.BAL.Settings
{
    /// <summary>
    /// Глобальные (в том числе форумные и офисные) настройки
    /// </summary>
    /// В этом классе нужно лишь объявить пустые свойства-настройки с атрибутом Setting.
    /// Всю работу выполняет перехватчик GlobalSettingsInterceptor.
    public class GlobalSettings : BaseSettingCollection
    {
        #region Fields

        private static GlobalSettings m_Instance;

        #endregion

        #region static Instance

        /// <summary>
        /// Экземпляр класса глобальных настроек
        /// </summary>
        public static GlobalSettings Instance
        {
            get
            {
                if (m_Instance != null)
                    return m_Instance;

                ProxyGenerator proxy = new ProxyGenerator();
                GlobalSettingsInterceptor interceptor = new GlobalSettingsInterceptor();
                m_Instance = (GlobalSettings)proxy.CreateClassProxy(typeof(GlobalSettings), interceptor);

                return m_Instance;
            }
        }

        #endregion

        #region Global Settings

        /// <summary>
        /// Адрес SMTP-сервера для отправки сообщений
        /// </summary>
        [Setting(SettingType.Global, "MailServer")]
        public virtual string MailServer
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// Адрес отправителя сообщений
        /// </summary>
        [Setting(SettingType.Global, "MailFromAddress")]
        public virtual string MailFromAddress
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// Тема email-сообщений
        /// </summary>
        [Setting(SettingType.Global, "MailSubject")]
        public virtual string MailSubject
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// Шаблон текста сообщения.
        /// </summary>
        [Setting(SettingType.Global, "MailMessageTemplate")]
        public virtual string MailMessageTemplate
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// Форум обсуждения новостей
        /// </summary>
        [Setting(SettingType.Global, "NewsDiscussForumID")]
        public virtual int NewsDiscussForumID
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// URL-адрес к сервису файлов
        /// </summary>
        [Setting(SettingType.Global, "FileManagerWebServiceURL")]
        public virtual string FileManagerWebServiceURL
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// "Дополнительные" минуты к началу рабочего дня.
        /// </summary>
        [Setting(SettingType.Global, "BonusWorkMinutes")]
        public virtual int BonusWorkMinutes
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Путь к папке-хранилищу книг
        /// </summary>
        [Setting(SettingType.Global, "DownloadBasePath")]
        public virtual string DownloadBasePath
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// Язык книг
        /// </summary>
        [Setting(SettingType.Global, "BooksLanguages")]
        public virtual string BooksLanguages
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// Время, которое необходимое отработать
        /// </summary>
        [Setting(SettingType.Global, "DefaultWorkTime")]
        public virtual TimeSpan DefaultWorkTime
        {
            get { return TimeSpan.Zero; }
            set { }
        }

        /// <summary>
        /// Максимальное время обеденного перерыва
        /// </summary>
        [Setting(SettingType.Global, "MaxLunchTime")]
        public virtual TimeSpan MaxLunchTime
        {
            get { return TimeSpan.Zero; }
            set { }
        }

        #endregion

        #region Forum Settings

        /// <summary>
        /// Путь к файлу "transform.txt", который содержит в себе список замен, используемых на форуме
        /// </summary>
        [Setting(SettingType.Forum, "PathToTransformationFile")]
        public virtual string PathToTransformationFile
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// Разрешить публикацию постов с одинаковым содержанием на различных форумах
        /// </summary>
        [Setting(SettingType.Forum, "AllowDuplicatePosts")]
        public virtual bool AllowDuplicatePosts
        {
            get { return false; }
            set { }
        }

        /// <summary>
        /// Название форума на сайте
        /// </summary>
        [Setting(SettingType.Forum, "SiteName")]
        public virtual string SiteName
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// Вид преобразования даты в строку
        /// </summary>
        [Setting(SettingType.Forum, "DefaultDateFormat")]
        public virtual string DefaultDateFormat
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// Вид преобразования времени в строку
        /// </summary>
        [Setting(SettingType.Forum, "DefaultTimeFormat")]
        public virtual string DefaultTimeFormat
        {
            get { return String.Empty; }
            set { }
        }

        #endregion

        #region Office Settings

        /// <summary>
        /// URL к офису в Ярославле
        /// </summary>
        [Setting(SettingType.Office, "Yaroslavl")]
        public virtual string ServiceURLYaroslavl
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// URL к офису в Москве
        /// </summary>
        [Setting(SettingType.Office, "Moscow")]
        public virtual string ServiceURLMoscow
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// URL к офису в Гилфорде
        /// </summary>
        [Setting(SettingType.Office, "Guilford")]
        public virtual string ServiceURLGuilford
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// URL к офису в Осло
        /// </summary>
        [Setting(SettingType.Office, "Oslo")]
        public virtual string ServiceURLOslo
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// URL к офису в Минске
        /// </summary>
        [Setting(SettingType.Office, "Minsk")]
        public virtual string ServiceURLMinsk
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// URL к офису в Нью-Йорке
        /// </summary>
        [Setting(SettingType.Office, "New York")]
        public virtual string ServiceURLNewYork
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// URL к офису в Сан-Франциско
        /// </summary>
        [Setting(SettingType.Office, "San Francisco")]
        public virtual string ServiceURLSanFrancisco
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// Локальный URL к офису в Ярославле
        /// </summary>
        [Setting(SettingType.Office, "Local Yaroslavl")]
        public virtual string ServiceURLLocalYaroslavl
        {
            get { return String.Empty; }
            set { }
        }

        #endregion

        #region ToList method

        public override IList<ISetting> ToList(SettingType settingType)
        {
            switch (settingType)
            {
                case SettingType.Office:
                    {
                        return ToList<GlobalSettings, office>(settingType).ToArray();
                    }
                case SettingType.Global:
                case SettingType.Forum:
                    {
                        return ToList<GlobalSettings, GlobalSettingEntity>(settingType).ToArray();
                    }
                default:
                    {
                        return new List<ISetting>();
                    }
            }
        }

        #endregion
    }
}
