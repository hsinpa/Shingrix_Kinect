using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EzySlice;

namespace Shingrix.Mode.Game
{
    public class CutterHandler
    {
        ITracker m_tracker;
        BacteriaSpawner m_bacteriaSpawner;
        CutPieceProcessor m_cutPieceProcessor;

        Dictionary<int, BacteriaObject> m_trackerOccupyTable = new Dictionary<int, BacteriaObject>();

        public System.Action<Vector3, Vector3> BacteriaCutEvent;

        public CutterHandler(BacteriaSpawner bacteriaSpawner, ITracker tracker) {
            m_bacteriaSpawner = bacteriaSpawner;
            m_tracker = tracker;
            m_cutPieceProcessor = new CutPieceProcessor();
        }

        public void OnUpdate() {
            ProcessPossibleCollider();

            ProcessTargetBacteria();

            m_cutPieceProcessor.OnUpdate();
        }

        public void Dispose()
        {
            m_trackerOccupyTable.Clear();
            m_cutPieceProcessor.Dispose();
        }

        private void ProcessTargetBacteria() {
            var trackers = m_tracker.GetTrackers();

            foreach (var tracker in trackers)
            {
                if (m_trackerOccupyTable.TryGetValue(tracker.index, out var bacteria)) {

                    if (bacteria == null) {
                        m_trackerOccupyTable.Remove(tracker.index);
                        continue;
                    }

                    var bacteriaBound = bacteria.Collider.bounds;
                    bacteriaBound.center = new Vector3(bacteriaBound.center.x, bacteriaBound.center.y, 0);

                    bool intersect = bacteriaBound.Intersects(tracker.bounds);

                    if (!intersect)
                    {

                        //Ignore, if cut too shallow
                        float distance = Vector3.Distance(bacteria.Contact_Point, tracker.bounds.center);

                        if (distance < (bacteria.Collider.bounds.size.x * 0.5f)) continue;

                        var bacteriaPosition = bacteria.transform.position;
                        var bacteriaScale = bacteria.transform.localScale;
                        var cutDirection = bacteria.GetVector(tracker.bounds.center);
                        var plane = Vector3.Cross(cutDirection, Vector3.forward);
                        var cutCenter = Vector3.Lerp(bacteria.Contact_Point, tracker.bounds.center, 0.5f);
                        cutCenter = bacteria.Collider.bounds.ClosestPoint(cutCenter);

                        GameObject[] cutPieces = bacteria.gameObject.SliceInstantiate(cutCenter, plane, bacteria.cutMaterial);

                        if (cutPieces != null && cutPieces.Length == 2) {
                            m_trackerOccupyTable.Remove(tracker.index);

                            m_cutPieceProcessor.Register(cutPieces[0], cutPieces[1], plane, cutDirection);

                            bacteria.PlayBreakEffect();

                            m_bacteriaSpawner.EnqueueDeleteObject(bacteria);

                            BacteriaCutEvent?.Invoke(bacteriaPosition, bacteriaScale);
                        }
                    }
                }
            }
        }

        private void ProcessPossibleCollider() {
            var allBateriaCol = m_bacteriaSpawner.BateriaList;
            var trackers = m_tracker.GetTrackers();

            foreach (var bacteria in allBateriaCol) {
                if (bacteria.transform.position.y < ShingrixStatic.Bacteria.cuttablePoint) continue;

                foreach (var tracker in trackers)
                {
                    if (m_trackerOccupyTable.ContainsKey(tracker.index)) continue;

                    var bacteriaBound = bacteria.Collider.bounds;
                    var trackBound = tracker.bounds;

                    bacteriaBound.center = new Vector3(bacteriaBound.center.x, bacteriaBound.center.y, 0);

                    bool intersect = bacteriaBound.Intersects(trackBound);

                    if (intersect) {
                        //Debug.Log("Intersect " + bacteria.name);
                        bacteria.SetCutterContactPoint(tracker.index, trackBound.center);
                        m_trackerOccupyTable.Add(tracker.index, bacteria);
                        return;
                    }
                }
            }
        }

    }
}