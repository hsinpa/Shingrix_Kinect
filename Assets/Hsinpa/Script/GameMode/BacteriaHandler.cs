using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shingrix.Mode.Game {
    public class BacteriaSpawner
    {
        private GameObject m_bacteriaPrefab;
        private Transform m_container;

        private List<GameObject> m_bateriaList = new List<GameObject>();
        private List<GameObject> m_waitForDeletionPipe = new List<GameObject>();

        private float m_bacMoveSpeed = 1;
        private int m_bacLength = 0;
        private float nextSpawnTime;

        private Vector3 _cacheVector3 = new Vector3();

        public BacteriaSpawner(GameObject bacteriaPrefab, Transform container) {
            this.m_bacteriaPrefab = bacteriaPrefab;
            this.m_container = container;
        }

        public void OnUpdate() {
            ProcessDeleteObj();
            ProcessBacteriaSpawn();
            ProcessBacteriaMovement();

        }

        public void Dispose() {
            Hsinpa.Utility.UtilityFunc.ClearChildObject(m_container);
            m_waitForDeletionPipe.Clear();
            m_bateriaList.Clear();
            m_bacLength = 0;
            nextSpawnTime = Time.time + ShingrixStatic.Bacteria.spawnTimeStep;
        }

        private void RemoveBateria(GameObject deleteObject) {
            m_bateriaList.Remove(deleteObject);
            m_bacLength--;

            GameObject.Destroy(deleteObject);
        }

        private void ProcessBacteriaSpawn() {
            if (nextSpawnTime < Time.time && m_bacLength < ShingrixStatic.Bacteria.maxBacteriaSize) {
                GameObject spawnBateria = GameObject.Instantiate(this.m_bacteriaPrefab, this.m_container);

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

                // Upward
                if (worldPosition.y < ShingrixStatic.Bacteria.midPosition) 
                {
                    bacteriaObj.transform.Translate(ShingrixStatic.Bacteria.upwardGeneralVelocity * Time.deltaTime, relativeTo: Space.World);
                }

                // Passing mid point, move backward
                if (worldPosition.y > ShingrixStatic.Bacteria.midPosition)
                {
                    bacteriaObj.transform.Translate(ShingrixStatic.Bacteria.midGeneralVelocity * Time.deltaTime, relativeTo: Space.World);
                }

                //Reach vanish point, delete it
                if (worldPosition.y > ShingrixStatic.Bacteria.vanishPositionYZ.x && worldPosition.z > ShingrixStatic.Bacteria.vanishPositionYZ.y) {
                    m_waitForDeletionPipe.Add(bacteriaObj);
                    return;
                }
            }
        }

        private void ProcessDeleteObj() {
            foreach (var deletionObj in m_waitForDeletionPipe) {
                RemoveBateria(deletionObj);
            }
        }
    }
}