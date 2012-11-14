using System;
using System.Collections;

namespace Core.DB.QueryStatement
{
    public class QueryStatementClause
    {
        #region [ Constructors ]

        public QueryStatementClause(string bitWiseOperator)
            : this(bitWiseOperator, string.Empty)
        { }

        public QueryStatementClause(string bitWiseOperator, string oper)
        {
            m_BitWiseOperator = bitWiseOperator;
            m_Operator = oper;
        }

        #endregion

        #region [ Fields ]

        private string m_Operator;
        private readonly string m_BitWiseOperator;

        #endregion

        #region [ Properties ]

        public string BitWiseOperator
        {
            get { return m_BitWiseOperator; }
        }

        public string Operator
        {
            get 
            {
                if (string.IsNullOrEmpty(m_Operator) && !string.IsNullOrEmpty(FieldName))
                    m_Operator = calculateOpertaor();

                return m_Operator; 
            }
        }

        public string FieldName { get; set; }

        public object Value { get; set; }

        #endregion

        public override string ToString()
        {
            if (Value == null)
                return string.Empty;

            string value = string.Empty;
            if (Operator.Equals("LIKE", StringComparison.InvariantCultureIgnoreCase))
                value = string.Format("'%{0}%'", convertValueToString(Value));
            else
                if (Operator.Equals("IN", StringComparison.InvariantCultureIgnoreCase) || Value is ICollection)
                    value = string.Format("({0})", convertValueToString(Value));
                else
                    value = convertValueToString(Value);

            return string.Format("{0} {1} {2} {3}", BitWiseOperator, FieldName, Operator, value);
        }

        private string calculateOpertaor()
        {
            if (Value is ICollection)
                return "IN";

            if (Value is string)
                return "LIKE";

            return "=";
        }

        private string convertValueToString(object value)
        {
            if (value is ICollection)
            {
                string expr = string.Empty;
                foreach (var item in (ICollection)value)
                {
                    if (!string.IsNullOrEmpty(expr))
                        expr += item is QueryStatementClause ? " " : ", ";

                    expr += item.ToString();
                }
                return expr;
            }

            if (value is int && (int)value == 0)
                return string.Empty;

            if (value is bool)
                return (bool)value ? "1" : "0";

            return value.ToString();
        }
    }
}