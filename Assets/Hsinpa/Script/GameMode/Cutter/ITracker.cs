using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shingrix.Mode.Game
{
    public interface ITracker
    {
        public void OnUpdate();

        public List<TrackerStruct> GetTrackers();
        public TrackerStruct GetTracker(int index);
    }

    public struct TrackerStruct {
        public int index;
        public Vector3 position;
        public Bounds bounds;
        public bool isAvailable;

        public void UpdatePosition(Vector3 p_newPos) {
            this.position = p_newPos;
            this.bounds.center = p_newPos;
        }
    }
}