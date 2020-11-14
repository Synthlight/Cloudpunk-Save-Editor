using System.Numerics;

namespace Save_Editor.Models {
    public class Transform {
        public readonly Vector3 position;
        public readonly Vector3 rotation;

        public Transform(Vector3 position, Vector3 rotation) {
            this.position = position;
            this.rotation = rotation;
        }
    }
}