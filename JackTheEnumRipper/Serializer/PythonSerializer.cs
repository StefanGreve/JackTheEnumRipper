using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using JackTheEnumRipper.Interfaces;
using JackTheEnumRipper.Models;

using Microsoft.Extensions.Options;

namespace JackTheEnumRipper.Serializer
{
    public class PythonSerializer(IOptions<AppSettings> appSettings) : ISerializer
    {
        public Format Format => Format.Python;

        private readonly AppSettings _appSettings = appSettings.Value;

        public void Serialize(IEnumerable<AbstractEnum> enums, string path)
        {
            var builder = new StringBuilder();
            string identation = this._appSettings.Indentation ?? "\t";

            if (!string.IsNullOrEmpty(this._appSettings.Comment))
            {
                builder.AppendLine($"# {this._appSettings.Comment}");
            }

            builder.AppendLine($"from enum import Enum, unique");

            foreach (AbstractEnum @enum in enums)
            {
                builder.AppendLine(Environment.NewLine);
                builder.AppendLine($"# namespace={@enum.Namespace},scope={(@enum.IsPublic ? "public" : "internal")},type={@enum.Type}");
                builder.AppendLine($"@unique");
                builder.AppendLine($"class {@enum.Name}(Enum):");
                
                foreach (AbstractField field in @enum.Fields)
                {
                    builder.AppendLine($"{identation}{field.Name.ToUpper()} = {field.Value}");
                }
            }

            var encoding = Encoding.GetEncoding(this._appSettings.Encoding);
            string content = builder.ToString();
            File.WriteAllText(path, content, encoding);
        }
    }
}
