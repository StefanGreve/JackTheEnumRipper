using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;

using JackTheEnumRipper.Interfaces;
using JackTheEnumRipper.Models;

using Microsoft.Extensions.Options;

namespace Serializer
{
    public class PhpSerializer(IOptions<AppSettings> appSettings) : ISerializer
    {
        public Format Format => Format.Php;

        private readonly AppSettings _appSettings = appSettings.Value;

        public void Serialize(IEnumerable<AbstractEnum> enums, string path)
        {
            var builder = new StringBuilder();
            string identation = this._appSettings.Indentation ?? "\t";

            builder.AppendLine("<?php");
            builder.AppendLine();

            if (!string.IsNullOrEmpty(this._appSettings.Comment))
            {
                builder.AppendLine($"// {this._appSettings.Comment}{Environment.NewLine}");
            }

            foreach (var group in enums.GroupBy(x => x.Namespace))
            {
                using IEnumerator<AbstractEnum> enumerator = group.GetEnumerator();
                string @namespace = group.Key.Replace(".", "\\");
                builder.AppendLine($"namespace {@namespace} {{");

                while (enumerator.MoveNext())
                {
                    AbstractEnum @enum = enumerator.Current;
                    builder.AppendLine($"{identation}// scope={(@enum.IsPublic ? "public" : "internal")},type={@enum.Type}");
                    builder.AppendLine($"{identation}class {@enum.Name} {{");

                    foreach (AbstractField field in @enum.Fields)
                    {
                        builder.AppendLine($"{identation}{identation}const {field.Name} = {field.Value};");
                    }

                    builder.AppendLine($"{identation}}}");
                }

                builder.AppendLine("}");
            }

            builder.AppendLine();
            builder.AppendLine("?>");

            var encoding = Encoding.GetEncoding(this._appSettings.Encoding);
            string content = builder.ToString();
            File.WriteAllText(path, content, encoding);
        }
    }
}