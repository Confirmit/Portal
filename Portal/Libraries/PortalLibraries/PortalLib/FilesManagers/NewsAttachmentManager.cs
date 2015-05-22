using System;
using System.Web;

using ConfirmIt.PortalLib.NewsTape;
using UlterSystems.PortalLib.NewsTape;

namespace ConfirmIt.PortalLib.FilesManagers
{
    public class NewsAttachmentManager : FileManager
    {
        /// <summary>
        /// Saving new file attachments.
        /// </summary>
        /// <param name="fileCollection">Collection of posted files.</param>
        /// <param name="currentNews">News for eorking.</param>
        public void SaveNewsAttachmentFiles(HttpFileCollection fileCollection, News currentNews)
        {
            for (int i = 0; i < fileCollection.Count; i++)
            {
                HttpPostedFile file = fileCollection[i];

                if (file.FileName == null ||
                    String.IsNullOrEmpty(file.FileName))
                    continue;

                String uniqueName = Guid.NewGuid() + "_" + GetFileName(file.FileName);

                // добавляем неприкрепленный аттачмент в БД
                NewsAttachment attach = new NewsAttachment();
                attach.FileName = uniqueName;

                SaveFile(file, uniqueName);
                currentNews.Attachments.Add(attach);
            }
        }

        #region Delete attachments

        /// <summary>
        /// Delete marked files from attachment collection.
        /// </summary>
        /// <param name="collection">NewsAttachmentCollection.</param>
        public void DeleteAttachFiles(NewsAttachmentCollection collection)
        {
            foreach (NewsAttachment attach in collection)
            {
                if (attach.IsDeleted)
                    DeleteAttachFile(attach);
            }
        }

        /// <summary>
        /// Delete attach file.
        /// </summary>
        /// <param name="attach">File to delete.</param>
        public void DeleteAttachFile(NewsAttachment attach)
        {
            DeleteFile(attach.FileName);
        }

        #endregion
    }
}
