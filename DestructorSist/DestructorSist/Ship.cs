using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DestructorSist
{
    class Ship
    {
        public Model Model; public Matrix[] Transforms;
        //Position of the model in world space
        public Vector3 Position = Vector3.Zero;
        //Velocity of the model, applied each frame to the model's position        public Vector3 Velocity = Vector3.Zero;
        public Matrix RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2);
        private float rotation;
        public Vector3 Velocity;
        private const float VelocityScale=5.0f;
        public bool isActive=true;
        public Vector3 position;
        public Vector3 direction;
        public float speed;

        Model asteroidModel;
        Matrix[] asteroidTransforms;
        Asteroid[] asteroidList = new Asteroid[GameConstants.NumAsteroids];
        Random random=new Random();

        public float Rotation
        {
            get { return rotation; }
            set
            {
                float newVal = value;
                while (newVal >= MathHelper.TwoPi)
                {
                    newVal -= MathHelper.TwoPi;
                }
                while (newVal < 0)
                {
                    newVal += MathHelper.TwoPi;
                }
                if (rotation != value)
                {
                    rotation = value;
                    RotationMatrix =
                    Matrix.CreateRotationX(MathHelper.PiOver2) *
                    Matrix.CreateRotationZ(rotation);
                }
                /*
                if (rotation != newVal)
                {
                    rotation = newVal;
                    RotationMatrix = Matrix.CreateRotationY(rotation);
                }*/
            }
        }
        public void Update(GamePadState controllerState)
        {           
            // Rotate the model using the left thumbstick, and scale it down.            
            Rotation -= controllerState.ThumbSticks.Left.X * 0.10f;
            // Finally, add this vector to our velocity.
            Velocity += RotationMatrix.Forward * VelocityScale * controllerState.Triggers.Right;

            //Velocity += RotationMatrix.Forward * 1.0f *controllerState.Triggers.Right;
        }
    }
}
