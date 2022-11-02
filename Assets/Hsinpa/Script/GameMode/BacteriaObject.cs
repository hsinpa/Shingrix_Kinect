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

        [SerializeField]
        private ParticleSystem m_break_particle;

        [SerializeField]
        private Renderer m_renderer;

        public Material cutMaterial;

        [SerializeField]
        private Material[] material_sets;

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

        private float _moveSpeed, _rotateSpeed;
        public float MoveSpeed => _moveSpeed;
        public float RotationSpeed => _rotateSpeed;

        private Vector3 _angularVelocity;
        public Vector3 AngularVelocity => _angularVelocity;

        public void SetUp(float maxSpeed, float minSpeed, int maxRotation, int minRotation) {
            _moveSpeed = Random.Range(minSpeed, maxSpeed);
            _rotateSpeed = Random.Range(minRotation, maxRotation);
            _angularVelocity = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));

            m_renderer.material = material_sets[Random.Range(0, material_sets.Length)];
        }

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