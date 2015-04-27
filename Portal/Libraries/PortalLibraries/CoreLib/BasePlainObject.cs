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
    /// ������� ����� ��� ������� (�.�. ��� ��������, �������������� ����� ������ � ����� 
    /// ����������� ������� ���� ������) �������� ������-������.
    /// </summary>
    public abstract class BasePlainObject : BaseObject
    {
        #region [ �������� / ���������� / �������� ������� ]

        #region [ �������� ������� ]

        /// <summary>
        /// ��������� ������ �� ��������������.
        /// </summary>
        /// <param name="id">�������������.</param>
        /// <returns>���������� true, ���� ������ ��� ������ � ��, ����� false.</returns>
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
        /// ���������� ������� ��� �������� ������� �� �� �� ��������������.
        /// </summary>
        /// <param name="id">������������� �������, ������� ������� ���������.</param>
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

        #region [ ���������� ������� ]

        #region [ ������� ������ ������� � �� ]

        private void Add()
        {
            BaseCommandCollection insertCommands = GetInsertCommands();
            ID = Convert.ToInt32(insertCommands.ExecScalar());

            // ����� � ���
            Logger.InfoInsertObject(this);
        }

        /// <summary>
        /// ������� ������� � ��, ������� ��������� ����� ������ � ��, ��������������� �������� �������.
        /// ����� ���� ������� ����������� �������� �������������� ����� ������������ �������, ������� ����� 
        /// �������� ������� ExecScalar().
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

        #region  [ ��������� ������������� ������� � �� ]

        private void Update()
        {
            BaseCommandCollection updateCommands = GetUpdateCommands();
            updateCommands.ExecNonQuery();

            // ����� � ���
            Logger.InfoUpdateObject(this);
        }

        /// <summary>
        /// ������� ������� � ��, ������� �������� ������ �� ������� � ��.
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

        #region [ �������� ������� ]

        public override void Delete()
        {
            DeleteObjectByID(this.GetType(), ID.Value);
        }

        /// <summary>
        /// ������� ������ ������� ���� �� ��������������.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        public static void DeleteObjectByID(Type type, int id)
        {
            // ��� ������ � ��� ���������� �������� �������� �������
            LogObjectAttribute logAttr = Logger.GetLogObjectAttribute(type);
            string value = null;
            if (logAttr != null)
            {
                // ���� ������ ������������ ��� ����������������
                // ��� ����� ����� ��������� ���������� ������
                BasePlainObject obj = (BasePlainObject)Activator.CreateInstance(type);
                obj.Load(id);
                // �������� �������� �������
                value = Logger.GetLogObjectValue(obj, logAttr.PropertyName);
            }

            // ���������� ��������������� ��������
            IBaseCommand deleteCommand = new Query(ObjectMapper.GetDeleteStatement(type));
            deleteCommand.Add("@ID", id);

            deleteCommand.ExecNonQuery();

            if (logAttr != null)
            {
                // ����� � ���
                Logger.InfoDeleteObject(type, value);
            }
        }

        #endregion

        #endregion

        #region [ ������������ ������ �������� ]

        /// <summary>
        /// ���������� ������� �� ��� ��������� ������ ��������.
        /// </summary>
        /// <param name="type">��� ��������.</param>
        /// <returns></returns>
        public static BaseCommand GetObjectsListCommand(Type type)
        {
            return new Query(ObjectMapper.GetSelectQueryStatement(type).ToString());
        }

        /// <summary>
        /// ���������� ������� �� ��� ��������� ������������� ������ ��������.
        /// </summary>
        /// <param name="type">��� ��������.</param>
        /// <param name="args">��������� ��������.</param>
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
        /// ���������� �������, ���������� �������� ������� �� ��, 
        /// ��������������� �������� ��������� ����.
        /// </summary>
        /// <param name="type">��� ��������.</param>
        /// <param name="args">��������� ��������.</param>
        /// <param name="totalCount">����� ���������� �������� ������� ����.</param>
        /// <returns></returns>
        private static DataSet GetObjectsPageDataset(Type type, PagingArgs args, out int totalCount, params object[] param)
        {
            BaseCommand command = GetObjectsPageCommand(type, args, param);
            DataSet ds = command.ExecDataSet();
            totalCount = Convert.ToInt32(command.GetReturnValue());
            return ds;
        }

        /// <summary>
        /// ���������� �������, ���������� ��� ������ ��, ��������������� �������� ��������� ����.
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
        /// ���������� ��� ������� ������� ���� � ���� BaseObjectCollection
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
        /// ���������� ��� ������� ������� ���� � ���� ��������� ���� �ollectionType.
        /// </summary>
        /// <param name="type">��� �������.</param>
        /// <returns>��������� ���� collectionType.</returns>
        public static object GetObjects(Type type, Type collectionType, params object[] param)
        {
            object coll = Activator.CreateInstance(collectionType);
            MethodInfo fill_from_dataset_method = coll.GetType().GetMethod("FillFromDataSet", new Type[] { typeof(DataSet) });
            fill_from_dataset_method.Invoke(coll, new object[] { GetObjectsDataset(type, param) });
            
            return coll;
        }

        /// <summary>
        /// ���������� ��� ������� ������� ���� � ���� BaseObjectCollection, 
        /// ��������������� �� ��������� ���������� ��������.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sortPropertyName">��� �������� ������, �� �������� ����� ����������� ��������.</param>
        /// <param name="sortOrderAsc">true - ���� �� �����������, false - �� ��������.</param>
        /// <returns></returns>
        public static object GetObjects(Type type, string sortPropertyName, bool sortOrderAsc, params object[] param)
        {
			Type tp = typeof( BaseObjectCollection<> );
			Type[] tpArgs = { type };
			Type collectionType = tp.MakeGenericType( tpArgs );
			return GetObjects( type, collectionType, sortPropertyName, sortOrderAsc, param );
        }

        /// <summary>
        /// ���������� ��� ������� ������� ���� � ���� ��������� ���� collectionType, 
        /// ��������������� �� ��������� ���������� ��������.
        /// </summary>
        /// <param name="type">��� �������.</param>
        /// <param name="collectionType">��� ���������.</param>
        /// <param name="sortPropertyName">��� �������� ������, �� �������� ����� ����������� ��������.</param>
        /// <param name="sortOrderAsc"></param>
        /// <param name="param">true - ���� �� �����������, false - �� ��������.</param>
        /// <returns>��������� ���� collectionType.</returns>
        public static object GetObjects(Type type, Type collectionType, string sortPropertyName, bool sortOrderAsc, params object[] param)
        {
            // �������� �������� �� ����� ��������
			PropertyInfo prop = ObjectPropertiesMapper.GetDBReadPropertyByName( type, sortPropertyName );
            
            if (prop == null)
                throw new CoreInvalidOperationException(ResourceManager.GetString("PropertyException", type.FullName, sortPropertyName));
            
            // �������� ��� ����
            string fieldName = ObjectPropertiesMapper.GetFieldNameByProperty(prop);

            // ��������� ��������� �������
            var args = new PagingArgs(0, PagingArgs.MaxPageSize, fieldName, sortOrderAsc);
            int totalCount;

            // �������� ������
            object coll = Activator.CreateInstance(collectionType);
            MethodInfo fill_from_dataset_method = coll.GetType().GetMethod("FillFromDataSet", new[] {typeof (DataSet)});
            fill_from_dataset_method.Invoke(coll, new object[] {GetObjectsPageDataset(type, args, out totalCount, param)});
            
            return coll;
        }

		/// <summary>
		/// ���������� �������� �������� ��������� ����.
		/// </summary>
		/// <param name="type">��� ��������.</param>
		/// <param name="args">��������� ��������.</param>
		/// <returns>��������� ���������. ���� ������� �� �������, �� ��������� ������.</returns>
		public static PagingResult GetObjectsPage( Type type, PagingArgs args )
		{
			return GetObjectsPage( type, args, null );
		}

        /// <summary>
        /// ���������� �������� �������� ��������� ����.
        /// </summary>
        /// <param name="type">��� ��������.</param>
        /// <param name="args">��������� ��������.</param>
		/// <param name="param">�������������� ��������� �������.</param>
        /// <returns>��������� ���������. ���� ������� �� �������, �� ��������� ������.</returns>
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
		/// ���������� ��������� ��������, ��������� � ������ �� �������� �����.
		/// ��������� ������ ������� ������� ��������� ID.
		/// </summary>
		/// <param name="type">��� ��������� ��������.</param>
		/// <param name="collectionType">��� ��������� ��������� ��������</param>
		/// <param name="propertyName">��� �������� ��������� ��������, 
		/// �� �������� ���������� ����� � ������.</param>
		/// <returns>���������� ��������� ��������. ���� ������� �� �������,
		/// ��������� ����� ������.</returns>
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
		/// ���������� ��������� ��������, ��������� � ������ �� �������� ����� � 
		/// ��������������� �������������. ��������� ������ ������� ������� ��������� ID.
		/// </summary>
		/// <param name="type">��� ��������� ��������.</param>
		/// <param name="collectionType">��� ��������� ��������� ��������</param>
		/// <param name="propertyName">��� �������� ��������� ��������, 
		/// �� �������� ���������� ����� � ������.</param>
		/// <param name="param">�������������� ����������� �� �������.</param>
		/// <returns>���������� ��������� ��������. ���� ������� �� �������,
		/// ��������� ����� ������.</returns>
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
		/// ���������� ��������� ��������, ��������� � ������ �� �������� ����� � 
		/// ��������������� �������������. ��������� ������ ������� ������� ��������� ID.
		/// </summary>
		/// <param name="type">��� ��������� ��������.</param>
		/// <param name="collectionType">��� ��������� ��������� ��������</param>
		/// <param name="propertyName">��� �������� ��������� ��������, 
		/// �� �������� ���������� ����� � ������.</param>
		/// <param name="param">�������������� ����������� �� �������.</param>
		/// <returns>���������� ��������� ��������. ���� ������� �� �������,
		/// ��������� ����� ������.</returns>
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
		/// ���������� �������� ��������, ��������� � ������ �� ��������� ����.
		/// ��������� ������ ��� ������� ������� ��������� ID.
		/// </summary>
		/// <param name="type">��� ��������� ��������.</param>
		/// <param name="propertyName">��� �������� ��������� ��������, 
		/// �� �������� ���������� ����� � ������.</param>
		/// <returns>��������� �������. ���� ������� �� �������, �� �� ������.</returns>
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
		/// ���������� �������� ��������, ��������� � ������ �� ��������� ����.
		/// ��������� ������ ��� ������� ������� ��������� ID.
		/// </summary>
		/// <param name="type">��� ��������� ��������.</param>
		/// <param name="propertyName">��� �������� ��������� ��������, 
		/// �� �������� ���������� ����� � ������.</param>
		/// <returns>��������� �������. ���� ������� �� �������, �� �� ������.</returns>
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
        /// ��������� ������ �� ��������� ������ ���� ����� � ���������
        /// </summary>
        /// <param name="field_name"></param>
        /// <param name="value"></param>
        /// <returns>������ ������, ���� ������ ������ � null, ���� ���</returns>
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
        /// ���������� �������� �������� ��������� ����, ��������������� �� �������� �����
        /// </summary>
        /// <param name="type">��� �������</param>
        /// <param name="args">��������� ��������</param>
        /// <param name="param">��������� ���������� � ������� ��� �������� - �������� - ...</param>
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
        /// ���������� �������� ��������� DBRead ��� �������� � �������� ������
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