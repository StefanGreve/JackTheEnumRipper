using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using JackTheEnumRipper.Core;
using JackTheEnumRipper.Interfaces;
using JackTheEnumRipper.Models;

using Microsoft.Extensions.Options;

namespace Serializer
{
    public class RustSerializer(IOptions<AppSettings> appSettings) : ISerializer
    {
        public Format Format => Format.Rust;

        private readonly AppSettings _appSettings = appSettings.Value;

        public void Serialize(IEnumerable<AbstractEnum> enums, string path)
        {
            var builder = new StringBuilder();
            string identation = this._appSettings.Indentation ?? "\t";

            if (!string.IsNullOrEmpty(this._appSettings.Comment))
            {
                builder.AppendLine($"// {this._appSettings.Comment}{Environment.NewLine}");
            }

            foreach (IGrouping<string, AbstractEnum> group in enums.GroupBy(x => x.Namespace))
            {
                using IEnumerator<AbstractEnum> enumerator = group.GetEnumerator();
                string[] namespaces = group.Key.Split('.');
                int maxIdentation = namespaces.Length;

                while (enumerator.MoveNext())
                {
                    AbstractEnum @enum = enumerator.Current;

                    for (int i = 0; i < maxIdentation; i++)
                    {
                        builder.AppendLine($"{(i == 0 ? string.Empty : identation.Repeat(i))}mod {@namespaces[i]} {{");
                    }

                    string enumIdentation = identation.Repeat(maxIdentation);
                    string fieldIdentation = identation.Repeat(maxIdentation + 1);

                    builder.AppendLine($"{enumIdentation}// type={@enum.Type}");
                    builder.AppendLine($"{enumIdentation}{(@enum.IsPublic ? "pub" : string.Empty)} enum {@enum.Name} {{");

                    foreach (AbstractField field in @enum.Fields)
                    {
                        builder.AppendLine($"{fieldIdentation}{field.Name} = {field.Value},");
                    }

                    for (int i = maxIdentation; i >= 0; i--)
                    {
                        builder.AppendLine($"{(i == 0 ? string.Empty : identation.Repeat(i))}}}");
                    }
                }
            }

            var encoding = Encoding.GetEncoding(this._appSettings.Encoding);
            string content = builder.ToString();
            File.WriteAllText(path, content, encoding);
        }
    }
}