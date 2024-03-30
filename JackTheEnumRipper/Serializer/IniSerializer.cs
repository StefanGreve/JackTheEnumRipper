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
    public class IniSerializer(IOptions<AppSettings> appSettings) : ISerializer
    {
        public Format Format => Format.Ini;

        private readonly AppSettings _appSettings = appSettings.Value;

        public void Serialize(IEnumerable<AbstractEnum> enums, string path)
        {
            var sections = enums.Select(@enum => new Section
            {
                Comment = $"scope={(@enum.IsPublic ? "public" : "internal")},type={@enum.Type}",
                Name = $"{@enum.Namespace}.{@enum.Name}",
                Settings = @enum.Fields.Select(field => new Setting
                {
                    Name = field.Name,
                    Value = field.Value,
                }),
            }); ;

            var encoding = Encoding.GetEncoding(this._appSettings.Encoding);
            var ini = new Ini(sections, this._appSettings.Comment, commentSymbol: '#');
            File.WriteAllText(path, ini.ToString(), encoding);
        }
    }
}