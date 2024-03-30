<p align="center">
	<img src="https://github.com/tolik518/JackTheEnumRipper/assets/3026792/2bd482c4-ef87-40fd-aa16-57c87af617de">
</p>
<h1 align="center">Jack the Enum Ripper</h1>

<p align="center">
	Jack the Enum Ripper is a CLI tool designed to extract enums from .NET assemblies and output them in various formats.
</p>

## Features

- **Dump Enums**: Load a .NET assembly (.exe or .dll) to dump all enumeration types.
- **Output Formats**: Supports exporting enums into multiple formats, including C#, JSON, INI, PHP, Rust, and Python adhering to each language's conventions.

## Getting Started

### Usage

Run `JackTheEnumRipper.exe` from the command line with the following syntax:

```powershell
JackTheEnumRipper.exe export --path <assembly> [--format <format>]
```

Alternatively, use the `--help` option to read the help manual.

### Supported Formats

- `json`
- `ini`
- `php`
- `rust`
- `csharp`
- `python`

A default format or encoding can be configured in the `appsettings.json` configuration file.

### Example

To extract enums from `MyExecutable.exe` in Rust format:

```powershell
JackTheEnumRipper.exe export --path ./path/to/MyExecutable.exe --format rust
```

This will command will output a `enum.rs` file in the current working directory.

## Building

```powershell
dotnet publish -c Release -r win-x64 --framework net8.0 --self-contained -o ./bin/publish/release
```

## Contributing

Contributions are welcome! If you have suggestions for improvements, please fork the repository and submit a pull request, or open an issue to discuss your ideas.

## License

Jack the Enum Ripper is open-source software licensed under the MIT License. See the `LICENSE` file for more details.
