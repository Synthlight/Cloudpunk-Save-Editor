using System.IO;

namespace Save_Editor.Models {
    public class QuestText : NotifyPropertyChangedImpl {
        public string id              { get; set; }
        public bool   enabled         { get; set; }
        public int    progressCurrent { get; set; }
        public int    progressMax     { get; set; }

        public QuestText(BinaryReader reader) {
            id              = reader.ReadString();
            enabled         = reader.ReadBoolean();
            progressCurrent = reader.ReadInt32();
            progressMax     = reader.ReadInt32();
        }
    }

    public static partial class Extensions {
        public static QuestText ReadQuestText(this BinaryReader reader) {
            return new QuestText(reader);
        }

        public static void Write(this BinaryWriter writer, QuestText questText) {
            writer.Write(questText.id);
            writer.Write(questText.enabled);
            writer.Write(questText.progressCurrent);
            writer.Write(questText.progressMax);
        }
    }
}