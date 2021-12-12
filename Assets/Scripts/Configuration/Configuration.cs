using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Configuration
{
    public static class Config
    {
        public const float XMin = -27;
        public const float XMax = 27;
        public const float ZMin = -55;
        public const float ZMax = 55;

        public static readonly Vector3 StartPosition = new Vector3(0, 1, 0);

        public const string GameId = "";
    }
}
