using System;
using System.Collections.Generic;
using System.Web;

using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.FilesManagers
{
    public class PhotosAttachmentManager : FileManager
    {
        /// <summary>
        /// Saving new file attachments.
        /// </summary>
        /// <param name="fileCollection">Collection of posted files.</param>
        /// <param name="person"></param>
        public void SavePhotoAttachmentFiles(HttpFileCollection fileCollection, Person person)
        {
            for (int i = 0; i < fileCollection.Count; i++)
            {
                HttpPostedFile file = fileCollection[i];

                if (file.FileName != null && !String.IsNullOrEmpty(file.FileName))
                {
                    String uniqueName = String.Empty;
                    String fileName = String.Empty;

                    fileName = file.FileName.Replace("/", "\\");
                    int index = fileName.LastIndexOf("\\");
                    fileName = fileName.Substring(index + 1);

                    // уникальное имя файла на сервере
                    uniqueName = Guid.NewGuid() + "_" + fileName;

                    //добавляем информацию о фото в БД
                    person.AddStandardStringAttribute(PersonAttributeTypes.Photo, uniqueName);

                    SaveFile(file, uniqueName);
                }
            }
        }

        #region Delete attachments

        /// <summary>
        /// Delete file from DB.
        /// </summary>
        /// <param name="userID">Current user id.</param>
        /// <param name="nameOfPhoto">Name of photo.</param>
        public void DeleteAttachPhoto(int userID, string nameOfPhoto)
        {
            IList<PersonAttribute> userPhoto =
                PersonAttributes.GetPersonAttributesByKeyword(userID, PersonAttributeTypes.Photo.ToString(),
                                                              "StringField", nameOfPhoto);

            foreach (PersonAttribute attribute in userPhoto)
            {
                DeleteFile(nameOfPhoto);
                attribute.Delete();
                return;
            }
        }

        #endregion
    }
}
