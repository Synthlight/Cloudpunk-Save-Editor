using System.IO;
using System.Numerics;

namespace Save_Editor.Models {
    public class Car : NotifyPropertyChangedImpl {
        public CarType carId                            { get; set; }
        public int     version                          { get; set; }
        public bool    gameObject_activeInHierarchy     { get; set; }
        public Vector3 transform_position               { get; set; }
        public Vector3 transform_eulerAngles            { get; set; }
        public float   currentFloorLevel                { get; set; }
        public int     hitBelowCounter                  { get; set; }
        public float   currentStatus                    { get; set; }
        public float   currentFuel                      { get; set; }
        public bool    parkObject_activeInHierarchy     { get; set; }
        public Vector3 parkObject_transform_position    { get; set; }
        public Vector3 parkObject_transform_eulerAngles { get; set; }
        public bool    controlsFrozen                   { get; set; }
        public bool    isInVehicleCockpit               { get; set; }
        public uint    currentCracksTexIndex            { get; set; }

        // These are placed later in the save file.
        public int restrictLandingToParkVolumeID { get; set; }
        public int activateHuxleyAtParkVolumeID  { get; set; }
        public int activatePashtaAtParkVolumeID  { get; set; }

        public Car(BinaryReader reader, int saveVersion) {
            if (saveVersion >= 3) {
                carId = (CarType) reader.ReadInt32();
            }
            version                          = reader.ReadInt32();
            gameObject_activeInHierarchy     = reader.ReadBoolean();
            transform_position               = reader.ReadVector3();
            transform_eulerAngles            = reader.ReadVector3();
            currentFloorLevel                = reader.ReadSingle();
            hitBelowCounter                  = reader.ReadInt32();
            currentStatus                    = reader.ReadSingle();
            currentFuel                      = reader.ReadSingle();
            parkObject_activeInHierarchy     = reader.ReadBoolean();
            parkObject_transform_position    = reader.ReadVector3();
            parkObject_transform_eulerAngles = reader.ReadVector3();
            if (version >= 2) {
                controlsFrozen = reader.ReadBoolean();
            }
            if (version >= 3) {
                isInVehicleCockpit    = reader.ReadBoolean();
                currentCracksTexIndex = reader.ReadUInt32();
            }
        }
    }

    public static partial class Extensions {
        public static Car ReadCar(this BinaryReader reader, int saveVersion) {
            return new Car(reader, saveVersion);
        }

        public static void Write(this BinaryWriter writer, Car car, int saveVersion) {
            if (saveVersion >= 3) {
                writer.Write((int) car.carId);
            }
            writer.Write(car.version);
            writer.Write(car.gameObject_activeInHierarchy);
            writer.Write(car.transform_position);
            writer.Write(car.transform_eulerAngles);
            writer.Write(car.currentFloorLevel);
            writer.Write(car.hitBelowCounter);
            writer.Write(car.currentStatus);
            writer.Write(car.currentFuel);
            writer.Write(car.parkObject_activeInHierarchy);
            writer.Write(car.parkObject_transform_position);
            writer.Write(car.parkObject_transform_eulerAngles);
            if (car.version >= 2) {
                writer.Write(car.controlsFrozen);
            }
            if (car.version >= 3) {
                writer.Write(car.isInVehicleCockpit);
                writer.Write(car.currentCracksTexIndex);
            }
        }
    }
}