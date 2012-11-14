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
        #region ������

        /// <summary>
        /// ���������� ID ���� ������ ��� ������� ������������, ������� 0, ��������������� ����� ��������.
        /// </summary>
        /// <param name="personID">ID ������������</param>
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
        /// ���������� ID ���� ������ ��� ������� ������������, ������� 0, ��������������� ����� ��������.
        /// </summary>
        /// <param name="personID">ID ������������</param>
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
        /// ���������� ��������� ���������� �������� ��� ��������� ������������.
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
        /// ���������� ��������� ������ �������� �� ������ ��� ��������� ������������.
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
        /// ���������� ������� "�����������" �����.
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
        /// �������� ������ ������� �� ������������ ��������� ��������.
        /// </summary>
        /// <param name="news">�������</param>
        /// <param name="notAllowedTag">First not allowed tag.</param>
        /// <returns>true - ����� ���������, false - ����� �� ���������</returns>
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
            //������� ������� ����������� ����� �� ��
            ArrayList arrList = GetAllowTags();

            notAllowedTag = String.Empty;
            if (rows.Length == 1)                               //����� ���
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
                        if (arrList.IndexOf(tag.ToLower()) == -1)                //�������� �� ��� ����������
                        {                                        //���
                            notAllowedTag = tag;
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// ����� ��������.
        /// </summary>
        /// <param name="args"> ��������� ���������</param>
        /// <param name="strSearchTerms"> ����� ��� ������</param>
        /// <param name="iSearchAuthorID">ID ������</param>
        /// <param name="iNewsStatus">������ �������</param>
        /// <param name="personID">ID ������������,���������������� ��������</param>
        /// <param name="officeID">ID �����</param>
        /// <param name="iPeriod">������ ��� ������</param>
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
        /// ���������� ������, � ������� ��� ����� �������� ������ �������� ����� BR.
        /// </summary>
        /// <param name="str">������� ������.</param>
        /// <returns>������, � ������� ��� ����� �������� ������ �������� ����� BR.</returns>
        public static string TextFormatting(string str)
        {
            String result = String.Empty;
            string[] stringSeparators = new string[] { "\r\n" };
            string[] rows = str.Split(stringSeparators, StringSplitOptions.None);
            for (int i = 0; i < rows.Length - 1; i++)
            {
                int l1 = rows[i].Length - 1;
                int l2 = 0;
                if ((l1 != -1) && (rows[i][l1] == '>') && (rows[i + 1][l2] == '<'))      //������� ������� ������ ����� ������
                {
                    result += rows[i];
                }
                else                                                   //�������� �������� ������ � ������ �� ���� <BR>
                {
                    result += rows[i];
                    result += "<BR>";
                }
            }
            result += rows[rows.Length - 1];
            return result;
        }

        /// <summary>
        /// ���������� ������ ����������� ��� ������ �������.
        /// </summary>
        /// <param name="NewsID">ID �������.</param>
        /// <returns></returns>
        public static NewsAttachmentCollection GetNewsAttachments(int newsID)
        {
            NewsAttachmentCollection coll = new NewsAttachmentCollection();
            coll.FillFromDataSet(DBManager.GetNewsAttachments(newsID));
            return coll;
        }

        /// <summary>
        /// ������� ��������������� ����� � ������� � �� ��.
        /// </summary>
        public static void CleanAttachments()
        {
            NewsAttachmentManager fileManager = new NewsAttachmentManager();
            NewsAttachmentCollection coll = new NewsAttachmentCollection();

            coll.FillFromDataSet(DBManager.GetUnnecessaryAttachments());

            // ������� � ������� ����
            foreach (NewsAttachment attach in coll)
            {
                fileManager.DeleteAttachFile(attach);
            }

            // ������� �������� ���������� �� ��
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
