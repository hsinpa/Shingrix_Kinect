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

        private float m_worldHeight_top;
        private float m_worldHeight_bottom;
        private float m_worldWidth_left;
        private float m_worldWidth_right;

        public MouseTracker(Camera camera) {
            m_camera = camera;
            m_trackerStructs.Add(new TrackerStruct() { 
                index = 0, 
                position = Vector3.zero, 
                bounds = new Bounds(new Vector3(0, -10, 0), new Vector3(0.1f, 0.1f, 0.1f))
            });
            m_cache_vector3.z = m_camera.nearClipPlane;

            float aspectRatio = (float)Screen.width / Screen.height;

            float worldHeight_radius = m_camera.orthographicSize;
            float worldWidth_radius = (m_camera.orthographicSize * 2 * aspectRatio) * 0.5f;

            m_worldHeight_top = worldHeight_radius;
            m_worldHeight_bottom = -worldHeight_radius;

            m_worldWidth_right = worldWidth_radius;
            m_worldWidth_left = -worldWidth_radius;

            //Debug.Log($"m_worldHeight_top {m_worldHeight_top}");
            //Debug.Log($"m_worldHeight_bottom {m_worldHeight_bottom}");
            //Debug.Log($"m_worldWidth_right {m_worldWidth_right}");
            //Debug.Log($"m_worldWidth_left {m_worldWidth_left}");
        }

        public void OnUpdate()
        {
            Vector2 screenMousePos = Mouse.current.position.ReadValue();
            float uv_x = screenMousePos.x / Screen.width;
            float uv_y = screenMousePos.y / Screen.height;

            m_cache_vector3.Set(
                Mathf.Lerp(m_worldWidth_left, m_worldWidth_right, uv_x),
                Mathf.Lerp(m_worldHeight_bottom, m_worldHeight_top, uv_y), 0);
                        //Debug.Log(worldPos);

            var tracker = m_trackerStructs[0];
            tracker.UpdatePosition(m_cache_vector3);
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