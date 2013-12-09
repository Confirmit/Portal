using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

using Core.ORM;
using Core.DB;
using Core.Exceptions;
using Core.Resources;
using Core.ORM.Attributes;
using Core.DB.QueryStatement;

namespace Core
{
    /// <summary>
    /// Базовый класс для простых (т.е. без иерархии, сопоставляемых одной записи в одной 
    /// независимой таблице базы данных) объектов бизнес-логики.
    /// </summary>
    public abstract class BasePlainObject : BaseObject
    {
        #region [ Загрузка / сохранение / удаление объекта ]

        #region [ Загрузка объекта ]

        /// <summary>
        /// Загружает объект по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Возвращает true, если объект был найден в БД, иначе false.</returns>
        public virtual bool Load(int id)
        {
            IBaseCommand command = GetLoadByIDCommand(id);
            DataRow row = command.ExecDataRow();
            if (row == null)
                return false;

            ReadFromRow(row);
            return true;
        }

        /// <summary>
        /// Возвращает команду для загрузки объекта из БД по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта, который следует загрузить.</param>
        /// <returns></returns>
        protected virtual IBaseCommand GetLoadByIDCommand(int id)
        {
            IBaseCommand command = new Query(ObjectMapper.GetSelectByIDQueryStatement(this.GetType()).ToString());
            command.Add("@ID", id);

            return command;
        }

		public virtual bool LoadByReference(params object[] param)
		{
			bool result = false;

			Type thisType = this.GetType();
		    var queryStatement = ObjectMapper.GetSelectQueryStatement(thisType);

			Dictionary<string, object> prms;
            queryStatement.ConcatClauses(GetObjectByFieldCondition(this.GetType(), out prms, param));
            BaseCommand command = new Query(queryStatement.ToString());

            foreach (string key in prms.Keys)
            {
                command.Add(key, prms[key]);
            }

			DataRow row = command.ExecDataRow();
			if (row != null)
			{
				ReadFromRow(row);
				result = true;
			}

			return result;
		}

        #endregion

        #region [ Сохранение объекта ]

        #region [ Вставка нового объекта в БД ]

        private void Add()
        {
            BaseCommandCollection insertCommands = GetInsertCommands();
            ID = Convert.ToInt32(insertCommands.ExecScalar());

            // пишем в лог
            Logger.InfoInsertObject(this);
        }

        /// <summary>
        /// Создает команду к БД, которая вставляет новую запись в БД, соответствующую текущему объекту.
        /// Кроме того команда запрашивает значение идентификатора вновь добавленного объекта, которое можно 
        /// получить вызовом ExecScalar().
        /// </summary>
        /// <returns></returns>
        private BaseCommandCollection GetInsertCommands()
        {
            var insertCommands = new BaseCommandCollection();
            var dictMappingData = new Dictionary<string, List<FieldData>>();
            Dictionary<string, string> updateCommandsText = ObjectMapper.GetInsertStatements(this, out dictMappingData);

            foreach (var updateCommandText in updateCommandsText)
            {
                IBaseCommand command = new Query(updateCommandText.Value);
                addParametersToCommand(command, dictMappingData[updateCommandText.Key]);
                insertCommands.Add(command.Command);
            }

            return insertCommands;
        }

        #endregion

        #region  [ Изменение существующего объекта в БД ]

        private void Update()
        {
            BaseCommandCollection updateCommands = GetUpdateCommands();
            updateCommands.ExecNonQuery();

            // пишем в лог
            Logger.InfoUpdateObject(this);
        }

        /// <summary>
        /// Создает команду к БД, которая изменяет запись об объекте в БД.
        /// </summary>
        /// <returns></returns>
        private BaseCommandCollection GetUpdateCommands()
        {
            BaseCommandCollection updateCommands = new BaseCommandCollection();
            Dictionary<string, List<FieldData>> dictMappingData = new Dictionary<string, List<FieldData>>();
            Dictionary<string, string> updateCommandsText = ObjectMapper.GetUpdateStatements(this, out dictMappingData);

            foreach (var updateCommandText in updateCommandsText)
            {
                IBaseCommand command = new Query(updateCommandText.Value);
                command.Add("@ID", ID);
                addParametersToCommand(command, dictMappingData[updateCommandText.Key]);
                updateCommands.Add(command.Command);
            }

            return updateCommands;
        }

