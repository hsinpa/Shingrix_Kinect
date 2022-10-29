using Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shingrix.Mode.Game {
    public class BacteriaSpawner
    {
        private BacteriaObject m_bacteriaPrefab;
        private Transform m_container;

        private List<BacteriaObject> m_bateriaList = new List<BacteriaObject>();
        public IReadOnlyCollection<BacteriaObject> BateriaList => this.m_bateriaList;

        private List<BacteriaObject> m_waitForDeletionPipe = new List<BacteriaObject>();

        private float m_bacMoveSpeed = 1;
        private int m_bacLength = 0;
        private float nextSpawnTime;

        private Vector3 _cacheVector3 = new Vector3();
        private PoolManager m_poolManager;

        public BacteriaSpawner(BacteriaObject bacteriaPrefab, Transform container) {
            this.m_bacteriaPrefab = bacteriaPrefab;
            this.m_container = container;

            this.m_poolManager = Pooling.PoolManager.instance;
            this.m_poolManager.CreatePool(bacteriaPrefab.gameObject, ShingrixStatic.Event.ObjPoolKeyBateria, ShingrixStatic.Bacteria.maxBacteriaSize);
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
            Hsinpa.Utility.UtilityFunc.ClearChildObject(m_container);
            m_waitForDeletionPipe.Clear();
            m_bateriaList.Clear();
            m_bacLength = 0;
            nextSpawnTime = Time.time + ShingrixStatic.Bacteria.spawnTimeStep;
        }

        private void RemoveBateria(BacteriaObject deleteObject) {
            m_bateriaList.Remove(deleteObject);
            m_bacLength--;

            this.m_poolManager.Destroy(deleteObject.gameObject);
        }

        private void ProcessBacteriaSpawn() {
            if (nextSpawnTime < Time.time && m_bacLength < ShingrixStatic.Bacteria.maxBacteriaSize) {
                GameObject spawnRawObject =  this.m_poolManager.ReuseObject(ShingrixStatic.Event.ObjPoolKeyBateria);
                spawnRawObject.transform.SetParent(this.m_container);

                BacteriaObject spawnBateria = spawnRawObject.GetComponent<BacteriaObject>();

                spawnBateria.SetUp(ShingrixStatic.Bacteria.maxBateriaMoveSpeed, ShingrixStatic.Bacteria.minBateriaMoveSpeed, 
                                    ShingrixStatic.Bacteria.maxBateriaRotateSpeed, ShingrixStatic.Bacteria.minBateriaRotateSpeed);

                spawnBateria.transform.position = new Vector3( 
                    Random.Range(ShingrixStatic.Bacteria.screenWidth.x, ShingrixStatic.Bacteria.screenWidth.y), 
                    ShingrixStatic.Bacteria.spawnPosition, 
                    0);

                m_bateriaList.Add(spawnBateria);
                m_bacLength++;
                nextSpawnTime = Time.time + ShingrixStatic.Bacteria.spawnTimeStep;
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
                    bacteriaObj.transform.localScale = bacteriaObj.transform.localScale * 0.999f;

                    Vector3 next_position = bacteriaObj.transform.position;
                    next_position = next_position + (ShingrixStatic.Bacteria.midGeneralVelocity * Time.deltaTime * bacteriaObj.MoveSpeed);
                    next_position.x = Mathf.Lerp(next_position.x, 0, 0.0005f);

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
