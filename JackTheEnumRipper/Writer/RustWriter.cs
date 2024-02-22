﻿using System.IO;
using System;
using System.Linq;

class RustWriter : IEnumWriter
{
    private readonly string _outputDir;

    public RustWriter(string outputDir)
    {
        _outputDir = outputDir;
    }

    public void WriteEnum(Type enumType, string fileName)
    {
        var filePath = Path.Combine(_outputDir, $"{fileName}.rs");

        using (StreamWriter file = new StreamWriter(filePath))
        {
            // Rust naming convention prefers PascalCase for enum names
            string rustEnumName = ConvertToPascalCase(enumType.Name);

            // Start the enum definition
            file.WriteLine($"// Generated by JackTheEnumRipper");
            file.WriteLine($"#[repr(C)]"); // Assuming C representation for compatibility
            file.WriteLine($"enum {rustEnumName} {{");

            var values = Enum.GetValues(enumType);
            for (int i = 0; i < values.Length; i++)
            {
                var value = values.GetValue(i);
                // Convert each enum value name to PascalCase, assuming C# might use UPPER_CASE or other conventions
                string rustVariantName = ConvertToPascalCase(value.ToString());
                var convertedValue = Convert.ChangeType(value, Enum.GetUnderlyingType(enumType));
                WriteVariant(file, rustVariantName, convertedValue);
            }

            // End the enum definition
            file.WriteLine("}");
        }
    }

    private void WriteVariant(StreamWriter file, string name, object value)
    {
        // In Rust, enum variants are typically just names, but you can attach values in various ways
        // Here we're assuming a simple enum without attaching data to variants directly
        file.WriteLine($"    {name} = {value},");
    }

    private string ConvertToPascalCase(string input)
    {
        // Simple conversion to PascalCase for demonstration; consider edge cases and improvements
        return string.Concat(input.Split(new char[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower()));
    }
}
