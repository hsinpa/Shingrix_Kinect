using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shingrix
{
    public class ShingrixStatic
    {
        public class Bacteria {
            public static Vector2 screenWidth = new Vector2(-2f, 2f); //Left / Right
            public static Vector2 constraintHeight = new Vector2(-5, 7); // Bottom / Top
            public static Vector2 constraintWidth = new Vector2(-3, 3); // Left / Right

            public const float spawnPosition = -7.5f;
            public const float midPosition = 0.5f;
            public const float cuttablePoint = -1.8f;

            public static Vector2 vanishPositionYZ = new Vector2(3.8f, 0);

            public static Vector3 upwardGeneralVelocity = new Vector3(0, 1, 0);
            public static Vector3 midGeneralVelocity = new Vector3(0, 0.4f, 1);

            public const int maxBacteriaSize = 10;
            public const float spawnTimeStep = 1.7f;
        }

        public class GameMode {
            public const int Time = 10;
            public const int WaitReadyTime = 3;
            public const int WaitEndingTime = 8;
        }

        public class Event {

            public const string GameModeTimeup = "event@gamemode_timeup";
            public const string GameModeCutObject = "event@gamemode_cutobject";

            public const string IdleModeEnter = "event@idlemode_enter";
            public const string IdleModeEnd = "event@idlemode_end";

            public const string GameModeEnter = "event@gamemode_enter";
            public const string RankModeEnter = "event@rankmode_enter";
        }

        public class Data {
            public static string UserName = "";
            public static int UserScore = 0;
        }
    }
}