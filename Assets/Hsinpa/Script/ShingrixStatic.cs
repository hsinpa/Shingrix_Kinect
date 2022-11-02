using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shingrix
{
    public class ShingrixStatic
    {
        [System.Serializable]
        public struct ShingrixKineticData {
            public float kinect_pos_offset_x;
            public float kinect_pos_offset_y;
            public float kinect_pos_offset_z;
            public float kinect_scale;
        }

        public class Bacteria {
            public static Vector2 screenWidth = new Vector2(-2f, 2f); //Left / Right
            public static Vector2 constraintHeight = new Vector2(-5, 7); // Bottom / Top
            public static Vector2 constraintWidth = new Vector2(-3, 3); // Left / Right

            public const float spawnPosition = -7.5f;
            public const float midPosition = 2.0f;
            public const float cuttablePoint = -1.8f;

            public static Vector2 vanishPositionYZ = new Vector2(2.8f, 0);

            public static Vector3 upwardGeneralVelocity = new Vector3(0, 1, 0);
            public static Vector3 midGeneralVelocity = new Vector3(0, 0.1f, 1);

            public const int maxBacteriaSize = 10;
            public const float spawnTimeStep = 1.7f;

            public const float maxBateriaMoveSpeed = 1.05f;
            public const float minBateriaMoveSpeed = 0.9f;

            public const int maxBateriaRotateSpeed = 30;
            public const int minBateriaRotateSpeed = 0;
        }

        public class GameMode {
            public const int Time = 60;
            public const int WaitReadyTime = 3;
            public const int WaitEndingTime = 3;

            public const int LoginBackToIdleTime = 20;
        }

        public class Event {
            public const string GameModeTimeup = "event@gamemode_timeup";
            public const string GameModeCutObject = "event@gamemode_cutobject";

            public const string IdleModeEnter = "event@idlemode_enter";
            public const string LoginModeEnter = "event@loginmode_enter";
            public const string GameModeEnter = "event@gamemode_enter";
            public const string RankModeEnter = "event@rankmode_enter";

            public const string ObjPoolKeyBateria = "pool@bacteria";
            public const string ObjPoolKeybreakParticle = "pool@break_particle";
        }

        public class Data {
            public static string UserName = "";
        }

        public class IO {
            public const string GameSavePath = "game_saves.json";
            public const string KinectConfigPath = "kinect_config.json";
        }

        [System.Serializable]
        public struct RankSetsStruct {
            public List<RankStruct> sets;
        }

        [System.Serializable]
        public class RankStruct {
            public string id;
            public string name;
            public int score;
            public long timestamp;

            public bool IsValid => !string.IsNullOrEmpty(id);
        }
    }
}