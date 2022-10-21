using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shingrix
{
    public class ShingrixStatic
    {


        public class Bacteria {
            public static Vector2 screenWidth = new Vector2(-1.5f, 1.5f);

            public const float spawnPosition = -7;
            public const float midPosition = 2.25f;
            public static Vector2 vanishPositionYZ = new Vector2(8, 10);

            public static Vector3 upwardGeneralVelocity = new Vector3(0, 1, 0);
            public static Vector3 midGeneralVelocity = new Vector3(0, 0.5f, 1);

            public const int maxBacteriaSize = 10;
            public const float spawnTimeStep = 1.7f;
        }

    }
}