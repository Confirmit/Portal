using System.Collections.Generic;
using System.Linq;

namespace Core.DB.QueryStatement
{
    public enum QueryStatementType
    {
        SELECT = 0,
        //UPDATE = 1,
        //INSERT = 2,
        //DELETE = 3
    }

    public class QueryStatement
    {
        #region [ Constructors ]

        public QueryStatement(QueryStatementType statementType)
        {
            m_QueryStatementType = statementType;
        }

        #endregion

        #region [ Fields ]

        private readonly QueryStatementType m_QueryStatementType;
        private List<string> m_Fields = new List<string>();
        private List<string> m_Tables = new List<string>();
        private List<QueryStatementClause> m_Clauses = new List<QueryStatementClause>();

        #endregion

        #region [ Properties ]

        public QueryStatementType QueryStatementType
        {
            get { return m_QueryStatementType; }
        }

        public List<string> Tables
        {
            get { return m_Tables; }
            set { m_Tables = value; }
        }

        public List<string> Fields
        {
            get { return m_Fields; }
            set { m_Fields = value; }
        }

        public List<QueryStatementClause> Clauses
        {
            get { return m_Clauses; }
            set { m_Clauses = value; }
        }

        #endregion

        public void ConcatClauses(List<QueryStatementClause> clausesToAdd)
        {
            foreach (var clause in clausesToAdd)
            {
                var bitWiseOperator = (string.IsNullOrEmpty(clause.BitWiseOperator) && Clauses.Count != 0)
                                          ? "AND" : clause.BitWiseOperator;

                Clauses.Add(new QueryStatementClause(bitWiseOperator, clause.Operator)
                                {
                                    FieldName = clause.FieldName,
                                    Value = clause.Value
                                });
            }
        }

        public override string ToString()
        {
            var tablesList = string.Join(", ", Tables.Select(table => string.Format("[{0}]", table)).ToArray());
            var clause = string.Join(" ", Clauses.Select(clauseItem => clauseItem.ToString()).ToArray());
            if (!string.IsNullOrEmpty(clause))
                clause = string.Format(" WHERE {0}", clause);

            var fields = Fields.Count == 0 ? new List<string> { "*" } : Fields;
            var strFields = string.Join(", ", fields.ToArray());

            return string.Format("{0} {1} FROM {2} {3}", QueryStatementType, strFields, tablesList, clause);
        }
    }
}