using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Save_Editor.Models;

namespace Save_Editor {
    public static class Extensions {
        /// <summary>
        /// Converts a DateTime to the int representation which is the number of seconds since the unix epoch.
        /// </summary>
        /// <param name="dateTime">A DateTime to convert to epoch time.</param>
        /// <returns>The int number of seconds since the unix epoch.</returns>
        public static int ToEpoch32(this DateTime dateTime) {
            return (int) (dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// Converts an int representation of time since the unix epoch to a DateTime.
        /// </summary>
        /// <param name="epoch">The number of seconds since Jan 1, 1970.</param>
        /// <returns>A DateTime representing the time since the epoch.</returns>
        public static DateTime FromEpoch32(this int epoch) {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddSeconds(epoch);
        }

        public static DateTime ReadEpochDateTime32(this BinaryReader reader) {
            return reader.ReadInt32().FromEpoch32();
        }

        public static void WriteEpochDateTime32(this BinaryWriter writer, DateTime dateTime) {
            writer.Write(dateTime.ToEpoch32());
        }

        public static Vector3 ReadVector3(this BinaryReader reader) {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static void Write(this BinaryWriter writer, Vector3 vector3) {
            writer.Write(vector3.X);
            writer.Write(vector3.Y);
            writer.Write(vector3.Z);
        }

        public static Quaternion ReadQuaternion(this BinaryReader reader) {
            return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static void Write(this BinaryWriter writer, Quaternion quaternion) {
            writer.Write(quaternion.X);
            writer.Write(quaternion.Y);
            writer.Write(quaternion.Z);
            writer.Write(quaternion.W);
        }

        public static Transform ReadTransform(this BinaryReader reader) {
            return new Transform(reader.ReadVector3(), reader.ReadVector3());
        }

        public static void Write(this BinaryWriter writer, Transform transform) {
            writer.Write(transform.position);
            writer.Write(transform.rotation);
        }

        public static GameObject ReadGameObject(this BinaryReader reader) {
            return new GameObject(reader.ReadBoolean());
        }

        public static void Write(this BinaryWriter writer, GameObject gameObject) {
            writer.Write(gameObject.value);
        }

        public static ObservableCollection<T> ReadList<T>(this BinaryReader reader) {
            var list   = new ObservableCollection<T>();
            var length = reader.ReadInt32();
            for (var i = 0; i < length; i++) {
                list.Add(reader.ReadT<T>());
            }
            return list;
        }

        public static T ReadT<T>(this BinaryReader reader) {
            var type = typeof(T);

            if (type == typeof(string)) return (T) (object) reader.ReadString();
            if (type == typeof(bool)) return (T) (object) reader.ReadBoolean();
            if (type == typeof(Dialog)) return (T) (object) reader.ReadDialog();
            if (type == typeof(QuestText)) return (T) (object) reader.ReadQuestText();
            if (type == typeof(Item)) return (T) (object) reader.ReadItem();
            if (type == typeof(SavedObject)) return (T) (object) reader.ReadSavedObject();
            if (type == typeof(SavedObjectValue)) return (T) (object) reader.ReadSavedObjectValue();
            if (type == typeof(HolocashAccount)) return (T) (object) reader.ReadHolocashAccount();

            throw new NotImplementedException($"No read method set for \"{type}\"");
        }

        public static void Write<T>(this BinaryWriter writer, ObservableCollection<T> list) {
            writer.Write(list.Count);
            foreach (var item in list) {
                writer.WriteT(item);
            }
        }

        public static void WriteT<T>(this BinaryWriter writer, T item) {
            var type = typeof(T);

            var writeMethod = writer.GetType().GetMethod(nameof(BinaryWriter.Write), BindingFlags.Public | BindingFlags.Instance, null, new[] {type}, null);
            if (writeMethod != null) {
                writeMethod.Invoke(writer, new object[] {item});
                return;
            }

            var methods                        = GetExtensionMethods(typeof(BinaryWriter), type).ToList();
            if (methods.Count > 0) writeMethod = methods.First();
            if (writeMethod != null) {
                writeMethod.Invoke(null, new object[] {writer, item});
                return;
            }

            throw new NotImplementedException($"No write method set for \"{type}\"");
        }

        private static IEnumerable<MethodInfo> GetExtensionMethods(params Type[] types) {
            return from type in typeof(Extensions).Assembly.GetTypes()
                   where type.IsSealed && !type.IsGenericType && !type.IsNested
                   from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                   where method.IsDefined(typeof(ExtensionAttribute), false)
                   where Equals(method.GetParameters(), types)
                   select method;
        }

        public static bool Equals(ParameterInfo[] parameterInfos, Type[] types) {
            if (parameterInfos.Length != types.Length) return false;
            var match = true;
            for (var i = 0; i < parameterInfos.Length; i++) {
                if (parameterInfos[i].ParameterType != types[i]) match = false;
            }
            return match;
        }
    }
}