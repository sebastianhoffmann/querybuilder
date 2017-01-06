using System.Data.Common;
using System.Collections.Generic;
using Deviax.QueryBuilder.Parts;
using System.Text;
using System;

namespace Deviax.QueryBuilder.Visitors
{
    public partial class ActualCommandResult : IVisitorResult
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
        
        public void Finished()
        {
            Command.CommandText = _sb.ToString();
            AddParameters();
            _sb.Clear();
            _sb = null;
        }

        public IVisitorResult Append(int value)
        {
            _sb.Append(value);
            return this;
        }
    }

    internal class PreparedCommandCache<T>
    {
        // ReSharper disable once StaticMemberInGenericType
        public static readonly PreparedCommand[] PreparedCommands = new PreparedCommand[32];

        public static PreparedCommand Set(int idx, PreparedCommand c)
        {
            PreparedCommands[idx] = c;
            return c;
        }

        public static PreparedCommand Get(int idx) => PreparedCommands[idx];
    }

    public partial class PreparedCommand
    {
        private readonly string _query;
        private readonly List<IParameter> _parameters;

        public PreparedCommand(string query, List<IParameter> paras)
        {
            _query = query;
            _parameters = paras;
        }

        public static PreparedCommand Store<T>(int idx, PreparedCommand c) => PreparedCommandCache<T>.Set(idx, c);
        public static PreparedCommand Get<T>(int idx) => PreparedCommandCache<T>.Get(idx);

        public DbCommand Instantiate(DbConnection con, DbTransaction tx, params IParameter[] paras)
        {
            var cmd = con.CreateCommand();
            cmd.CommandText = _query;
            cmd.Transaction = tx;
            if (paras.Length != _parameters.Count)
                throw new ArgumentException();

            foreach (var para in paras)
                para.ApplyTo(cmd);

            return cmd;
        }
    }

    public partial class PreparingCommandResult : IVisitorResult
    {
        private List<IParameter> _parameters;
        public PreparedCommand Result;
        private StringBuilder _sb;
        public void Start()
        {
            _sb = new StringBuilder();
            _parameters = new List<IParameter>();
        }

        //public Dictionary<string, object> ParameterDic = new Dictionary<String,Object>();

        public void AddParameter<T>(IParameter<T> para)
        {
            _parameters.Add(para);
        }

        public IVisitorResult Append(string str)
        {
            _sb.Append(str);
            return this;
        }



        public void Finished()
        {
            Result = new PreparedCommand(_sb.ToString(), _parameters);

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