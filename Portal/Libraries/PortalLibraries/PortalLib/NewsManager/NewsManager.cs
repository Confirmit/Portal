using System;
using System.IO;
using System.Web;
using System.Collections;
using System.Data;
using ConfirmIt.PortalLib.FilesManagers;
using Core;

using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.DB;
using UlterSystems.PortalLib.NewsTape;

using ConfirmIt.PortalLib.NewsTape;

namespace UlterSystems.PortalLib.NewsManager
{
    public class NewsManager
    {
        #region Методы

        /// <summary>
        /// Возвращает ID всех офисов для данного пользователя, включая 0, соответствующий общим новостям.
        /// </summary>
        /// <param name="personID">ID пользователя</param>
        /// <returns></returns>
        public static int[] GetUserOfficesIDs(int personID)
        {
            Office[] offices = Office.GetUserOffices(personID);
            int i = 0;
            int[] officeIDs = new int[offices.Length + 1];
            officeIDs[i++] = 0;
            foreach (Office office in offices)
            {
                officeIDs[i++] = office.ID.Value;
            }
            return officeIDs;
        }

        /// <summary>
        /// Возвращает ID всех офисов для данного пользователя, включая 0, соответствующий общим новостям.
        /// </summary>
        /// <param name="personID">ID пользователя</param>
        /// <returns></returns>
        public static string[] GetUserOfficesStringIDs(int? personID)
        {
            if (personID == null)
                return new string[] {"0"};

            Office[] offices = Office.GetUserOffices((int)personID);
            int i = 0;
            string[] officeIDs = new string[offices.Length + 1];
            officeIDs[i++] = "0";

            foreach (Office office in offices)
            {
                officeIDs[i++] = office.ID.ToString();
            }

            return officeIDs;
        }

        /// <summary>
        /// Возвращает коллекцию актуальных новостей для заданного пользователя.
        /// </summary>
        /// <param name="personID"></param>
        /// <returns></returns>
        public static NewsCollection GetActualNews(int personID)
        {
            NewsCollection fullColl = new NewsCollection();
            string[] offices = GetUserOfficesStringIDs(personID);
            fullColl.FillFromDataSet(DBManager.GetActualNews(offices));

            return fullColl;
        }

        /// <summary>
        /// Возвращает коллекцию полных новостей из архива для заданного пользователя.
        /// </summary>
        /// <returns></returns>

        public static PagingResult GetArchiveNews(PagingArgs args, int? personID)
        {
            NewsCollection coll = new NewsCollection();
            string[] offices = GetUserOfficesStringIDs(personID);
            int total_count;
            coll.FillFromDataSet(DBManager.GetArchiveNews(args, out total_count, offices));
            return new PagingResult(coll, total_count);
        }


        /// <summary>
        /// Возвращает таблицу "разрешенных" тегов.
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetAllowTags()
        {
            DataTable dt = DBManager.GetAllowTags();
            ArrayList arrList = new ArrayList();
            int rowscount = dt.Rows.Count;
            for (int i = 0; i < rowscount; i++)
            {
                arrList.Add(dt.Rows[i].ItemArray[0].ToString());
            }
            return arrList;
        }

