using System.Collections.ObjectModel;
using System.IO;

namespace Save_Editor.Models {
    public class SavedObject : NotifyPropertyChangedImpl {
        public string                                 id     { get; set; }
        public ObservableCollection<SavedObjectValue> values { get; } = new ObservableCollection<SavedObjectValue>();

        public SavedObject(BinaryReader reader) {
            id     = reader.ReadString();
            values = reader.ReadList<SavedObjectValue>();
        }
    }

    public static partial class Extensions {
        public static SavedObject ReadSavedObject(this BinaryReader reader) {
            return new SavedObject(reader);
        }

        public static void Write(this BinaryWriter writer, SavedObject savedObject) {
            writer.Write(savedObject.id);
            writer.Write(savedObject.values);
        }
    }
}