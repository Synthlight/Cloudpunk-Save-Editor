using System.Collections.ObjectModel;
using System.IO;

namespace Save_Editor.Models {
    public class Dialog : NotifyPropertyChangedImpl {
        public string                     id          { get; set; }
        public bool                       isPlaying   { get; set; }
        public bool                       completed   { get; set; }
        public ObservableCollection<bool> linesPlayed { get; } = new ObservableCollection<bool>();

        public Dialog(BinaryReader reader) {
            id          = reader.ReadString();
            isPlaying   = reader.ReadBoolean();
            completed   = reader.ReadBoolean();
            linesPlayed = reader.ReadList<bool>();
        }
    }

    public static partial class Extensions {
        public static Dialog ReadDialog(this BinaryReader reader) {
            return new Dialog(reader);
        }

        public static void Write(this BinaryWriter writer, Dialog dialog) {
            writer.Write(dialog.id);
            writer.Write(dialog.isPlaying);
            writer.Write(dialog.completed);
            writer.Write(dialog.linesPlayed);
        }
    }
}