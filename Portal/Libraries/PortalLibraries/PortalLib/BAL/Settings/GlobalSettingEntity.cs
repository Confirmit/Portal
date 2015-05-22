using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Core;
using Core.ORM.Attributes;
using Core.DB;
using Core.ORM;

namespace ConfirmIt.PortalLib.BAL.Settings
{
    /// <summary>
    /// Глобальная неофисная настройка
    /// </summary>
    [DBTable("Settings")]
    internal class GlobalSettingEntity : BasePlainObject, ISetting
    {
        #region Fields

        private SettingAttribute m_SettingAttribute = null;
        private object m_Value = string.Empty;

        #endregion

        #region Constructors

        public GlobalSettingEntity(SettingAttribute attribute)
        {
            m_SettingAttribute = new SettingAttribute(attribute.SettingType, attribute.SettingName);
        }

        #endregion

        #region ISetting Members

        public string KeyColumnName
        {
            get { return "Name"; }
        }

        public SettingAttribute SettingAttribute
        {
            get { return m_SettingAttribute; }
        }

        [DBRead("Value")]
        [DBNullable]
        public object Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        #endregion

        #region LoadByReference

        public override bool LoadByReference(params object[] param)
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

        private static string getObjectByFieldCondition(Type type, out Dictionary<string, object> prms, params object[] param)
        {
            string result = string.Empty;

            prms = new Dictionary<string, object>();
            if (param != null && param.Length > 0)
            {
                StringBuilder commandText = new StringBuilder();
                for (int i = 0; i < param.Length; i += 2)
                {
                    string field_name = (string)param[i];
                    object value = param[i + 1];

                    if (i != 0)
                        commandText.Append(" AND ");

                    if (value is MLString)
                    {
                        commandText.Append(String.Format("({0}_en = @prm_en{1} OR {0}_ru = @prm_ru{1})", field_name, i));
                        prms.Add(string.Format("@prm_en{0}", i), ((MLString)value)[CultureManager.Languages.English]);
                        prms.Add(string.Format("@prm_ru{0}", i), ((MLString)value)[CultureManager.Languages.Russian]);
                    }
                    else if (value is Array)
                    {
                        if (((Array)value).Length > 0)
                        {
                            List<string> str_values = new List<string>();
                            foreach (object str_value in (Array)value)
                                str_values.Add(str_value.ToString());
                            commandText.Append(String.Format("{0} IN ({1})", field_name, String.Join(",", str_values.ToArray())));
                        }
                        else
                        {
                            commandText.Append("NULL != NULL");
                        }
                    }
                    else
                    {
                        commandText.Append(String.Format("{0} = @prm{1}", field_name, i));
                        prms.Add(string.Format("@prm{0}", i), value);
                    }
                }

                if (commandText.ToString() != "")
                    commandText.Insert(0, " WHERE ");

                result = commandText.ToString();
            }

            return result;
        }

        #endregion
    }
}
