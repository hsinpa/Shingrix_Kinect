using Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Utility;
using UnityEngine.VFX;

namespace Shingrix.Mode.Game {
    public class BacteriaSpawner
    {
        private Transform m_container;

        private List<BacteriaObject> m_bateriaList = new List<BacteriaObject>();
        public IReadOnlyCollection<BacteriaObject> BateriaList => this.m_bateriaList;

        private List<BacteriaObject> m_waitForDeletionPipe = new List<BacteriaObject>();

        private float m_bacMoveSpeed = 1;
        private int m_bacLength = 0;
        private float nextSpawnTime;

        private Vector3 _cacheVector3 = new Vector3();
        private PoolManager m_poolManager;
        private float _lastSpawnPosition;

        public BacteriaSpawner(BacteriaObject bacteriaPrefab, BacteriaObject superPrefab, ParticleSystem breakParticle, VisualEffect slashParticle, Transform container) {
            this.m_container = container;

            this.m_poolManager = Pooling.PoolManager.instance;
            this.m_poolManager.CreatePool(bacteriaPrefab.gameObject, ShingrixStatic.Event.ObjPoolKeyBateria, ShingrixStatic.Bacteria.maxBacteriaSize);
            this.m_poolManager.CreatePool(superPrefab.gameObject, ShingrixStatic.Event.ObjPoolKeySuper, ShingrixStatic.Bacteria.maxBacteriaSize);
            this.m_poolManager.CreatePool(breakParticle.gameObject, ShingrixStatic.Event.ObjPoolKeybreakParticle, ShingrixStatic.Bacteria.maxParticleSize);
            this.m_poolManager.CreatePool(slashParticle.gameObject, ShingrixStatic.Event.ObjPoolKeySlashParticle, ShingrixStatic.Bacteria.maxParticleSize);

            _lastSpawnPosition = float.PositiveInfinity;
        }

        public void OnUpdate() {
            ProcessDeleteObj();
            ProcessBacteriaSpawn();
            ProcessBacteriaMovement();
        }

        public void EnqueueDeleteObject(BacteriaObject deleteObject) {
            m_waitForDeletionPipe.Add(deleteObject);
        }

        public void Dispose() {
            int childCount = m_container.childCount;
            for (int i = childCount - 1; i>=0; i--)
            {
                this.m_poolManager.Destroy(m_container.GetChild(i).gameObject);
            }

            m_waitForDeletionPipe.Clear();
            m_bateriaList.Clear();
            m_bacLength = 0;
            nextSpawnTime = Time.time + ShingrixStatic.Bacteria.spawnTimeStepMax;
            _lastSpawnPosition = float.PositiveInfinity;
        }

        private void RemoveBateria(BacteriaObject deleteObject) {
            m_bateriaList.Remove(deleteObject);
            m_bacLength--;

            this.m_poolManager.Destroy(deleteObject.gameObject);
        }

        private void ProcessBacteriaSpawn() {
            if (nextSpawnTime < Time.time && m_bacLength < ShingrixStatic.Bacteria.maxBacteriaSize) {

                string object_key = UtilityFunc.PercentageGame(ShingrixStatic.GameMode.SuperRate) ? ShingrixStatic.Event.ObjPoolKeySuper : ShingrixStatic.Event.ObjPoolKeyBateria;

                GameObject spawnRawObject =  this.m_poolManager.ReuseObject(object_key);
                spawnRawObject.transform.SetParent(this.m_container);
                spawnRawObject.transform.localScale = Vector3.one;

                BacteriaObject spawnBateria = spawnRawObject.GetComponent<BacteriaObject>();

                spawnBateria.SetUp(ShingrixStatic.Bacteria.maxBateriaMoveSpeed, ShingrixStatic.Bacteria.minBateriaMoveSpeed, 
                                    ShingrixStatic.Bacteria.maxBateriaRotateSpeed, ShingrixStatic.Bacteria.minBateriaRotateSpeed);

                float spawnPositionX =  Random.Range(ShingrixStatic.Bacteria.screenWidth.x, ShingrixStatic.Bacteria.screenWidth.y);

                float stepSize = 0.8f;
                while (Mathf.Abs(_lastSpawnPosition - spawnPositionX) < stepSize)
                {
                    spawnPositionX = Random.Range(ShingrixStatic.Bacteria.screenWidth.x, ShingrixStatic.Bacteria.screenWidth.y);
                }

                _lastSpawnPosition = spawnPositionX;

                spawnBateria.transform.position = new Vector3(
                    spawnPositionX, 
                    ShingrixStatic.Bacteria.spawnPosition,
                    Random.Range(-0.5f, 0.5f) //To show depth
                );

                m_bateriaList.Add(spawnBateria);
                m_bacLength++;

                float spawnTime = Random.Range(ShingrixStatic.Bacteria.spawnTimeStepMin, ShingrixStatic.Bacteria.spawnTimeStepMax);
                nextSpawnTime = Time.time + spawnTime;
            }
        }

        private void ProcessBacteriaMovement() {
            foreach (var bacteriaObj in m_bateriaList)
            {
                Vector3 worldPosition = bacteriaObj.transform.position;
                bacteriaObj.transform.RotateAround(worldPosition, bacteriaObj.AngularVelocity, bacteriaObj.RotationSpeed * Time.deltaTime);

                // Upward
                if (worldPosition.y < ShingrixStatic.Bacteria.midPosition) 
                {
                    bacteriaObj.transform.Translate(ShingrixStatic.Bacteria.upwardGeneralVelocity * Time.deltaTime * bacteriaObj.MoveSpeed, relativeTo: Space.World);
                }

                // Passing mid point, move backward
                if (worldPosition.y > ShingrixStatic.Bacteria.midPosition)
                {
                    bacteriaObj.transform.localScale = bacteriaObj.transform.localScale * 0.99f;

                    Vector3 next_position = bacteriaObj.transform.position;
                    next_position = next_position + (ShingrixStatic.Bacteria.midGeneralVelocity * Time.deltaTime * bacteriaObj.MoveSpeed);
                    next_position.x = Mathf.Lerp(next_position.x, 0, 0.0055f);

                    bacteriaObj.transform.position = next_position;
                }


                //Reach vanish point, delete it
                if (worldPosition.y > ShingrixStatic.Bacteria.vanishPositionYZ.x) {
                    m_waitForDeletionPipe.Add(bacteriaObj);
                    return;
                }
            }
        }

        private void ProcessDeleteObj() {
            int d_length = m_waitForDeletionPipe.Count;

            for (int i = d_length - 1; i >= 0; i--) {
                RemoveBateria(m_waitForDeletionPipe[i]);
                m_waitForDeletionPipe.RemoveAt(i);
            }
        }
    }
}
