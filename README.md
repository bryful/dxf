# dxf


## Overview


## Projects

- **JApp**: Windows Forms GUI application for interactive JavaScript execution
- **JCmd**: Command-line tool for executing JavaScript files
- **fls**: File listing utility

## JavaScript API Reference

### Global Functions

#### Output Functions
```javascript
write(object)           // Write text to output (without newline)
writeln(object)        // Write text to output (with newline)
cls()                  // Clear output screen
```

#### Dialog Functions
```javascript
alert(message, caption)                  // Show alert dialog
answerDialog(message, caption)           // Show Yes/No dialog, returns bool
inputBox(message, caption2, caption)     // Show input dialog, returns string or null
```

#### Utility Functions
```javascript
calc(expression)       // Evaluate mathematical expression, returns number
                      // Supports: sin, cos, tan, sqrt, abs, asin, acos, atan,
                      //          log, log10, exp, floor, ceil, round, min, max, pow
ls(path)              // List files in directory, returns string
```

**Example:**
```javascript
writeln("Hello World!");
var result = calc("sin(3.14159/2) + sqrt(16)");
writeln("Result: " + result);

if (answerDialog("Continue?", "Question")) {
    var name = inputBox("Enter your name:", "Input", "Name");
    alert("Hello " + name, "Greeting");
}
```

### App Object

Static properties and methods for application information.

#### Properties
```javascript
App.exeFilePath                    // Full path to executable
App.exeFileName                    // Executable filename
App.exeFileDir                     // Executable directory
App.exeFileNameWithoutExtension    // Executable name without extension
App.prefFilePath                   // User preference directory path
App.prefFileName                   // Preference file path
```

#### Static Methods
```javascript
App.callCmd(command)               // Execute cmd.exe command, returns output string
App.callProcess(exePath, args)     // Execute process, returns output string
App.commandLine()                  // Get command line arguments as array
App.commandLineJson()              // Get command line arguments as JSON string
App.getEnv(variableName)           // Get environment variable, returns string or null
```

**Example:**
```javascript
writeln("Running from: " + App.exeFileDir);
var output = App.callCmd("dir");
writeln(output);

var args = App.commandLine();
writeln("Arguments: " + args.join(", "));
```

### FileItem Object

File and directory manipulation object.

#### Constructor
```javascript
var file = new FileItem(path);     // Create FileItem from path
```

#### Instance Properties
```javascript
file.fullName          // Full path (read/write)
file.name              // Filename
file.ext               // Extension
file.directory         // Directory path
file.parent            // Parent directory as FileItem
file.length            // File size in bytes
file.created           // Creation DateTime
file.modified          // Last modified DateTime
file.exists            // Check if exists
file.isDirectory       // Check if directory
file.hidden            // Hidden attribute (read/write)
```

#### Instance Methods
```javascript
file.setFullName(path)             // Set file path, returns bool
file.remove()                      // Delete file/folder, returns bool
file.rename(newName)               // Rename file/folder, returns bool
file.copy(destPath)                // Copy file, returns bool
file.move(destPath)                // Move file/folder, returns bool
file.getFiles()                    // Get files array in directory
file.getDirectories()              // Get subdirectories array
file.create()                      // Create directory
file.execute()                     // Execute file or open folder
file.resolve()                     // Resolve shortcut, returns FileItem
file.resolvePath()                 // Get shortcut target path
```

#### Static Methods
```javascript
FileItem.encode(string)            // URL encode string
FileItem.decode(string)            // URL decode string
FileItem.readAllText(path, encoding)   // Read text file (encoding: "utf-8", "utf-16", "shift_jis")
FileItem.writeAllText(path, content, encoding) // Write text file
FileItem.getFilesFromDir(path)     // Get files array from directory
FileItem.getDirectoriesFromDir(path) // Get directories array
FileItem.createFolder(path)        // Create folder
FileItem.ls(path)                  // List files in directory
```

#### Static Properties (Special Folders)
```javascript
FileItem.currentPath               // Current working directory
FileItem.appDataPath               // %AppData%
FileItem.localAppDataPath          // %LocalAppData%
FileItem.myDocumentsPath           // My Documents
FileItem.desktopPath               // Desktop
FileItem.tempPath                  // Temp folder
FileItem.userProfilePath           // User profile
FileItem.programFilesPath          // Program Files
FileItem.systemPath                // System folder
FileItem.commonAppDataPath         // Common AppData
FileItem.startupPath               // Startup folder
```