        /// <summary>
        /// Проверка текста новости на безопасность введенной разметки.
        /// </summary>
        /// <param name="news">Новость</param>
        /// <param name="notAllowedTag">First not allowed tag.</param>
        /// <returns>true - текст безопасен, false - текст не безопасен</returns>
        public static bool SecurityCheckNews(News news, out string notAllowedTag)
        {
            string str = news.Caption;
            if (str.IndexOf('<') != -1)
            {
                notAllowedTag = String.Empty;
                return false;
            }

            str = news.Text;
            string[] rows = str.Split('<');
            //достать таблицу разрешенных тегов из БД
            ArrayList arrList = GetAllowTags();

            notAllowedTag = String.Empty;
            if (rows.Length == 1)                               //тегов нет
                return true;

            for (int i = 1; i < rows.Length; i++)
            {
                if (rows[i] != String.Empty)
                {
                    int p = -1;
                    int indexClose = rows[i].IndexOf('>');
                    int indexGap = rows[i].IndexOf(' ');
                    if (indexClose != -1)
                    {
                        if (indexGap != -1)
                        {
                            p = Math.Min(indexClose, indexGap);
                        }
                        else
                        {
                            p = indexClose;
                        }
                    }
                    else
                    {
                        if (indexGap != -1)
                        {
                            p = indexGap;
                        }
                        else
                        {
                            p = -1;
                        }
                    }

                    if (p != -1)
                    {
                        string tag = rows[i].Substring(0, p);
                        if (arrList.IndexOf(tag.ToLower()) == -1)                //является ли тег допустимым
                        {                                        //нет
                            notAllowedTag = tag;
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Поиск новостей.
        /// </summary>
        /// <param name="args"> аргументы пэйджинга</param>
        /// <param name="strSearchTerms"> слова для поиска</param>
        /// <param name="iSearchAuthorID">ID автора</param>
        /// <param name="iNewsStatus">Статус новости</param>
        /// <param name="personID">ID пользователя,просматривающего страницу</param>
        /// <param name="officeID">ID офиса</param>
        /// <param name="iPeriod">период для поиска</param>
        /// <returns></returns>
        public static PagingResult SearchNews(PagingArgs args,
                                                String searchTerms,
                                                int searchAuthorID,
                                                UlterSystems.PortalLib.DB.DBManager.NewsStatus newsStatus,
                                                int personID,
                                                int officeID,
                                                UlterSystems.PortalLib.DB.DBManager.SearchPeriod period
                                                )
        {
            NewsCollection coll = new NewsCollection();
            int total_count;
            string[] offices = GetUserOfficesStringIDs(personID);
            coll.FillFromDataSet(DBManager.SearchNews(args, out total_count, searchTerms, searchAuthorID, newsStatus, officeID, offices, period));
            return new PagingResult(coll, total_count);
        }

        /// <summary>
        /// Возвращает строку, в которой все знаки переноса строки заменены тегом BR.
        /// </summary>
        /// <param name="str">Входная строка.</param>
        /// <returns>Строка, в которой все знаки переноса строки заменены тегом BR.</returns>
        public static string TextFormatting(string str)
        {
            String result = String.Empty;
            string[] stringSeparators = new string[] { "\r\n" };
            string[] rows = str.Split(stringSeparators, StringSplitOptions.None);
            for (int i = 0; i < rows.Length - 1; i++)
            {
                int l1 = rows[i].Length - 1;
                int l2 = 0;
                if ((l1 != -1) && (rows[i][l1] == '>') && (rows[i + 1][l2] == '<'))      //удалить перевод строки между тегами
                {
                    result += rows[i];
                }
                else                                                   //заменить переводы строки в тексте на теги <BR>
                {
                    result += rows[i];
                    result += "<BR>";
                }
            }
            result += rows[rows.Length - 1];
            return result;
        }

        /// <summary>
        /// Возвращает список аттачментов для данной новости.
        /// </summary>
        /// <param name="NewsID">ID новости.</param>
        /// <returns></returns>
        public static NewsAttachmentCollection GetNewsAttachments(int newsID)
        {
            NewsAttachmentCollection coll = new NewsAttachmentCollection();
            coll.FillFromDataSet(DBManager.GetNewsAttachments(newsID));
            return coll;
        }

        /// <summary>
        /// Удаляет неприкрепленные файлы с сервера и из БД.
        /// </summary>
        public static void CleanAttachments()
        {
            NewsAttachmentManager fileManager = new NewsAttachmentManager();
            NewsAttachmentCollection coll = new NewsAttachmentCollection();

            coll.FillFromDataSet(DBManager.GetUnnecessaryAttachments());

            // удалить с сервера файл
            foreach (NewsAttachment attach in coll)
            {
                fileManager.DeleteAttachFile(attach);
            }

            // удалить ненужные аттачменты из БД
            DBManager.CleanAttachments();
        }

        /// <summary>
        /// Send news to archive.
        /// </summary>
        /// <param name="newsID">News identificator.</param>
        public static void SendNewsToArchive(int newsID)
        {
            News news = new News(newsID);

            DateTime expDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
            news.ExpireTime = expDateTime;
            news.Save();
        }

        #endregion
    }
}
