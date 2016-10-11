using System;
using System.Data.Common;
using System.Text;

namespace Deviax.QueryBuilder.Visitors
{
    public class ActualCommandResult : IVisitorResult
    {
        private readonly DbConnection _con;

        public ActualCommandResult(DbConnection con)
        {
            _con = con;
        }

        public DbCommand Command;

        private StringBuilder _sb;
        public void Start()
        {
            Command = _con.CreateCommand();
            _sb = new StringBuilder();
        }

        public IVisitorResult Append(string str)
        {
            _sb.Append(str);
            return this;
        }

        public void AddParameter<T>(string name, T value)
        {
            var val = Equals(null, value) ? (object)DBNull.Value : value;

            if (Command.Parameters.Contains(name))
            {
                Command.Parameters[name].Value = val;
            }
            else
            {
                var para = Command.CreateParameter();
                para.Value = val;
                para.ParameterName = name;
                Command.Parameters.Add(para);
            }
        }

        public void Finished()
        {
            Command.CommandText = _sb.ToString();
            _sb.Clear();
            _sb = null;
        }

        public IVisitorResult Append(int value)
        {
            _sb.Append(value);
            return this;
        }
    }
}