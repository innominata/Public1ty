using System;
using System.IO;
using System.Windows.Forms;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;

namespace AssemblyPublicizerUI
{
    public partial class MainForm : Form
    {
        private string? _draggedFilePath;

        public MainForm()
        {
            InitializeComponent();
            Text = "Public1ty";
            titleLabel.Text = "Public1ty";
        }

        // Handle drag-enter event
        private void dropPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1 && files[0].EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }
            e.Effect = DragDropEffects.None;
        }

        // Handle drag-drop event
        private void dropPanel_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1 && files[0].EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                {
                    _draggedFilePath = files[0];
                    UpdateStatus($"DLL selected: {Path.GetFileName(_draggedFilePath)}");
                    ProcessDroppedFile();
                }
            }
        }

        // Handle browse button click
        private void openFileButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "DLL files (*.dll)|*.dll";
                openDialog.Title = "Select a DLL file";

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    _draggedFilePath = openDialog.FileName;
                    UpdateStatus($"DLL selected: {Path.GetFileName(_draggedFilePath)}");
                    ProcessDroppedFile();
                }
            }
        }

        // Process the assembly to make all members public
        private void PublicizeAssembly(string inputPath, string outputPath)
        {
            try
            {
                // Show processing in the status label
                UpdateStatus($"Processing {Path.GetFileName(inputPath)}...");
                LogMessage($"Starting to process: {inputPath}");
                LogMessage($"Output will be saved to: {outputPath}");

                bool stripMethodBodies = stripMethodsCheckBox.Checked;
                if (stripMethodBodies)
                {
                    LogMessage("Method body stripping is enabled - proprietary code will be removed");
                }

                // Get the directory of the input assembly to search for dependencies
                string assemblyDirectory = Path.GetDirectoryName(inputPath) ?? "";

                // Create and configure a custom assembly resolver
                var resolver = new CustomAssemblyResolver(assemblyDirectory, this);
                UpdateStatus($"Configured assembly resolver");
                
                // Read the assembly with resolver
                LogMessage("Configuring assembly reader parameters...");
                var readerParams = new ReaderParameters 
                { 
                    ReadSymbols = false,
                    AssemblyResolver = resolver,
                    ReadingMode = ReadingMode.Immediate,
                    InMemory = true,
                    ApplyWindowsRuntimeProjections = false
                };
                
                UpdateStatus($"Loading assembly and resolving references...");
                LogMessage("Reading assembly...");
                var assembly = AssemblyDefinition.ReadAssembly(inputPath, readerParams);
                UpdateStatus($"Assembly loaded successfully");
                LogMessage($"Successfully loaded assembly: {assembly.Name.Name}, Version={assembly.Name.Version}");
                
                LogMessage("Starting to process assembly...");
                int typesProcessed = 0;
                int methodsProcessed = 0;
                int methodsStripped = 0;
                int fieldsProcessed = 0;
                int propertiesProcessed = 0;

                // Process all types in the assembly
                LogMessage($"Processing {assembly.Modules.Count} modules...");
                foreach (var module in assembly.Modules)
                {
                    LogMessage($"Module: {module.Name} with {module.Types.Count} types");
                    foreach (var type in module.Types)
                    {
                        LogMessage($"Processing type: {type.FullName}");
                        var stats = PublicizeType(type, stripMethodBodies);
                        typesProcessed += stats.TypesProcessed;
                        methodsProcessed += stats.MethodsProcessed;
                        methodsStripped += stats.MethodsStripped;
                        fieldsProcessed += stats.FieldsProcessed;
                        propertiesProcessed += stats.PropertiesProcessed;
                    }
                }

                // Write the modified assembly
                UpdateStatus($"Writing modified assembly...");
                LogMessage($"Writing output to: {outputPath}");
                var writerParams = new WriterParameters { WriteSymbols = false };
                assembly.Write(outputPath, writerParams);

                string summary = $"Done! Modified {typesProcessed} types, {methodsProcessed} methods, {fieldsProcessed} fields, and {propertiesProcessed} properties.";
                if (stripMethodBodies)
                {
                    summary += $" Stripped {methodsStripped} method bodies.";
                }
                UpdateStatus(summary);
                LogMessage("Publicizing completed successfully!");
                
                string successMessage = $"Assembly successfully processed!\n\nModified:\n- {typesProcessed} types\n- {methodsProcessed} methods\n- {fieldsProcessed} fields\n- {propertiesProcessed} properties";
                if (stripMethodBodies)
                {
                    successMessage += $"\n\nStripped implementation from {methodsStripped} methods, leaving only signatures.";
                }
                
                MessageBox.Show(successMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (AssemblyResolutionException ex)
            {
                string errorMessage = $"Failed to resolve a referenced assembly: {ex.AssemblyReference.Name}\n\n" +
                    "This usually happens when the application can't find dependencies of the DLL you're trying to publicize.\n\n" +
                    "Make sure all referenced DLLs are in the same folder as the target DLL.\n\n" +
                    "For Unity games, try copying the DLL to a folder that contains all game assemblies.";
                
                UpdateStatus($"Error: {ex.Message}");
                LogMessage($"ERROR: Assembly Resolution Exception: {ex.Message}");
                LogMessage($"Failed to resolve: {ex.AssemblyReference.FullName}");
                LogMessage(ex.StackTrace ?? "No stack trace available");
                
                MessageBox.Show(errorMessage, "Assembly Resolution Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}");
                LogMessage($"ERROR: {ex.GetType().Name}: {ex.Message}");
                LogMessage(ex.StackTrace ?? "No stack trace available");
                
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Custom assembly resolver that handles Unity-specific patterns
        /// </summary>
        private class CustomAssemblyResolver : DefaultAssemblyResolver
        {
            private readonly List<string> _searchDirectories = new List<string>();
            private readonly MainForm _form;

            public CustomAssemblyResolver(string primaryDirectory, MainForm form)
            {
                _form = form;
                
                // Add the primary directory first
                _searchDirectories.Add(primaryDirectory);
                AddSearchDirectory(primaryDirectory);
                _form.LogMessage($"Added search directory: {primaryDirectory}");

                // Add common Unity subdirectories if they exist
                string managedDir = Path.Combine(primaryDirectory, "Managed");
                if (Directory.Exists(managedDir))
                {
                    _searchDirectories.Add(managedDir);
                    AddSearchDirectory(managedDir);
                    _form.LogMessage($"Added search directory: {managedDir}");
                }

                // Check for Unity data folder pattern
                string dataDir = Path.Combine(primaryDirectory, "..", "Data");
                if (Directory.Exists(dataDir))
                {
                    string dataManagedDir = Path.Combine(dataDir, "Managed");
                    if (Directory.Exists(dataManagedDir))
                    {
                        _searchDirectories.Add(dataManagedDir);
                        AddSearchDirectory(dataManagedDir);
                        _form.LogMessage($"Added search directory: {dataManagedDir}");
                    }
                }

                // Go up one directory and check if there's a Managed folder there
                string parentDir = Path.GetDirectoryName(primaryDirectory) ?? "";
                if (Directory.Exists(parentDir))
                {
                    string parentManagedDir = Path.Combine(parentDir, "Managed");
                    if (Directory.Exists(parentManagedDir))
                    {
                        _searchDirectories.Add(parentManagedDir);
                        AddSearchDirectory(parentManagedDir);
                        _form.LogMessage($"Added search directory: {parentManagedDir}");
                    }
                }

                // Special handler for Unity's ResolveFailure event
                this.ResolveFailure += (sender, reference) =>
                {
                    _form.LogMessage($"Trying to resolve reference: {reference.Name}");
                    
                    // Try to find any version of the assembly
                    string baseName = reference.Name.Split(',')[0];
                    
                    foreach (var dir in _searchDirectories)
                    {
                        // Try exact name
                        string potentialPath = Path.Combine(dir, baseName + ".dll");
                        if (File.Exists(potentialPath))
                        {
                            _form.LogMessage($"Found candidate: {potentialPath}");
                            try
                            {
                                var result = AssemblyDefinition.ReadAssembly(potentialPath, new ReaderParameters 
                                { 
                                    AssemblyResolver = this,
                                    ReadingMode = ReadingMode.Deferred 
                                });
                                _form.LogMessage($"Successfully resolved: {reference.Name} -> {potentialPath}");
                                return result;
                            }
                            catch (Exception ex)
                            {
                                _form.LogMessage($"Failed to load {potentialPath}: {ex.Message}");
                            }
                        }
                        
                        // Look for files that start with the base name
                        try
                        {
                            var matchingFiles = Directory.GetFiles(dir, baseName + "*.dll");
                            foreach (var file in matchingFiles)
                            {
                                _form.LogMessage($"Found candidate by pattern: {file}");
                                try
                                {
                                    var result = AssemblyDefinition.ReadAssembly(file, new ReaderParameters 
                                    { 
                                        AssemblyResolver = this,
                                        ReadingMode = ReadingMode.Deferred
                                    });
                                    _form.LogMessage($"Successfully resolved: {reference.Name} -> {file}");
                                    return result;
                                }
                                catch (Exception ex)
                                {
                                    _form.LogMessage($"Failed to load {file}: {ex.Message}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _form.LogMessage($"Error searching for files in {dir}: {ex.Message}");
                        }
                    }
                    
                    _form.LogMessage($"Failed to resolve: {reference.Name}");
                    return null;
                };
            }

            public new List<string> GetSearchDirectories()
            {
                return _searchDirectories;
            }
        }

        // Make a type and all its members public
        private (int TypesProcessed, int MethodsProcessed, int MethodsStripped, int FieldsProcessed, int PropertiesProcessed) PublicizeType(TypeDefinition type, bool stripMethodBodies)
        {
            int typesProcessed = 1; // Count this type
            int methodsProcessed = 0;
            int methodsStripped = 0;
            int fieldsProcessed = 0;
            int propertiesProcessed = 0;

            // Make the type public
            if (type.IsNested)
                type.IsNestedPublic = true;
            else
                type.IsPublic = true;

            // Make all methods public
            foreach (var method in type.Methods)
            {
                bool methodModified = false;
                
                if (!method.IsPublic && !method.IsConstructor)
                {
                    method.IsPublic = true;
                    methodModified = true;
                }
                else if (method.IsConstructor && !method.IsStatic)
                {
                    method.IsPublic = true;
                    methodModified = true;
                }
                
                if (methodModified)
                    methodsProcessed++;
                    
                // Strip method body if requested and applicable
                if (stripMethodBodies && method.HasBody && !method.IsAbstract && !method.IsRuntime)
                {
                    // Special cases where we don't want to strip the body
                    bool isSpecialCase = false;
                    
                    // Don't strip parameterless constructors as they're needed for object creation
                    if (method.IsConstructor && method.Parameters.Count == 0)
                        isSpecialCase = true;
                        
                    // Don't strip property getters/setters that just access a field
                    if ((method.IsSetter || method.IsGetter) && method.Body.Instructions.Count <= 3)
                        isSpecialCase = true;
                        
                    if (!isSpecialCase)
                    {
                        StripMethodBody(method);
                        methodsStripped++;
                    }
                }
            }

            // Make all fields public
            foreach (var field in type.Fields)
            {
                if (!field.IsPublic)
                {
                    field.IsPublic = true;
                    fieldsProcessed++;
                }
            }

            // Make all properties public
            foreach (var property in type.Properties)
            {
                bool propertyModified = false;
                
                if (property.GetMethod != null && !property.GetMethod.IsPublic)
                {
                    property.GetMethod.IsPublic = true;
                    propertyModified = true;
                }
                
                if (property.SetMethod != null && !property.SetMethod.IsPublic)
                {
                    property.SetMethod.IsPublic = true;
                    propertyModified = true;
                }
                
                if (propertyModified)
                    propertiesProcessed++;
            }

            // Process nested types recursively
            foreach (var nestedType in type.NestedTypes)
            {
                var stats = PublicizeType(nestedType, stripMethodBodies);
                typesProcessed += stats.TypesProcessed;
                methodsProcessed += stats.MethodsProcessed;
                methodsStripped += stats.MethodsStripped;
                fieldsProcessed += stats.FieldsProcessed;
                propertiesProcessed += stats.PropertiesProcessed;
            }
            
            return (typesProcessed, methodsProcessed, methodsStripped, fieldsProcessed, propertiesProcessed);
        }

        // Strip the implementation from a method, leaving only the signature
        private void StripMethodBody(MethodDefinition method)
        {
            if (!method.HasBody)
                return;
            
            // Create a new empty method body
            method.Body = new MethodBody(method);
            
            var il = method.Body.GetILProcessor();
            
            // For non-void methods, we need to add a default return value
            if (method.ReturnType.MetadataType != MetadataType.Void)
            {
                if (method.ReturnType.IsValueType)
                {
                    // For value types, create a default instance
                    var variable = new VariableDefinition(method.ReturnType);
                    method.Body.Variables.Add(variable);
                    il.Append(il.Create(OpCodes.Ldloca_S, variable));
                    il.Append(il.Create(OpCodes.Initobj, method.ReturnType));
                    il.Append(il.Create(OpCodes.Ldloc_0));
                }
                else
                {
                    // For reference types, return null
                    il.Append(il.Create(OpCodes.Ldnull));
                }
            }
            
            // Add return instruction
            il.Append(il.Create(OpCodes.Ret));
        }

        // Update the log textbox on the UI thread
        private void UpdateStatus(string message)
        {
            LogMessage(message);
        }

        // Add a message to the log textbox
        private void LogMessage(string message)
        {
            if (logTextBox.InvokeRequired)
            {
                logTextBox.Invoke(new Action<string>(LogMessage), message);
            }
            else
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
                logTextBox.AppendText($"[{timestamp}] {message}{Environment.NewLine}");
                logTextBox.SelectionStart = logTextBox.Text.Length;
                logTextBox.ScrollToCaret();
                Application.DoEvents();
            }
        }

        // Process the file when dropped
        private void ProcessDroppedFile()
        {
            if (string.IsNullOrEmpty(_draggedFilePath) || !File.Exists(_draggedFilePath))
            {
                UpdateStatus("Invalid file path.");
                return;
            }

            if (!_draggedFilePath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                UpdateStatus("Only DLL files are supported.");
                return;
            }

            // Generate default output path
            string fileName = Path.GetFileNameWithoutExtension(_draggedFilePath);
            string directory = Path.GetDirectoryName(_draggedFilePath) ?? "";
            string defaultOutputPath = Path.Combine(directory, $"{fileName}_public.dll");

            // Ask user for save location
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "DLL files (*.dll)|*.dll";
                saveDialog.Title = "Save Publicized Assembly";
                saveDialog.FileName = $"{fileName}_public.dll";
                saveDialog.InitialDirectory = directory;

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    PublicizeAssembly(_draggedFilePath, saveDialog.FileName);
                }
                else
                {
                    UpdateStatus("Operation cancelled.");
                }
            }
        }
    }
}