        #endregion

        private void addParametersToCommand(IBaseCommand command, List<FieldData> listFieldData)
        {
            foreach (FieldData fieldData in listFieldData)
            {
                if (fieldData.ObjectType == typeof(Byte[]))
                    command.Add("@" + fieldData.FieldName, fieldData.ObjectValue, DbType.Binary);
                else
                    command.Add("@" + fieldData.FieldName, fieldData.ObjectValue);
            }
        }

        public override void Save()
        {
            if (!IsSaved)
                Add();
            else
                Update();
        }

        #endregion

        #region [ Удаление объекта ]

        public override void Delete()
        {
            DeleteObjectByID(this.GetType(), ID.Value);
        }

        /// <summary>
        /// Удаляет объект данного типа по идентификатору.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        public static void DeleteObjectByID(Type type, int id)
        {
            // для записи в лог необходимо получить название объекта
            LogObjectAttribute logAttr = Logger.GetLogObjectAttribute(type);
            string value = null;
            if (logAttr != null)
            {
                // если объект предназначен для протоколирования
                // для этого перед удалением подгружаем объект
                BasePlainObject obj = (BasePlainObject)Activator.CreateInstance(type);
                obj.Load(id);
                // получаем значение объекта
                value = Logger.GetLogObjectValue(obj, logAttr.PropertyName);
            }

            // производим непосредственно удаление
            IBaseCommand deleteCommand = new Query(ObjectMapper.GetDeleteStatement(type));
            deleteCommand.Add("@ID", id);

            deleteCommand.ExecNonQuery();

            if (logAttr != null)
            {
                // пишем в лог
                Logger.InfoDeleteObject(type, value);
            }
        }

        #endregion

        #endregion

        #region [ Постраничные списки объектов ]

        /// <summary>
        /// Возвращает команду БД для получения списка объектов.
        /// </summary>
        /// <param name="type">Тип объектов.</param>
        /// <returns></returns>
        public static BaseCommand GetObjectsListCommand(Type type)
        {
            return new Query(ObjectMapper.GetSelectQueryStatement(type).ToString());
        }

        /// <summary>
        /// Возвращает команду БД для получения постраничного списка объектов.
        /// </summary>
        /// <param name="type">Тип объектов.</param>
        /// <param name="args">Параметры страницы.</param>
        /// <returns></returns>
        public static BaseCommand GetObjectsPageCommand(Type type, PagingArgs args, params object[] param)
        {
            Dictionary<string, object> prms;
            var statement = ObjectMapper.GetSelectQueryStatement(type);
            statement.ConcatClauses(GetObjectByFieldCondition(type, out prms, param));

            BaseCommand command = new Query(statement.ToString());
            foreach (string key in prms.Keys)
            {
                command.Add(key, prms[key]);
            }

            var procedure = new Procedure("uiGetObjectsPage");
            procedure.Add("@PageIndex", args.PageIndex);
            procedure.Add("@PageSize", args.PageSize);
            procedure.Add("@OrderField", args.SortExpression);
            procedure.Add("@IsOrderASC", args.SortOrderASC);
            procedure.Add("@Query", ObjectMapper.GetCommandFullText(command));
            procedure.AddReturnValueParameter();

            return procedure;
        }

        /// <summary>
        /// Возвращает датасет, содержащий страницу записей из БД, 
        /// соответствующих объектам заданного типа.
        /// </summary>
        /// <param name="type">Тип объектов.</param>
        /// <param name="args">Параметры страницы.</param>
        /// <param name="totalCount">Общее количество объектов данного типа.</param>
        /// <returns></returns>
        private static DataSet GetObjectsPageDataset(Type type, PagingArgs args, out int totalCount, params object[] param)
        {
            BaseCommand command = GetObjectsPageCommand(type, args, param);
            DataSet ds = command.ExecDataSet();
            totalCount = Convert.ToInt32(command.GetReturnValue());
            return ds;
        }

