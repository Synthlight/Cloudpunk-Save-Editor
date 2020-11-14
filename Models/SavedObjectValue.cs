using System;
using System.IO;
using System.Numerics;

namespace Save_Editor.Models {
    public class SavedObjectValue : NotifyPropertyChangedImpl {
        public int                  id   { get; set; }
        public SavedObjectValueType type { get; set; }
        public object               data { get; set; }

        public SavedObjectValue(BinaryReader reader) {
            id   = reader.ReadInt32();
            type = (SavedObjectValueType) reader.ReadInt32();

            data = type switch {
                SavedObjectValueType.Bool => reader.ReadBoolean(),
                SavedObjectValueType.Float => reader.ReadSingle(),
                SavedObjectValueType.Int => reader.ReadInt32(),
                SavedObjectValueType.Vector3 => reader.ReadVector3(),
                SavedObjectValueType.Transform => reader.ReadTransform(),
                SavedObjectValueType.String => reader.ReadString(),
                SavedObjectValueType.GameObject => reader.ReadGameObject(),
                _ => throw new NotImplementedException($"No read method set for \"{type}\"")
            };
        }
    }

    public static partial class Extensions {
        public static SavedObjectValue ReadSavedObjectValue(this BinaryReader reader) {
            return new SavedObjectValue(reader);
        }

        public static void Write(this BinaryWriter writer, SavedObjectValue savedObjectValue) {
            writer.Write(savedObjectValue.id);
            writer.Write((int) savedObjectValue.type);

            switch (savedObjectValue.type) {
                case SavedObjectValueType.Bool:
                    writer.Write((bool) savedObjectValue.data);
                    break;
                case SavedObjectValueType.Float:
                    writer.Write((float) savedObjectValue.data);
                    break;
                case SavedObjectValueType.Int:
                    writer.Write((int) savedObjectValue.data);
                    break;
                case SavedObjectValueType.Vector3:
                    writer.Write((Vector3) savedObjectValue.data);
                    break;
                case SavedObjectValueType.Transform:
                    writer.Write((Transform) savedObjectValue.data);
                    break;
                case SavedObjectValueType.String:
                    writer.Write(((string) savedObjectValue.data));
                    break;
                case SavedObjectValueType.GameObject:
                    writer.Write((GameObject) savedObjectValue.data);
                    break;
                default: throw new NotImplementedException($"No write method set for \"{savedObjectValue.type}\"");
            }
        }
    }
}