using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DestructorSist
{
    struct Asteroid
    {
        public Vector3 position;
        public Vector3 direction;
        public float speed;
    public void Update(float delta)
    {
        position += direction * speed * GameConstants.AsteroidSpeedAdjustment * delta;
    }
    }
}
