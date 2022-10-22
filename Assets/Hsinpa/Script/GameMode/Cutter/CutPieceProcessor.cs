using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Shingrix.Mode.Game
{
    public class CutPieceProcessor
    {
        private List<PiecePair> piecePairs = new List<PiecePair>();
        private float angularVelocity = 150;
        private float speedVelocity = 2.5f;

        public CutPieceProcessor() { 
        
        }

        public void Register(GameObject pieceA, GameObject pieceB, Vector3 normal, Vector3 tangent) {
            piecePairs.Add(new PiecePair() {pieceA = pieceA, pieceB = pieceB, normal = normal, tangent = tangent });
        }

        public void Dispose()
        {
            int lens = piecePairs.Count;

            for (int i = lens - 1; i >= 0; i--)
            {
                if (piecePairs[i].pieceA != null) GameObject.Destroy(piecePairs[i].pieceA);
                if (piecePairs[i].pieceB != null) GameObject.Destroy(piecePairs[i].pieceB);
            }

            piecePairs.Clear();
        }

        public void OnUpdate() {
            ProcessPieces();
        }

        private void ProcessPieces() {
            int lens = piecePairs.Count;

            for (int i = lens - 1; i >= 0; i--) {

                var piecePair = piecePairs[i];

                bool isAliveA = ProcessPiece(piecePair.pieceA, piecePair.normal, piecePair.tangent);
                bool isAliveB = ProcessPiece(piecePair.pieceB, piecePair.normal * -1, piecePair.tangent);

                if (!isAliveA && !isAliveB)
                    piecePairs.RemoveAt(i);
            }
        }

        private bool ProcessPiece(GameObject gameObject, Vector3 normal, Vector3 tangent)
        {
            if (gameObject == null) return false;

            Vector3 position = gameObject.transform.position;

            if (position.x < ShingrixStatic.Bacteria.constraintWidth.x || position.x > ShingrixStatic.Bacteria.constraintWidth.y ||
                position.y < ShingrixStatic.Bacteria.constraintHeight.x || position.y > ShingrixStatic.Bacteria.constraintHeight.y
                ) {
                GameObject.Destroy(gameObject);
                return false;
            }

            gameObject.transform.Translate(normal * Time.deltaTime * speedVelocity, relativeTo: Space.World);
            gameObject.transform.Rotate(tangent, angularVelocity * Time.deltaTime);

            return true;
        }

        private struct PiecePair {
            public GameObject pieceA;
            public GameObject pieceB;
            public Vector3 normal;
            public Vector3 tangent;
        }

    }
}