**Example:**
```javascript
var file = new FileItem("C:\\test.txt");
if (file.exists) {
    var content = FileItem.readAllText(file.fullName, "utf-8");
    writeln(content);
}

var desktop = new FileItem(FileItem.desktopPath);
var files = desktop.getFiles();
for (var i = 0; i < files.length; i++) {
    writeln(files[i].name + " - " + files[i].length + " bytes");
}
```

### FileDialog Object

Modern file and folder selection dialogs.

#### Constructor
```javascript
var dlg = new FileDialog("foo.txt");
```

#### Properties
```javascript
dlg.fileName           // Selected filename (read/write)
dlg.initialDirectory   // Initial directory
dlg.title              // Dialog title
dlg.filter             // File filter (e.g., "Text(*.txt)|*.txt|All(*.*)|*.*")
dlg.filterIndex        // Filter index (1-based)
dlg.defaultExt         // Default extension
dlg.multiSelect        // Enable multiple selection
```

#### Methods
```javascript
dlg.openDialog()                   // Show open file dialog
dlg.openDialog(filename)
dlg.openDialog(filename, title)

dlg.saveDialog()                   // Show save file dialog
dlg.saveDialog(filename)
dlg.saveDialog(filename, title)

dlg.folderDialog()                 // Show folder picker dialog (modern UI)
dlg.folderDialog(title)
dlg.folderDialog(initialDir, title)
```

**Example:**
```javascript
var dlg = new FileDialog();
dlg.filter = "JavaScript(*.js)|*.js|Text(*.txt)|*.txt|All(*.*)|*.*";
dlg.title = "Select Script File";

var filename = dlg.openDialog();
if (filename) {
    writeln("Selected: " + filename);
    var content = FileItem.readAllText(filename, "utf-8");
    writeln(content);
}

// Folder selection
var folder = dlg.folderDialog("Select Output Folder");
if (folder) {
    writeln("Selected folder: " + folder);
}
```

## Usage

### JApp (GUI)
1. Launch JApp.exe
2. Enter JavaScript code in the editor
3. Click Execute button to run the code
4. View output in the output panel

### JCmd (Command Line)
```bash
JCmd <scriptfile> [arg1] [arg2] ...
```

**Example:**
```bash
JCmd test.js
JCmd myscript arg1 arg2
```

If no extension is provided, `.js` is automatically appended.

## Script Default Folders

Scripts are automatically searched in:
1. `%AppData%\<AppName>\scripts\`
2. `<ExecutableDir>\<AppName>_scripts\`

## Library Dependencies

- [Jint](https://github.com/sebastienros/jint) - JavaScript interpreter for .NET
- [NCalc](https://ncalc.github.io/ncalc/articles/index.html) - Mathematical expression evaluator
- [WindowsAPICodePack-Shell](https://github.com/aybe/Windows-API-Code-Pack-1.1) - Modern Windows dialogs

## Requirements

- .NET 10
- Visual Studio 2022 or later
- Windows 10/11

## License

This software is released under the MIT License, see LICENSE


## Example Scripts

### Hello World
```javascript
writeln("Hello World!");
var name = inputBox("What's your name?", "Input", "Name");
if (name) {
    alert("Hello, " + name + "!", "Greeting");
}
```

### File Processing
```javascript
var dlg = new FileDialog();
dlg.filter = "Text Files(*.txt)|*.txt";
var filename = dlg.openDialog("Select a text file");

if (filename) {
    var content = FileItem.readAllText(filename, "utf-8");
    var lines = content.split("\n");
    writeln("File has " + lines.length + " lines");
    
    // Process each line
    for (var i = 0; i < lines.length; i++) {
        writeln((i + 1) + ": " + lines[i]);
    }
}
```

### Mathematical Calculations
```javascript
writeln("Trigonometry Example:");
var angle = 45;
var radians = angle * Math.PI / 180;

var result1 = calc("sin(" + radians + ")");
var result2 = calc("cos(" + radians + ")");
var result3 = calc("sqrt(2) / 2");

writeln("sin(45) = " + result1);
writeln("cos(45) = " + result2);
writeln("√2/2 = " + result3);
```

### Directory Listing
```javascript
var dlg = new FileDialog();
var folder = dlg.folderDialog("Select a folder to list");

if (folder) {
    var dir = new FileItem(folder);
    var files = dir.getFiles();
    
    writeln("Files in " + folder + ":");
    writeln("─".repeat(50));
    
    for (var i = 0; i < files.length; i++) {
        var size = (files[i].length / 1024).toFixed(2);
        writeln(files[i].name + " (" + size + " KB)");
    }
    
    writeln("─".repeat(50));
    writeln("Total: " + files.length + " files");
}

## Authors

**bry-ful** (Hiroshi Furuhashi)  
Twitter: [@bryful](https://twitter.com/bryful)

