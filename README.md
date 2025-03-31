# Public1ty

*Making private assemblies public - your code's new best friend*

An easy to use application that allows you to easily create public versions of .NET assemblies. This is particularly useful for game modding, where you need to access private members of game assemblies without using reflection.

## Purpose

When creating mods for Unity and other .NET-based games, you often need to access internal members (methods, fields, properties, and types) that are marked as private or internal. While reflection can be used to access these members at runtime, it's cumbersome and doesn't provide IDE support like auto-completion.

Public1ty creates modified copies of game assemblies where all members are made public, allowing you to:

- Get full IntelliSense/auto-completion in your IDE
- Access private members directly in your code (with "Allow unsafe code" enabled)
- Avoid tedious reflection code
- Strip proprietary implementations to create "skeleton" assemblies
- Easily reference the original assembly at runtime while developing against the publicized version

## Features

- Simple drag-and-drop interface for DLL files
- Code stripping to remove proprietary implementations while keeping signatures
- Detailed logging of the publicizing process
- Smart assembly resolution for dependencies (especially for Unity games)
- Statistics showing how many types, methods, fields, and properties were modified
- Single-file executable with no installation required

## Usage

1. Download the latest release from the releases page
2. Make sure you have the [.NET 6.0 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) installed
3. Run `Public1ty.exe`
4. Either:
   - Drag and drop a DLL file onto the drop zone, or
   - Click "Browse for DLL..." to select a file
5. Choose whether to strip method implementations (checked by default)
6. Choose where to save the publicized assembly
7. The application will create a new assembly with all members public

### Required Prerequisites

- [.NET 6.0 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) (x64)
- Windows 10 or later

### Code Stripping

The "Strip proprietary code" option (enabled by default) removes the actual implementation from methods, leaving only their signatures. This is useful for:

- Ensuring your mod doesn't accidentally contain proprietary game code
- Creating reference-only assemblies that are much smaller in size
- Focusing exclusively on the API structure without implementation details
- Reducing the risk of legal issues when sharing modding references

### Important Notes

- For Unity games, it's recommended to:
  - Process all assemblies in a common folder that contains all dependencies
  - Compile your mod with the "Allow unsafe code" option enabled in your project settings
  - Reference the publicized assemblies during development, but distribute your mod with references to the original assemblies

## Development

This project uses:
- [.NET 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Windows Forms
- [Mono.Cecil](https://github.com/jbevain/cecil) for assembly manipulation

To build from source:
1. Clone this repository
2. Open the solution in Visual Studio 2022 or later (or use `dotnet` CLI)
3. Build the solution
4. Run the application

## Acknowledgments

This project was inspired by [CabbageCrow's AssemblyPublicizer](https://github.com/CabbageCrow/AssemblyPublicizer), which provides similar functionality as a command-line tool. The UI version aims to make the process more user-friendly while adding features like detailed logging and improved dependency resolution.

## License

This project is licensed under the MIT License - see the LICENSE file for details. 