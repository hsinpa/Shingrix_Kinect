using Shingrix;
using Shingrix.Mode.Game;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Kinect = Windows.Kinect;

namespace Hsinpa.KinectWrap {
    public class CustomBodyView : MonoBehaviour
    {
        [SerializeField]
        private GameObject handTrackPrefab;

        public BodySourceManager BodySourceManager;
        public Material mJointMat;

        private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
        private Dictionary<uint, TrackerStruct> _TrackTable = new Dictionary<uint, TrackerStruct>();
        private ShingrixStatic.ShingrixKineticData _kineticData;
        private Vector3 _kineticDataPosition;
        private Vector3 _failHandPosition = new Vector3(1000, -1000, 0);

        public List<TrackerStruct> _trackerStructs = new List<TrackerStruct>();

        private List<Kinect.JointType> _BodyPart = new List<Kinect.JointType>() {
            Kinect.JointType.HandLeft, Kinect.JointType.HandRight
        };

        public List<TrackerStruct> GetTrackerStructs() {
            _trackerStructs.Clear();

            foreach (var keyPair in _TrackTable)
                _trackerStructs.Add(keyPair.Value);

            return _trackerStructs;
        }

        public TrackerStruct GetTrackerStruct(uint id)
        {
            if (_TrackTable.TryGetValue(id, out var trackerStruct))
                return trackerStruct;
            return default(TrackerStruct);
        }

        private void Start()
        {
            _kineticData = new ShingrixStatic.ShingrixKineticData()
            {
                kinect_pos_offset_x = 0,
                kinect_pos_offset_y = 0,
                kinect_pos_offset_z = 0,
                kinect_scale = 0.5f,
                kinect_depth_min = 5, kinect_depth_max = 13
            };

            string rawData = Hsinpa.Utility.IOUtility.GetFileText(Path.Combine(Application.streamingAssetsPath, ShingrixStatic.IO.KinectConfigPath));
            if (!string.IsNullOrEmpty(rawData))
                _kineticData = JsonUtility.FromJson<ShingrixStatic.ShingrixKineticData>(rawData);

            _kineticDataPosition = new Vector3(_kineticData.kinect_pos_offset_x, _kineticData.kinect_pos_offset_y, _kineticData.kinect_pos_offset_z);

            Debug.Log($"_kineticData pos {_kineticData.kinect_pos_offset_x} {_kineticData.kinect_pos_offset_y} {_kineticData.kinect_pos_offset_z}, scale {_kineticData.kinect_scale}");
        }

        void Update()
        {
            Kinect.Body[] data = BodySourceManager.GetData();
            if (data == null)
            {
                return;
            }

            List<ulong> trackedIds = new List<ulong>();
            foreach (var body in data)
            {
                if (body == null)
                {
                    continue;
                }

                if (body.IsTracked)
                {
                    trackedIds.Add(body.TrackingId);
                }
            }

            List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

            // First delete untracked bodies
            foreach (ulong trackingId in knownIds)
            {
                if (!trackedIds.Contains(trackingId))
                {
                    Destroy(_Bodies[trackingId]);
                    _Bodies.Remove(trackingId);
                    _TrackTable.Remove((uint)trackingId);
                }
            }

            foreach (var body in data)
            {
                if (body == null)
                {
                    continue;
                }
                if (body.IsTracked)
                {
                    if (!_Bodies.ContainsKey(body.TrackingId))
                    {
                        _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                    }

                    RefreshBodyObject(body, _Bodies[body.TrackingId]);
                }
            }
        }

        private GameObject CreateBodyObject(ulong id)
        {
            GameObject body = new GameObject("Body:" + id);
            int bodyPartLens = _BodyPart.Count;

            for (uint i = 0; i < bodyPartLens; i++)
            {
                //GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                GameObject jointObj = GameObject.Instantiate(handTrackPrefab);

                jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                jointObj.name = _BodyPart[(int)i].ToString();
                jointObj.transform.parent = body.transform;
                jointObj.transform.position = new Vector3(-10, -10, 0);
                uint part_id = (uint) ((id * 10) + i);

                _TrackTable.Add(part_id, new TrackerStruct() {
                    index = (int)part_id,
                    position = jointObj.transform.position,
                    bounds = new Bounds(jointObj.transform.position, new Vector3(0.1f, 0.1f,0.1f)) 
                });
            }

            return body;
        }

        private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
        {
            int bodyPartLens = _BodyPart.Count;

            for (uint i = 0; i < bodyPartLens; i++)
            {
                uint part_id = (uint)((body.TrackingId * 10) + i);
                Kinect.JointType jt = _BodyPart[(int)i];

                Kinect.Joint sourceJoint = body.Joints[jt];

                Transform jointObj = bodyObject.transform.Find(jt.ToString());
                MeshRenderer meshRender = jointObj.GetComponent<MeshRenderer>();
                jointObj.localPosition = GetVector3FromJoint(sourceJoint);

                jointObj.localPosition = jointObj.localPosition * _kineticData.kinect_scale;
                jointObj.localPosition = jointObj.localPosition + _kineticDataPosition;
                float depth = jointObj.localPosition.z;

                if (_TrackTable.TryGetValue(part_id, out var trackerStruct)) {
                    bool is_depth_valid = depth < _kineticData.kinect_depth_max && depth > _kineticData.kinect_depth_min;
                    var jointPosition = (is_depth_valid) ? jointObj.localPosition : _failHandPosition;
                    jointPosition.z = 0;

                    meshRender.enabled = (is_depth_valid);
                    
                    trackerStruct.UpdatePosition(jointPosition);

                    _TrackTable[part_id] = trackerStruct;
                }
            }
        }

        private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
        {
            return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
        }



    }
}
