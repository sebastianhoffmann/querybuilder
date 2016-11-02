using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder.Visitors
{
    public class ToSqlResult : IVisitorResult
    {
        public Dictionary<string, object> ParameterDic;
        public StringBuilder StringBuilder;

        public void Start()
        {
            StringBuilder = new StringBuilder();
            ParameterDic = new Dictionary<string, object>();
        }

        public IVisitorResult Append(string str)
        {
            StringBuilder.Append(str);
            return this;
        }

        public void AddParameter<T>(IParameter<T> para)
        {
            ParameterDic[para.Name] = para.Value;
        }

        public string ParameterDescription => 
            $"{Environment.NewLine}-- Parameters: {Environment.NewLine}--{string.Join($"{Environment.NewLine}--", ParameterDic.Select(kvp => $"{kvp.Key}: {kvp.Value}"))}";

        public void Finished()
        {
            var replacers = new Dictionary<Type, Func<object, string>> {
                {typeof(string), (o) => $"'{o}'"},
                {typeof(int), (o) => $"{o}"},
                {typeof(short), (o) => $"{o}"},
                {typeof(long), (o) => $"{o}"},
                {typeof(ushort), (o) => $"{o}"},
                {typeof(DateTime), (o) => $"'{((DateTime)o).ToString("o")}'"}
            };


            foreach (var para in ParameterDic)
            {
                var t = para.Value.GetType();
                var ti = t.GetTypeInfo();

                Func<object, string> replacer = null;
                if (ti.IsArray)
                {
                    var elements = new List<object>();
                    foreach (var ele in (IEnumerable) para.Value)
                        elements.Add(ele);

                    replacer = (o) => $"ARRAY[{string.Join(", ", elements.Select(replacers[ti.GetElementType()]))}]";
                }
                else
                {
                    replacer = replacers[t];
                }

                StringBuilder.Replace($"@{para.Key}", replacer(para.Value));
            }
        }

        public IVisitorResult Append(int value)
        {
            StringBuilder.Append(value);
            return this;
        }
    }
}