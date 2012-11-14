﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Core.Resources.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Произошла ошибка в Active directory, во время попытки получения пользователя из группы &apos;{0}&apos;. Возможно группа с таким именем не существует..
        /// </summary>
        internal static string ADException {
            get {
                return ResourceManager.GetString("ADException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Параметр не может быть пустым..
        /// </summary>
        internal static string ArgumentNullException {
            get {
                return ResourceManager.GetString("ArgumentNullException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Параметр &apos;{0}&apos; не может быть пустым..
        /// </summary>
        internal static string ArgumentNullWithParamException {
            get {
                return ResourceManager.GetString("ArgumentNullWithParamException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не удалось закрыть соединение с сообщением &apos;{0}&apos;..
        /// </summary>
        internal static string CloseConnectionException {
            get {
                return ResourceManager.GetString("CloseConnectionException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Колонка {0} не найдена в справочнике &apos;{1}&apos;..
        /// </summary>
        internal static string ColumnNotFoundException {
            get {
                return ResourceManager.GetString("ColumnNotFoundException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Свойство {0} не может иметь пустое значение. Используйте DBNullable attribute..
        /// </summary>
        internal static string DBNullableAttributeException {
            get {
                return ResourceManager.GetString("DBNullableAttributeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DBRead аттрибут не найден для свойства &apos;{0}&apos; типа &apos;{1}&apos;..
        /// </summary>
        internal static string DBReadAttributeException {
            get {
                return ResourceManager.GetString("DBReadAttributeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Аттрибут DBTable должен быть определен для типа &apos;{0}&apos;..
        /// </summary>
        internal static string DBTableAttributeException {
            get {
                return ResourceManager.GetString("DBTableAttributeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Справочник &apos;{0}&apos; не может быть импортирован, т.к. он не помечен как импортируемый..
        /// </summary>
        internal static string DictionaryCantBeImportedException {
            get {
                return ResourceManager.GetString("DictionaryCantBeImportedException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Элемент справочника &apos;{0}&apos; с ключом {1} не может закрыт, так как связан с элементом  справочника &apos;{2}&apos; с ключом {3}..
        /// </summary>
        internal static string DictionaryElementCouldNotBeClosedException {
            get {
                return ResourceManager.GetString("DictionaryElementCouldNotBeClosedException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Элемент справочника &apos;{0}&apos; с ключом {1} не может удален, так как связан с элементом  справочника &apos;{2}&apos; с ключом {3}..
        /// </summary>
        internal static string DictionaryElementCouldNotBeDeletedException {
            get {
                return ResourceManager.GetString("DictionaryElementCouldNotBeDeletedException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ошибка аутентификации пользователя. {0}.
        /// </summary>
        internal static string ErrorAuthentificatingUserException {
            get {
                return ResourceManager.GetString("ErrorAuthentificatingUserException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ошибка поиска группы пользователей в ActiveDirecory..
        /// </summary>
        internal static string ErrorSearchingGroupException {
            get {
                return ResourceManager.GetString("ErrorSearchingGroupException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ошибка поиска пользователя в ActiveDirecory..
        /// </summary>
        internal static string ErrorSearchingUserException {
            get {
                return ResourceManager.GetString("ErrorSearchingUserException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Неверный параметр запроса {0}..
        /// </summary>
        internal static string InvalidRequestParamException {
            get {
                return ResourceManager.GetString("InvalidRequestParamException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Элемент с ключём {0} уже существует в справочнике &apos;{1}&apos;..
        /// </summary>
        internal static string KeyAlreadyExistsException {
            get {
                return ResourceManager.GetString("KeyAlreadyExistsException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ключ {0} не найден в словаре &apos;{1}&apos;..
        /// </summary>
        internal static string KeyNotFoundException {
            get {
                return ResourceManager.GetString("KeyNotFoundException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не найдена связь объекта {1} справочника &apos;{0}&apos; c объектами справочника &apos;{2}&apos; по ключу {3}..
        /// </summary>
        internal static string LinkNotFoundException {
            get {
                return ResourceManager.GetString("LinkNotFoundException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Метод {0} не найден в иерархии классов..
        /// </summary>
        internal static string MethodException {
            get {
                return ResourceManager.GetString("MethodException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Метод не поддерживается..
        /// </summary>
        internal static string NotSupportedException {
            get {
                return ResourceManager.GetString("NotSupportedException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Объект {0} не найден..
        /// </summary>
        internal static string ObjectNotFoundException {
            get {
                return ResourceManager.GetString("ObjectNotFoundException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Обьекты не могут быть скопированы. У них нет нужных полей..
        /// </summary>
        internal static string ObjectsCopyException {
            get {
                return ResourceManager.GetString("ObjectsCopyException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Обьекты не могут быть загружены из базы данных. У них нет нужных полей..
        /// </summary>
        internal static string ObjectsException {
            get {
                return ResourceManager.GetString("ObjectsException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Обьекты не могут быть добавлены в базу данных. У них нет нужных полей..
        /// </summary>
        internal static string ObjectsInsertException {
            get {
                return ResourceManager.GetString("ObjectsInsertException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to У обьектов разные типы..
        /// </summary>
        internal static string ObjectsTypesException {
            get {
                return ResourceManager.GetString("ObjectsTypesException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Обьекты не могут быть обновлены в базе данных. У них нет нужных полей..
        /// </summary>
        internal static string ObjectsUpdateException {
            get {
                return ResourceManager.GetString("ObjectsUpdateException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не удалось открыть соединение с сообщением &apos;{0}&apos;..
        /// </summary>
        internal static string OpenConnectionException {
            get {
                return ResourceManager.GetString("OpenConnectionException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Метод упаковки не был указан..
        /// </summary>
        internal static string PackingException {
            get {
                return ResourceManager.GetString("PackingException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не удалось создать параллельную транзакцию..
        /// </summary>
        internal static string ParallelTransactionException {
            get {
                return ResourceManager.GetString("ParallelTransactionException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to У свойства &apos;{0}&apos; нет аттрибута DBRead..
        /// </summary>
        internal static string PropertyDBReadAttributeException {
            get {
                return ResourceManager.GetString("PropertyDBReadAttributeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Класс &apos;{0}&apos; не содержит свойства &apos;{1}&apos;..
        /// </summary>
        internal static string PropertyException {
            get {
                return ResourceManager.GetString("PropertyException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не удалось найти свойство &apos;{0}&apos;..
        /// </summary>
        internal static string PropertyNotFoundException {
            get {
                return ResourceManager.GetString("PropertyNotFoundException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Свойство &apos;{0}&apos; не найдено в типе &apos;{1}&apos;..
        /// </summary>
        internal static string PropertyNotFoundInTypeException {
            get {
                return ResourceManager.GetString("PropertyNotFoundInTypeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Значение свойства &apos;{0}&apos; нулл..
        /// </summary>
        internal static string PropertyNullValueException {
            get {
                return ResourceManager.GetString("PropertyNullValueException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В конфигурации для роли {0} не предусмотрена соответствующая группа..
        /// </summary>
        internal static string RoleException {
            get {
                return ResourceManager.GetString("RoleException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Метод сериализации не был указан..
        /// </summary>
        internal static string SerializationException {
            get {
                return ResourceManager.GetString("SerializationException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не найдено свойство настроек &apos;{0}&apos;..
        /// </summary>
        internal static string SettingsPropertyNotFoundException {
            get {
                return ResourceManager.GetString("SettingsPropertyNotFoundException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Число задач процессора (количество потоков) не может быть меньше или равно 0..
        /// </summary>
        internal static string ThreadException {
            get {
                return ResourceManager.GetString("ThreadException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Класс &apos;{0}&apos; не содержит DBTable аттрибут..
        /// </summary>
        internal static string TypeDBTableException {
            get {
                return ResourceManager.GetString("TypeDBTableException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to У типа &apos;(0)&apos; нет свойства &apos;{1}&apos;..
        /// </summary>
        internal static string TypeException {
            get {
                return ResourceManager.GetString("TypeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не удалось распознать название колонки {0}  в словаре &apos;{1}&apos;..
        /// </summary>
        internal static string WrongColumnNameException {
            get {
                return ResourceManager.GetString("WrongColumnNameException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Размер массива значений ключей не совпадает с размером массива ключей при загрузке справочника &apos;{0}&apos;..
        /// </summary>
        internal static string WrongKeyValueListException {
            get {
                return ResourceManager.GetString("WrongKeyValueListException", resourceCulture);
            }
        }
    }
}