        /// <summary>
        /// Возвращает датасет, содержащий все записи БД, соответствующие объектам заданного типа.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static DataSet GetObjectsDataset(Type type, params object[] param)
        {
            Dictionary<string, object> prms;
            var queryStatement = ObjectMapper.GetSelectQueryStatement(type);
            queryStatement.ConcatClauses(GetObjectByFieldCondition(type, out prms, param));

            BaseCommand command = new Query(queryStatement.ToString());
            foreach (string key in prms.Keys)
            {
                command.Add(key, prms[key]);
            }
            return command.ExecDataSet();
        }

        /// <summary>
        /// Возвращает все объекты данного типа в виде BaseObjectCollection
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetObjects(Type type, params object[] param)
        {
            Type tp = typeof(BaseObjectCollection<>);
            Type[] tpArgs = { type };
            Type collectionType = tp.MakeGenericType(tpArgs);
			return GetObjects( type, collectionType, param );
        }

        /// <summary>
        /// Возвращает все объекты данного типа в виде коллекции типа сollectionType.
        /// </summary>
        /// <param name="type">Тип обьекта.</param>
        /// <returns>Коллекция типа collectionType.</returns>
        public static object GetObjects(Type type, Type collectionType, params object[] param)
        {
            object coll = Activator.CreateInstance(collectionType);
            MethodInfo fill_from_dataset_method = coll.GetType().GetMethod("FillFromDataSet", new Type[] { typeof(DataSet) });
            fill_from_dataset_method.Invoke(coll, new object[] { GetObjectsDataset(type, param) });
            
            return coll;
        }

        /// <summary>
        /// Возвращает все объекты данного типа в виде BaseObjectCollection, 
        /// отсортированные по значениям указанного свойства.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sortPropertyName">Имя свойства класса, по которому нужно сортировать значения.</param>
        /// <param name="sortOrderAsc">true - если по возрастанию, false - по убыванию.</param>
        /// <returns></returns>
        public static object GetObjects(Type type, string sortPropertyName, bool sortOrderAsc, params object[] param)
        {
			Type tp = typeof( BaseObjectCollection<> );
			Type[] tpArgs = { type };
			Type collectionType = tp.MakeGenericType( tpArgs );
			return GetObjects( type, collectionType, sortPropertyName, sortOrderAsc, param );
        }

        /// <summary>
        /// Возвращает все объекты данного типа в виде коллекции типа collectionType, 
        /// отсортированные по значениям указанного свойства.
        /// </summary>
        /// <param name="type">Тип обьекта.</param>
        /// <param name="collectionType">Тип коллекции.</param>
        /// <param name="sortPropertyName">Имя свойства класса, по которому нужно сортировать значения.</param>
        /// <param name="sortOrderAsc"></param>
        /// <param name="param">true - если по возрастанию, false - по убыванию.</param>
        /// <returns>Коллекция типа collectionType.</returns>
        public static object GetObjects(Type type, Type collectionType, string sortPropertyName, bool sortOrderAsc, params object[] param)
        {
            // получаем свойство по имени свойства
			PropertyInfo prop = ObjectPropertiesMapper.GetDBReadPropertyByName( type, sortPropertyName );
            
            if (prop == null)
                throw new CoreInvalidOperationException(ResourceManager.GetString("PropertyException", type.FullName, sortPropertyName));
            
            // получаем имя поля
            string fieldName = ObjectPropertiesMapper.GetFieldNameByProperty(prop);

            // формируем аргументы выборки
            var args = new PagingArgs(0, PagingArgs.MaxPageSize, fieldName, sortOrderAsc);
            int totalCount;

            // получаем данные
            object coll = Activator.CreateInstance(collectionType);
            MethodInfo fill_from_dataset_method = coll.GetType().GetMethod("FillFromDataSet", new[] {typeof (DataSet)});
            fill_from_dataset_method.Invoke(coll, new object[] {GetObjectsPageDataset(type, args, out totalCount, param)});
            
            return coll;
        }

