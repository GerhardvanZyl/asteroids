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
        public const float XMin = -56;
        public const float XMax = 56;
        public const float ZMin = -100;
        public const float ZMax = 100;

        public static readonly Vector3 StartPosition = new Vector3(0, 1, 0);
    }
}
