using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DestructorSist
{
    class GameConstants
    {
        public const float CameraHeight=25000.0f;
        public const float PlayfieldSizeX=15000f;
        public const float PlayfieldSizeY=11500f;
        //asteroidconstants
        public const int NumAsteroids=10;
        public const float AsteroidMinSpeed=100.0f;
        public const float AsteroidMaxSpeed=300.0f;
        public const float AsteroidSpeedAdjustment=5.0f;

        public const float AsteroidBoundingSphereScale=0.95f; //95%size
        public const float ShipBoundingSphereScale=0.5f; //50%size
    }
}