		/// <summary>
		/// Возвращает страницу объектов заданного типа.
		/// </summary>
		/// <param name="type">Тип объектов.</param>
		/// <param name="args">Параметры страницы.</param>
		/// <returns>Результат пейджинга. Если объекты не найдены, то результат пустой.</returns>
		public static PagingResult GetObjectsPage( Type type, PagingArgs args )
		{
			return GetObjectsPage( type, args, null );
		}

        /// <summary>
        /// Возвращает страницу объектов заданного типа.
        /// </summary>
        /// <param name="type">Тип объектов.</param>
        /// <param name="args">Параметры страницы.</param>
		/// <param name="param">Дополнительные параметры выборки.</param>
        /// <returns>Результат пейджинга. Если объекты не найдены, то результат пустой.</returns>
        public static PagingResult GetObjectsPage(Type type, PagingArgs args, params object[] param)
        {
            Type tp = typeof(BaseObjectCollection<>);
            Type[] tpArgs = { type };
            Type collectionType = tp.MakeGenericType(tpArgs);

            if (!string.IsNullOrEmpty(args.SortExpression))
            {
                PropertyInfo prop = ObjectPropertiesMapper.GetDBReadPropertyByName(type, args.SortExpression);
                if (prop == null)
                    throw new CoreInvalidOperationException(ResourceManager.GetString("PropertyException", type.FullName, args.SortExpression));

                args.SortExpression = ObjectPropertiesMapper.GetFieldNameByProperty(prop);
            }

            object coll = Activator.CreateInstance(collectionType);
            int totalCount = 0;
            MethodInfo fill_from_dataset_method = coll.GetType().GetMethod("FillFromDataSet", new [] { typeof(DataSet) });
            fill_from_dataset_method.Invoke(coll, new object[] { GetObjectsPageDataset(type, args, out totalCount, param) });
            return new PagingResult(coll, totalCount);
        }

		/// <summary>
		/// Возвращает коллекцию объектов, связанных с данным по внешнему ключу.
		/// Первичным ключём данного объекта считается ID.
		/// </summary>
		/// <param name="type">Тип связанных объектов.</param>
		/// <param name="collectionType">Тип коллекции связанных объектов</param>
		/// <param name="propertyName">Имя свойства связанных объектов, 
		/// по которому происходит связь с данным.</param>
		/// <returns>Возвращает коллекцию объектов. Если объекты не найдены,
		/// коллекция будет пустой.</returns>
        public virtual object GetLinkedObjects(Type type, Type collectionType, string propertyName)
        {
            if (!DBAttributesManager.HasDBTableAttribute(type))
                throw new CoreInvalidOperationException(ResourceManager.GetString("TypeDBTableException", type.FullName));

            object result = Activator.CreateInstance(collectionType);
            if (ID.HasValue)
                result = GetObjects(type, collectionType, propertyName, ID.Value);

            return result;
        }

		/// <summary>
		/// Возвращает коллекцию объектов, связанных с данным по внешнему ключу с 
		/// дополнительными ограничениями. Первичным ключём данного объекта считается ID.
		/// </summary>
		/// <param name="type">Тип связанных объектов.</param>
		/// <param name="collectionType">Тип коллекции связанных объектов</param>
		/// <param name="propertyName">Имя свойства связанных объектов, 
		/// по которому происходит связь с данным.</param>
		/// <param name="param">Дополнительные ограничения на выборку.</param>
		/// <returns>Возвращает коллекцию объектов. Если объекты не найдены,
		/// коллекция будет пустой.</returns>
        public virtual object GetLinkedObjects(Type type, Type collectionType, string propertyName, params object[] param)
        {
            if (!DBAttributesManager.HasDBTableAttribute(type))
                throw new CoreInvalidOperationException(ResourceManager.GetString("TypeDBTableException", type.FullName));

            object result = Activator.CreateInstance(collectionType);
            if (ID.HasValue)
            {
                object[] par = new object[param.Length + 2];
                par[0] = propertyName;
                par[1] = ID.Value;
                param.CopyTo(par, 2);

                result = GetObjects(type, collectionType, par);
            }

            return result;
        }

