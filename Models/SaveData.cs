using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Save_Editor.Models {
    public class SaveData : NotifyPropertyChangedImpl {
        public int                                   saveVersion          { get; set; }
        public int                                   sceneId              { get; set; }
        public float                                 timeSinceStart       { get; set; }
        public string                                time                 { get; set; }
        public DateTime                              epoch                { get; set; }
        public ObservableCollection<Dialog>          dialog               { get; } = new ObservableCollection<Dialog>();
        public ObservableCollection<QuestText>       questText            { get; } = new ObservableCollection<QuestText>();
        public ObservableCollection<Item>            items                { get; } = new ObservableCollection<Item>();
        public Player                                player               { get; set; }
        public Car                                   car                  { get; set; }
        public int                                   money                { get; set; }
        public int                                   numLocationsUnlocked { get; set; }
        public int                                   numRepairs           { get; set; }
        public bool                                  blockingOverride     { get; set; }
        public ObservableCollection<SavedObject>     savedObjects         { get; } = new ObservableCollection<SavedObject>();
        public ObservableCollection<HolocashAccount> holocashAccounts     { get; } = new ObservableCollection<HolocashAccount>();
        public float                                 joosTimeLeft         { get; set; }
        public float                                 joosTimePassed       { get; set; }
        public float                                 alcoholTimeLeft      { get; set; }
        public float                                 alcoholTimePassed    { get; set; }
        public float                                 speedGainTimeLeft    { get; set; }
        public float                                 stimsTimeLeft        { get; set; }
        public float                                 stimsTimePassed      { get; set; }
        public float                                 pheromonesTimeLeft   { get; set; }
        public float                                 pheromonesTimePassed { get; set; }
        public float                                 foodCooldown         { get; set; }
        public float                                 drinkCooldown        { get; set; }
        public float                                 drugCooldown         { get; set; }
        public bool                                  globalVehicleDamage  { get; set; }

        public SaveData(BinaryReader reader) {
            saveVersion          = reader.ReadInt32();
            sceneId              = reader.ReadInt32();
            timeSinceStart       = reader.ReadSingle();
            time                 = reader.ReadString();
            epoch                = reader.ReadEpochDateTime32();
            dialog               = reader.ReadList<Dialog>();
            questText            = reader.ReadList<QuestText>();
            items                = reader.ReadList<Item>();
            player               = reader.ReadPlayer();
            car                  = reader.ReadCar(saveVersion);
            money                = reader.ReadInt32();
            numLocationsUnlocked = reader.ReadInt32();
            if (saveVersion >= 8) {
                numRepairs = reader.ReadInt32();
                if (saveVersion >= 8) {
                    blockingOverride = reader.ReadBoolean();
                }
            }
            savedObjects = reader.ReadList<SavedObject>();
            if (saveVersion >= 2) {
                car.restrictLandingToParkVolumeID = reader.ReadInt32();
                if (saveVersion >= 4) {
                    holocashAccounts = reader.ReadList<HolocashAccount>();
                    if (saveVersion >= 5) {
                        car.activateHuxleyAtParkVolumeID = reader.ReadInt32();
                        if (saveVersion >= 7) {
                            car.activatePashtaAtParkVolumeID = reader.ReadInt32();
                            if (saveVersion >= 10) {
                                joosTimeLeft         = reader.ReadSingle();
                                joosTimePassed       = reader.ReadSingle();
                                alcoholTimeLeft      = reader.ReadSingle();
                                alcoholTimePassed    = reader.ReadSingle();
                                speedGainTimeLeft    = reader.ReadSingle();
                                stimsTimeLeft        = reader.ReadSingle();
                                stimsTimePassed      = reader.ReadSingle();
                                pheromonesTimeLeft   = reader.ReadSingle();
                                pheromonesTimePassed = reader.ReadSingle();
                                if (saveVersion >= 11) {
                                    foodCooldown  = reader.ReadSingle();
                                    drinkCooldown = reader.ReadSingle();
                                    drugCooldown  = reader.ReadSingle();
                                    if (saveVersion >= 12) {
                                        globalVehicleDamage = reader.ReadBoolean();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public static partial class Extensions {
        public static SaveData ReadSaveData(this BinaryReader reader) {
            return new SaveData(reader);
        }

        public static void Write(this BinaryWriter writer, SaveData saveData) {
            writer.Write(saveData.saveVersion);
            writer.Write(saveData.sceneId);
            writer.Write(saveData.timeSinceStart);
            writer.Write(saveData.time);
            writer.WriteEpochDateTime32(saveData.epoch);
            writer.Write(saveData.dialog);
            writer.Write(saveData.questText);
            writer.Write(saveData.items);
            writer.Write(saveData.player);
            writer.Write(saveData.car, saveData.saveVersion);
            writer.Write(saveData.money);
            writer.Write(saveData.numLocationsUnlocked);
            if (saveData.saveVersion >= 8) {
                writer.Write(saveData.numRepairs);
                if (saveData.saveVersion >= 8) {
                    writer.Write(saveData.blockingOverride);
                }
            }
            writer.Write(saveData.savedObjects);
            if (saveData.saveVersion >= 2) {
                writer.Write(saveData.car.restrictLandingToParkVolumeID);
                if (saveData.saveVersion >= 4) {
                    writer.Write(saveData.holocashAccounts);
                    if (saveData.saveVersion >= 5) {
                        writer.Write(saveData.car.activateHuxleyAtParkVolumeID);
                        if (saveData.saveVersion >= 7) {
                            writer.Write(saveData.car.activatePashtaAtParkVolumeID);
                            if (saveData.saveVersion >= 10) {
                                writer.Write(saveData.joosTimeLeft);
                                writer.Write(saveData.joosTimePassed);
                                writer.Write(saveData.alcoholTimeLeft);
                                writer.Write(saveData.alcoholTimePassed);
                                writer.Write(saveData.speedGainTimeLeft);
                                writer.Write(saveData.stimsTimeLeft);
                                writer.Write(saveData.stimsTimePassed);
                                writer.Write(saveData.pheromonesTimeLeft);
                                writer.Write(saveData.pheromonesTimePassed);
                                if (saveData.saveVersion >= 11) {
                                    writer.Write(saveData.foodCooldown);
                                    writer.Write(saveData.drinkCooldown);
                                    writer.Write(saveData.drugCooldown);
                                    if (saveData.saveVersion >= 12) {
                                        writer.Write(saveData.globalVehicleDamage);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}