using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;

using Core.DB.QueryStatement;
using Core.ORM;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.FiltersSupport
{
    //public class FieldFilter
    //{
    //    #region [ Constructors ]

    //    public FieldFilter(DBFilterFieldAttribute fieldAttribute, object value)
    //        : this(fieldAttribute.FieldName, fieldAttribute.Operator, value)
    //    {}

    //    public FieldFilter(string fieldName, object value)
    //        : this(fieldName, string.Empty, value)
    //    { }

    //    public FieldFilter(string fieldName, string Operator, object value)
    //    {
    //        m_FieldName = fieldName;
    //        m_Operator = Operator;
    //        m_Value = value;
    //    }

    //    #endregion

    //    #region [ Fields ]

    //    private string m_FieldName = string.Empty;
    //    private string m_Operator = string.Empty;
    //    private object m_Value = null;

    //    #endregion

    //    #region [ Properties ]

    //    public string FieldName
    //    {
    //        get { return m_FieldName; }
    //        set { m_FieldName = value; }
    //    }

    //    public string Operator
    //    {
    //        get 
    //        {
    //            if (string.IsNullOrEmpty(m_Operator))
    //                m_Operator = calculateOpertaor();

    //            return m_Operator; 
    //        }
    //        set { m_Operator = value; }
    //    }

    //    public object Value
    //    {
    //        get { return m_Value; }
    //        set { m_Value = value; }
    //    }

    //    #endregion

    //    #region [ Methods ]

    //    private string calculateOpertaor()
    //    {
    //        if (Value is ICollection)
    //            return "IN";

    //        if (Value is string)
    //            return "LIKE";

    //        return "=";
    //    }

    //    private string convertValueToString(object value)
    //    {
    //        if (value is ICollection)
    //        {
    //            string expr = string.Empty;
    //            foreach (var item in (ICollection)value)
    //            {
    //                if (!string.IsNullOrEmpty(expr))
    //                    expr += ", ";

    //                expr += item.ToString();
    //            }
    //            return expr;
    //        }

    //        if (value is int && (int)value == 0)
    //            return string.Empty;

    //        if (value is bool)
    //            return (bool)value ? "1" : "0";

    //        return value.ToString();
    //    }

    //    public string ToString(string tableName)
    //    {
    //        string value = Operator.Equals("LIKE", StringComparison.InvariantCultureIgnoreCase)
    //                                        ? string.Format("'%{0}%'", convertValueToString(Value))
    //                                        : convertValueToString(Value);
            
    //        if (Operator.Equals("IN", StringComparison.InvariantCultureIgnoreCase))
    //            value = string.Format("({0})", convertValueToString(Value));

    //        return string.IsNullOrEmpty(tableName)
    //            ? string.Format("{0} {1} {2}", FieldName, Operator, value)
    //            : string.Format("{0}.{1} {2} {3}", tableName, FieldName, Operator, value);
    //    }

    //    #endregion
    //}

    public static class DBFilterBuilder
    {
        public static QueryStatement GetQueryStatement(object objFilter)
        {
            var type = objFilter.GetType();
            var objMappingData = GetObjectMappingData(type);
            if (objMappingData.Count == 0)
                return null;

            return GetQueryStatement(objFilter, objMappingData);
        }

        public static QueryStatement GetQueryStatement(object objFilter, Dictionary<string, List<KeyValuePair<PropertyInfo, DBFilterFieldAttribute>>> objectMappingData)
        {
            var queryStatement = new QueryStatement(QueryStatementType.SELECT);

            string filterIDs = string.Empty;
            DBTableAttribute dbTabeAttr = DBAttributesManager.GetDBTableAttribute(objFilter.GetType());

            if (dbTabeAttr != null && dbTabeAttr.UseInherits && objectMappingData.Count > 1)
            {
                var baseID = string.Format("[{0}].ID", objectMappingData.Keys.First());
                foreach (var item in objectMappingData.Keys.Skip(1))
                {
                    var bitWiseOperator = queryStatement.Clauses.Count == 0 ? string.Empty : "AND";
                    queryStatement.Clauses.Add(new QueryStatementClause(bitWiseOperator, "=") 
                                                    { 
                                                        FieldName = baseID, 
                                                        Value = string.Format("[{0}].ID", item) 
                                                    });
                }
            }

            processQueryStatement(objFilter, queryStatement, objectMappingData);

            return queryStatement;
        }

        public static Dictionary<string, List<KeyValuePair<PropertyInfo, DBFilterFieldAttribute>>> GetObjectMappingData(Type type)
        {
            var columnNamesList = new Dictionary<Type, List<KeyValuePair<PropertyInfo, DBFilterFieldAttribute>>>();
            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var dbFilterAttribute = GetAttribute<DBFilterFieldAttribute>(prop);
                dbFilterAttribute = dbFilterAttribute != null ? dbFilterAttribute : GetAttribute<DBFilterTableAttribute>(prop);

                if (dbFilterAttribute == null)
                    continue;

                Type tableType = ObjectPropertiesMapper.GetTableTypeForProperty(type, prop);
                if (!columnNamesList.ContainsKey(tableType))
                    columnNamesList.Add(tableType, new List<KeyValuePair<PropertyInfo, DBFilterFieldAttribute>>());

                columnNamesList[tableType].Add(new KeyValuePair<PropertyInfo, DBFilterFieldAttribute>(prop, dbFilterAttribute));
            }

            var coll = columnNamesList.OrderBy(elem => elem.Key, new InhertitsTypeComparer());
            var result = coll.Select(elem => new { TableName = DBAttributesManager.GetDBTableName(elem.Key), FieldNames = elem.Value });

            return result.ToDictionary(elem => elem.TableName, elem => elem.FieldNames);
        }

        #region [ Helpers ]

        public static TAttribute GetAttribute<TAttribute>(PropertyInfo prop)
            where TAttribute : Attribute
        {
            var attrs = prop.GetCustomAttributes(typeof(TAttribute), true);
            return (attrs.Length != 0) ? (TAttribute)attrs[0] : null;
        }

        #endregion

        private static void processQueryStatement(object objFilter, QueryStatement queryStatement
                                    , Dictionary<string, List<KeyValuePair<PropertyInfo, DBFilterFieldAttribute>>> objectMappingData)
        {
            foreach (var pair in objectMappingData)
            {
                queryStatement.Tables.Add(pair.Key);

                foreach (var pairForTable in pair.Value)
                {
                    object value = null;
                    try
                    {
                        value = pairForTable.Key.GetValue(objFilter, null);
                        if (needToSkip(value))
                            continue;
                    }
                    catch (FormatException)
                    {
                        throw new Exception(String.Format("FilterBuilder_IcorrectInputFormat - {0}.", pairForTable.Key.Name));
                    }

                    var filterFieldAttr = pairForTable.Value;
                    if (filterFieldAttr == null)
                        continue;

                    var oper = filterFieldAttr.Operator;
                    if (filterFieldAttr.IsObjectProperty)
                    {
                        oper = "=";
                        value = GetQueryStatement(value);
                    }

                    if (!isValueValid(value))
                    {
                        if (filterFieldAttr.DefaultValue == null || string.IsNullOrEmpty(filterFieldAttr.DefaultValue.ToString()))
                            continue;

                        value = filterFieldAttr.DefaultValue;
                    }

                    var bitWiseOperator = string.Empty;
                    if (filterFieldAttr is DBFilterTableAttribute)
                    {
                        DBFilterTableAttribute filterTableAttr = (DBFilterTableAttribute)filterFieldAttr;

                        var innerQuery = new QueryStatement(QueryStatementType.SELECT);
                        innerQuery.Tables.Add(filterTableAttr.FromTableName);
                        innerQuery.Fields.Add(string.Format("[{0}].{1}", filterTableAttr.FromTableName, filterTableAttr.SelectFieldName));
                        innerQuery.Clauses.Add(new QueryStatementClause(string.Empty, oper)
                                                    {
                                                        FieldName = string.Format("[{0}].{1}", filterTableAttr.FromTableName, filterTableAttr.SelectFilterFieldName),
                                                        Value = value
                                                    });

                        value = innerQuery;
                    }

                    bitWiseOperator = queryStatement.Clauses.Count == 0 ? string.Empty : "AND";
                    queryStatement.Clauses.Add(new QueryStatementClause(bitWiseOperator, oper)
                                                    {
                                                        FieldName = string.Format("[{0}].{1}", pair.Key, filterFieldAttr.FieldName),
                                                        Value = value
                                                    });
                }
            }
        }

        private static bool needToSkip(object value)
        {
            if (value == null)
                return true;

            if (value is string && string.IsNullOrEmpty((string)value))
                return true;

            if (value is ICollection && ((ICollection)value).Count == 0)
                return true;

            return false;
        }

        //~~~~~~~~~~~~~~~
        //public static string BuildSelectExpression(string tableName, object filter, string select, Dictionary<string, string> dict)
        //{
        //    string where = filter is string
        //                        ? filter.ToString()
        //                        : BuildFilterExpression(filter, tableName, dict);
        //    return string.Format("SELECT {0} FROM {1} WHERE {2}", select, tableName, where); 
        //}

        //public static string BuildFilterExpression(object objFilter, string tableName)
        //{
        //    return BuildFilterExpression(objFilter, tableName, new Dictionary<string, string>());
        //}

        //public static string BuildFilterExpression(object objFilter, string tableName, Dictionary<string, string> mappingData)
        //{
        //    if (objFilter == null)
        //        return string.Empty;

        //    StringBuilder expression = new StringBuilder();
        //    Type filterType = objFilter.GetType();
        //    PropertyInfo[] properties = filterType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //    foreach (PropertyInfo propSrc in properties)
        //    {
        //        object[] filterFieldAttrs = propSrc.GetCustomAttributes(typeof(DBFilterFieldAttribute), false);
        //        if (filterFieldAttrs == null || filterFieldAttrs.Length == 0)
        //            continue;

        //        object value = null;
        //        try
        //        {
        //            value = propSrc.GetValue(objFilter, null);
        //            if (value == null)
        //                continue;

        //            if (value is string && string.IsNullOrEmpty((string)value))
        //                continue;

        //            if (value is ICollection && ((ICollection)value).Count == 0)
        //                continue;
        //        }
        //        catch (FormatException)
        //        {
        //            throw new Exception(String.Format("BuildFilterExpression_IcorrectInputFormat - {0}.", propSrc.Name));
        //        }

        //        DBFilterFieldAttribute filterFieldAttr = (filterFieldAttrs != null && filterFieldAttrs.Length > 0)
        //                                           ? (DBFilterFieldAttribute)filterFieldAttrs[0] : null;

        //        if (filterFieldAttr == null)
        //            continue;

        //        if (filterFieldAttr is DBFilterTableAttribute)
        //        {
        //            DBFilterTableAttribute filterTableAttr = (DBFilterTableAttribute)filterFieldAttr;
        //            string select = string.Format("{0}.{1}", filterTableAttr.FromTableName, filterTableAttr.SelectFieldName);

        //            if (filterFieldAttr.IsObjectProperty)
        //                value = BuildSelectExpression(filterTableAttr.FromTableName, value, select, mappingData);
        //            else
        //            {
        //                FieldFilter field = new FieldFilter(filterTableAttr.SelectFilterFieldName, filterTableAttr.Operator, value);
        //                value = string.Format("({0})", BuildSelectExpression(filterTableAttr.FromTableName, field.ToString(filterTableAttr.FromTableName), select, mappingData));
        //            }
        //        }

        //        if (filterFieldAttr.IsObjectProperty && value != null && !(filterFieldAttr is DBFilterTableAttribute))
        //            value = BuildFilterExpression(value, tableName);

        //        if (!isValueValid(value))
        //        {
        //            if (filterFieldAttr.DefaultValue == null || string.IsNullOrEmpty(filterFieldAttr.DefaultValue.ToString()))
        //                continue;

        //            value = filterFieldAttr.DefaultValue;
        //        }

        //        FieldFilter fieldFilter = new FieldFilter(filterFieldAttr, value);
        //        string resTableName = (mappingData != null && mappingData.ContainsKey(propSrc.Name))
        //                            ? mappingData[propSrc.Name]
        //                            : tableName;

        //        if (!string.IsNullOrEmpty(expression.ToString()))
        //            expression.Append(" AND ");

        //        if (filterFieldAttr.IsObjectProperty && !(filterFieldAttr is DBFilterTableAttribute))
        //            expression.Append(value);
        //        else
        //            expression.Append(fieldFilter.ToString(resTableName));
        //    }
        //    return expression.ToString();
        //}

        private static bool isValueValid(object value)
        {
            return !(value == null
                || (value is int && (int)value == 0)
                || string.IsNullOrEmpty(value.ToString()));
        }
    }
}