		/// <summary>
		/// Возвращает коллекцию объектов, связанных с данным по внешнему ключу с 
		/// дополнительными ограничениями. Первичным ключём данного объекта считается ID.
		/// </summary>
		/// <param name="type">Тип связанных объектов.</param>
		/// <param name="collectionType">Тип коллекции связанных объектов</param>
		/// <param name="propertyName">Имя свойства связанных объектов, 
		/// по которому происходит связь с данным.</param>
		/// <param name="param">Дополнительные ограничения на выборку.</param>
		/// <returns>Возвращает коллекцию объектов. Если объекты не найдены,
		/// коллекция будет пустой.</returns>
        public virtual object GetLinkedObjects(Type type, Type collectionType, string propertyName,
            string sortPropertyName, bool sortAsc, params object[] param)
        {
            if (!DBAttributesManager.HasDBTableAttribute(type))
                throw new CoreInvalidOperationException(ResourceManager.GetString("TypeDBTableException", type.FullName));

            object result = Activator.CreateInstance(collectionType);
            if (ID.HasValue)
            {
                object[] par = new object[param.Length + 2];
                par[0] = propertyName;
                par[1] = ID.Value;
                param.CopyTo(par, 2);

                result = GetObjects(type, collectionType, sortPropertyName, sortAsc, par);
            }

            return result;
        }

		/// <summary>
		/// Возвращает страницу объектов, связанных с данным по ключевому полю.
		/// Первичным ключём для данного объекта считается ID.
		/// </summary>
		/// <param name="type">Тип связанных объектов.</param>
		/// <param name="propertyName">Имя свойства связанных объектов, 
		/// по которому происходит связь с данным.</param>
		/// <returns>Результат импорта. Если объекты не найдены, то он пустой.</returns>
        public virtual PagingResult GetLinkedObjectsPage(Type type, PagingArgs args, string propertyName)
        {
            if (!DBAttributesManager.HasDBTableAttribute(type))
                throw new CoreInvalidOperationException(ResourceManager.GetString("TypeDBTableException", type.FullName));

            PagingResult result = PagingResult.Empty;
            if (ID.HasValue)
                result = GetObjectsPage(type, args, propertyName, ID.Value);

            return result;
        }

		/// <summary>
		/// Возвращает страницу объектов, связанных с данным по ключевому полю.
		/// Первичным ключём для данного объекта считается ID.
		/// </summary>
		/// <param name="type">Тип связанных объектов.</param>
		/// <param name="propertyName">Имя свойства связанных объектов, 
		/// по которому происходит связь с данным.</param>
		/// <returns>Результат импорта. Если объекты не найдены, то он пустой.</returns>
        public virtual PagingResult GetLinkedObjectsPage(Type type, PagingArgs args, string propertyName,
            params object[] param)
        {
            if (!DBAttributesManager.HasDBTableAttribute(type))
                throw new CoreInvalidOperationException(ResourceManager.GetString("TypeDBTableException", type.FullName));

            PagingResult result = PagingResult.Empty;
            if (ID.HasValue)
            {
                object[] par = new object[param.Length + 2];
                par[0] = propertyName;
                par[1] = ID.Value;
                param.CopyTo(par, 2);

                result = GetObjectsPage(type, args, par);
            }

            return result;
        }

