using System.Collections.ObjectModel;
using System.IO;
using System.Numerics;

namespace Save_Editor.Models {
    public class Player : NotifyPropertyChangedImpl {
        public int                          version                   { get; set; }
        public bool                         enabledLoadState          { get; set; }
        public Vector3                      transform_position        { get; set; }
        public Vector3                      transform_eulerAngles     { get; set; }
        public int                          health                    { get; set; }
        public Quaternion                   defaultAlignment          { get; set; }
        public Quaternion                   alignment                 { get; set; }
        public Vector3                      moveToPoint_localPosition { get; set; }
        public bool                         unknown                   { get; set; } // writer.Write<bool>(false);
        public string                       nextSpawnPoint            { get; set; }
        public ObservableCollection<string> charactersToEnable        { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> charactersToSpawn         { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> charactersToDisable       { get; } = new ObservableCollection<string>();
        public bool                         freeLook                  { get; set; }
        public float                        zoomFactor                { get; set; }
        public bool                         lastFreeLook              { get; set; }
        public bool                         cameraLocked              { get; set; }

        public Player(BinaryReader reader) {
            version                   = reader.ReadInt32();
            enabledLoadState          = reader.ReadBoolean();
            transform_position        = reader.ReadVector3();
            transform_eulerAngles     = reader.ReadVector3();
            health                    = reader.ReadInt32();
            defaultAlignment          = reader.ReadQuaternion();
            alignment                 = reader.ReadQuaternion();
            moveToPoint_localPosition = reader.ReadVector3();
            unknown                   = reader.ReadBoolean();
            if (version >= 2) {
                nextSpawnPoint = reader.ReadString();
            }
            if (version >= 3) {
                charactersToEnable = reader.ReadList<string>();
                charactersToSpawn  = reader.ReadList<string>();
                if (version >= 4) {
                    charactersToDisable = reader.ReadList<string>();
                    if (version >= 5) {
                        freeLook = reader.ReadBoolean();
                        if (version >= 6) {
                            zoomFactor = reader.ReadSingle();
                            if (version >= 6) {
                                lastFreeLook = reader.ReadBoolean();
                                cameraLocked = reader.ReadBoolean();
                            }
                        }
                    }
                }
            }
        }
    }

    public static partial class Extensions {
        public static Player ReadPlayer(this BinaryReader reader) {
            return new Player(reader);
        }

        public static void Write(this BinaryWriter writer, Player player) {
            writer.Write(player.version);
            writer.Write(player.enabledLoadState);
            writer.Write(player.transform_position);
            writer.Write(player.transform_eulerAngles);
            writer.Write(player.health);
            writer.Write(player.defaultAlignment);
            writer.Write(player.alignment);
            writer.Write(player.moveToPoint_localPosition);
            writer.Write(player.unknown);
            if (player.version >= 2) {
                writer.Write(player.nextSpawnPoint);
            }
            if (player.version >= 3) {
                writer.Write(player.charactersToEnable);
                writer.Write(player.charactersToSpawn);
                if (player.version >= 4) {
                    writer.Write(player.charactersToDisable);
                    if (player.version >= 5) {
                        writer.Write(player.freeLook);
                        if (player.version >= 6) {
                            writer.Write(player.zoomFactor);
                            if (player.version >= 6) {
                                writer.Write(player.lastFreeLook);
                                writer.Write(player.cameraLocked);
                            }
                        }
                    }
                }
            }
        }
    }
}