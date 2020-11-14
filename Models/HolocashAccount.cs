using System.IO;

namespace Save_Editor.Models {
    public class HolocashAccount : NotifyPropertyChangedImpl {
        public string key   { get; set; }
        public int    funds { get; set; }

        public HolocashAccount() {
        }

        public HolocashAccount(BinaryReader reader) {
            key   = reader.ReadString();
            funds = reader.ReadInt32();
        }
    }

    public static partial class Extensions {
        public static HolocashAccount ReadHolocashAccount(this BinaryReader reader) {
            return new HolocashAccount(reader);
        }

        public static void Write(this BinaryWriter writer, HolocashAccount holocashAccount) {
            writer.Write(holocashAccount.key);
            writer.Write(holocashAccount.funds);
        }
    }
}