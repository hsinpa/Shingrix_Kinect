using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Shingrix.Mode.Game
{
    public class MouseTracker : ITracker
    {
        List<TrackerStruct> m_trackerStructs = new List<TrackerStruct>();
        Vector3 m_cache_vector3 = new Vector3();
        Camera m_camera;

        public MouseTracker(Camera camera) {
            m_camera = camera;
            m_trackerStructs.Add(new TrackerStruct() { 
                index = 0, 
                position = Vector3.zero, 
                bounds = new Bounds(new Vector3(0, -10, 0), new Vector3(0.1f, 0.1f, 0.1f))
            });
            m_cache_vector3.z = m_camera.nearClipPlane;
        }

        public void OnUpdate()
        {
            Vector2 screenMousePos = Mouse.current.position.ReadValue();
            m_cache_vector3.x = screenMousePos.x;
            m_cache_vector3.y = screenMousePos.y;

            Vector3 worldPos = m_camera.ScreenToWorldPoint(m_cache_vector3);
            worldPos.z = 0;

            var tracker = m_trackerStructs[0];
            tracker.UpdatePosition(worldPos);
            m_trackerStructs[0] = tracker;
        }

        public TrackerStruct GetTracker(int index)
        {
            return m_trackerStructs[0];
        }

        public List<TrackerStruct> GetTrackers()
        {
            return m_trackerStructs;
        }
    }
}