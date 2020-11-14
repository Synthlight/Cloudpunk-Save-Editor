using System.IO;

namespace Save_Editor.Models {
    public class Item : NotifyPropertyChangedImpl {
        public string id                   { get; set; }
        public int    version              { get; set; }
        public bool   unknown              { get; set; } // writer.Write<bool>(true);
        public int    amount               { get; set; }
        public int    totalAmountAvailable { get; set; }
        public bool   isEquipped           { get; set; }

        public Item() {
            unknown = true;
        }

        public Item(BinaryReader reader) {
            id      = reader.ReadString();
            version = reader.ReadInt32();
            unknown = reader.ReadBoolean();
            amount  = reader.ReadInt32();
            if (version >= 2) {
                totalAmountAvailable = reader.ReadInt32();
                if (version >= 3) {
                    isEquipped = reader.ReadBoolean();
                }
            }
        }
    }

    public static partial class Extensions {
        public static Item ReadItem(this BinaryReader reader) {
            return new Item(reader);
        }

        public static void Write(this BinaryWriter writer, Item item) {
            writer.Write(item.id);
            writer.Write(item.version);
            writer.Write(item.unknown);
            writer.Write(item.amount);
            if (item.version >= 2) {
                writer.Write(item.totalAmountAvailable);
                if (item.version >= 3) {
                    writer.Write(item.isEquipped);
                }
            }
        }
    }
}