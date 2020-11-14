using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Save_Editor.Models;

namespace Save_Editor {
    public partial class MainWindow {
        private const           string SAVE_FILE_FILTER = "Cloudpunk Save|Slot*.sav";
        private static readonly string TARGET_FOLDER    = Environment.ExpandEnvironmentVariables(@"%LOCALAPPDATA%Low\ION LANDS\Cloudpunk\");

        public SaveData saveData { get; private set; }
        public string   targetFile;

        public MainWindow() {
            if (!LoadFile()) {
                Application.Current.Shutdown();
                return;
            }

            InitializeComponent();

            SetupAppWideBinding(new KeyGesture(Key.S, ModifierKeys.Control), SaveFile); // Ctrl+S.
        }

        private bool LoadFile() {
            var target = GetOpenTarget();
            if (string.IsNullOrEmpty(target)) {
                Application.Current.Shutdown();
                return false;
            }

            targetFile = target;

            using var reader = new BinaryReader(File.Open(target, FileMode.Open, FileAccess.Read, FileShare.Read));

            saveData = reader.ReadSaveData();

            return true;
        }

        private void SaveFile() {
            var target = GetSaveTarget();
            if (string.IsNullOrEmpty(target)) return;

            using var writer = new BinaryWriter(File.Open(target, FileMode.Create, FileAccess.Write, FileShare.Read));

            writer.Write(saveData);
        }

        private string GetOpenTarget() {
            var ofdResult = new OpenFileDialog {
                Filter           = SAVE_FILE_FILTER,
                Multiselect      = false,
                InitialDirectory = targetFile == null ? TARGET_FOLDER : Path.GetDirectoryName(targetFile) ?? TARGET_FOLDER
            };
            ofdResult.ShowDialog();

            return ofdResult.FileName;
        }

        private string GetSaveTarget() {
            var sfdResult = new SaveFileDialog {
                Filter           = SAVE_FILE_FILTER,
                FileName         = $"{Path.GetFileNameWithoutExtension(targetFile)}",
                AddExtension     = true,
                InitialDirectory = targetFile == null ? TARGET_FOLDER : Path.GetDirectoryName(targetFile) ?? TARGET_FOLDER
            };
            return sfdResult.ShowDialog() == true ? sfdResult.FileName : null;
        }

        private void SetupAppWideBinding(KeyGesture keyGesture, Action onPress) {
            // Setup App-wide Binding.
            var command = new RoutedCommand();
            var ib      = new InputBinding(command, keyGesture);
            InputBindings.Add(ib);
            // Bind handler.
            var cb = new CommandBinding(command);
            cb.Executed += (sender, args) => onPress.Invoke();
            CommandBindings.Add(cb);
        }
    }
}