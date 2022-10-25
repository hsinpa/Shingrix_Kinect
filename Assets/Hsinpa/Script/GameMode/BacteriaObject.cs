using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shingrix.Mode.Game
{
    public class BacteriaObject : MonoBehaviour
    {
        [SerializeField]
        private Collider m_collider;
        public Collider Collider => this.m_collider;

        public Material cutMaterial;

        private Vector3 contact_transform_position;
        private Vector3 contact_point;
        public Vector3 Contact_Point {
            get {
                Vector3 diff = transform.position - contact_transform_position;
                return (diff + contact_point);
            }
        }

        private int contact_index;
        public int Contact_Index => contact_index;

        public void SetCutterContactPoint(int index, Vector3 p_contact_point) {
            contact_index = index;
            contact_point = p_contact_point;
            contact_transform_position = transform.position;
        }

        public Vector3 GetVector(Vector3 p_exit_point) {
            return (p_exit_point - (Contact_Point)).normalized;
        }
    }
}