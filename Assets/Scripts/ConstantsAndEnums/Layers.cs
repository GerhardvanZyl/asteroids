using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ConstantsAndEnums
{
    public enum Layers
    {
        Default = 0,
        TransparentFX = 1,
        IgnoreRayCast = 2,
        Water = 4,
        UI = 5,
        CanvasUI = 8,
        Projectiles = 9,
        Player = 10,
        Asteroids = 11,
        NonCollidingAsteroids = 12
    }
}