        /// <summary>
        /// Return QeryStatemenetClause collection
        /// </summary>
        /// <param name="type"></param>
        /// <param name="prms"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected static List<QueryStatementClause> GetObjectByFieldCondition(Type type, out Dictionary<string, object> prms, params object[] param)
        {
            prms = new Dictionary<string, object>();
            if (param == null || param.Length == 0)
                return new List<QueryStatementClause>();

            var clauses = new List<QueryStatementClause>();
            for (int i = 0; i < param.Length; i += 2)
            {
                string fieldName = (string) param[i];
                object fieldValue = param[i + 1];
                string bitWiseOperator = (i != 0) ? "AND" : string.Empty;

                if (fieldValue is MLString)
                {
                    var fName = GetDBReadName(type, fieldName);
                    var innerClause_en = new QueryStatementClause(string.Empty, "=")
                                             {
                                                 FieldName = string.Format("{0}_en", fName),
                                                 Value = string.Format("@prm_en{0}", i)
                                             };
                    var innerClause_ru = new QueryStatementClause("OR", "=")
                                             {
                                                 FieldName = string.Format("{0}_ru", fName),
                                                 Value = string.Format("@prm_ru{0}", i)
                                             };
                    clauses.Add(new QueryStatementClause(bitWiseOperator)
                                    {
                                        Value = new List<QueryStatementClause> {innerClause_en, innerClause_ru}
                                    });

                    prms.Add(string.Format("@prm_en{0}", i), ((MLString)fieldValue)[CultureManager.Languages.English]);
                    prms.Add(string.Format("@prm_ru{0}", i), ((MLString)fieldValue)[CultureManager.Languages.Russian]);

                    continue;
                }

                if (fieldValue is Array)
                {
                    if (((Array)fieldValue).Length > 0)
                    {
                        clauses.Add(new QueryStatementClause(bitWiseOperator, "IN")
                                        {
                                            FieldName = GetDBReadName(type, fieldName),
                                            Value = fieldValue
                                        });
                    }
                    else
                        clauses.Add(new QueryStatementClause(bitWiseOperator, "!=") {FieldName = "NULL", Value = "NULL"});

                    continue;
                }

                clauses.Add(new QueryStatementClause(bitWiseOperator, "=")
                                {
                                    FieldName = GetDBReadName(type, fieldName),
                                    Value = string.Format("@prm{0}", i)
                                });
                prms.Add(string.Format("@prm{0}", i), fieldValue);
            }

            return clauses;
        }

        /// <summary>
        /// Загружает объект по заданному набору имен полей и значениям
        /// </summary>
        /// <param name="field_name"></param>
        /// <param name="value"></param>
        /// <returns>Строку данных, если объект найден и null, если нет</returns>
        protected DataRow GetObjectByField(params object[] param)
        {
            Type thisType = this.GetType();
            var queryStatement = ObjectMapper.GetSelectQueryStatement(thisType);
            
            Dictionary<string, object> prms;
            queryStatement.ConcatClauses(GetObjectByFieldCondition(this.GetType(), out prms, param));
            
            BaseCommand command = new Query(queryStatement.ToString());
            foreach (string key in prms.Keys)
            {
                command.Add(key, prms[key]);
            }

            return command.ExecDataRow();
        }		

        /// <summary>
        /// Возвращает страницу объектов заданного типа, отфильтрованную по заданным полям
        /// </summary>
        /// <param name="type">Тип объекта</param>
        /// <param name="args">Параметры страницы</param>
        /// <param name="param">Параметры фильтрации в формате имя свойства - значение - ...</param>
        /// <returns></returns>
        public static PagingResult GetObjectsPageWithCondition(Type type, PagingArgs args, params object[] param)
        {
            Type tp = typeof(BaseObjectCollection<>);
            Type[] tpArgs = { type };
			Type collectionType = tp.MakeGenericType( tpArgs );
			object coll = Activator.CreateInstance( collectionType );
            int totalCount = 0;
            MethodInfo fill_from_dataset_method = coll.GetType().GetMethod("FillFromDataSet", new Type[] { typeof(DataSet) });
            fill_from_dataset_method.Invoke(coll, new object[] { GetObjectsPageDataset(type, args, out totalCount, param) });
            return new PagingResult(coll, totalCount);
        }

        /// <summary>
        /// Возвращает значение аттрибута DBRead для свойства с заданным именем
        /// </summary>
        /// <param name="property_name"></param>
        /// <returns></returns>
        private static string GetDBReadName(Type type, string property_name)
        {
            PropertyInfo prop = type.GetProperty(property_name);

            if (prop == null)
                throw new CoreApplicationException(ResourceManager.GetString("PropertyNotFoundInTypeException", property_name, type));

            foreach (Attribute attr in prop.GetCustomAttributes(typeof(DBReadAttribute), true))
                return ((DBReadAttribute)attr).FieldName;

            throw new CoreApplicationException(ResourceManager.GetString("DBReadAttributeException", property_name, type));
        }

        #endregion
    }
}