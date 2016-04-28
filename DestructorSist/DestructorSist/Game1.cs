using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DestructorSist//me quedé en el step 3
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Ship ship = new Ship();
        Vector3 cameraPosition=new Vector3(0.0f,0.0f,25000.0f);
        Matrix projectionMatrix;
        Matrix viewMatrix;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),GraphicsDevice.DisplayMode.AspectRatio,20000.0f, 30000.0f);
            viewMatrix = Matrix.CreateLookAt(cameraPosition,
            Vector3.Zero, Vector3.Up);

            base.Initialize();
        }

        SoundEffect soundEngine;
        SoundEffectInstance soundEngineInstance;
        SoundEffect soundHyperspaceActivation;
        //Vector3 modelPosition = Vector3.Zero;
        //float modelRotation = 0.0f;
        //Vector3 cameraPosition = new Vector3(0.0f, 50.0f, 5000.0f);
        Model myModel;
        float aspectRatio;
        //Vector3 modelVelocity=Vector3.Zero;

        private Matrix[] SetupEffectDefaults(Model myModel)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = projectionMatrix;
                    effect.View = viewMatrix;
                }
            }
            return absoluteTransforms;
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            ship.Model=Content.Load<Model>("Models/p1_wedge");
            ship.Transforms = SetupEffectDefaults(ship.Model);
            //spriteBatch = new SpriteBatch(GraphicsDevice);
            //myModel = Content.Load<Model>("Models\\p1_wedge");
            soundEngine = Content.Load<SoundEffect>("Waves\\engine_2");
            soundEngineInstance = soundEngine.CreateInstance();
            soundHyperspaceActivation = Content.Load<SoundEffect>("Waves\\hyperspace_activate");
            //aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            
        }
        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //tutorial1
            //modelRotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.1f);
            
            // Get some input.  
            UpdateInput();
            // Add velocity to the current position.
            //modelPosition += modelVelocity;
            //modelVelocity *= 0.95f;
            // Add velocity to the current position.   
            ship.Position += ship.Velocity;
            // Bleed off velocity over time.    
            ship.Velocity *= 0.95f;

            base.Update(gameTime);
        }

        protected void UpdateInput()
        {
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            KeyboardState currentKeyState = Keyboard.GetState();

            ship.Update(currentState);
            /*
            if (currentKeyState.IsKeyDown(Keys.A))
                modelRotation += 0.10f;
            else if (currentKeyState.IsKeyDown(Keys.D))
                modelRotation -= 0.10f;
            else
                modelRotation -= currentState.ThumbSticks.Left.X * 0.10f;
            Vector3 modelVelocityAdd = Vector3.Zero;
            modelVelocityAdd.X = -(float)Math.Sin(modelRotation);
            modelVelocityAdd.Z = -(float)Math.Cos(modelRotation);

            if (currentKeyState.IsKeyDown(Keys.W))
                modelVelocityAdd *= 1;
            else
                modelVelocityAdd *= currentState.Triggers.Right;

            modelVelocity += modelVelocityAdd;
            */
            GamePad.SetVibration(PlayerIndex.One,currentState.Triggers.Right,currentState.Triggers.Right);

            if (currentState.Buttons.A == ButtonState.Pressed || currentKeyState.IsKeyDown(Keys.Enter))
                {
                    ship.Position = Vector3.Zero;
                    ship.Velocity = Vector3.Zero;
                    ship.Rotation = 0.0f;
                    soundHyperspaceActivation.Play();
                }
            if (currentState.Triggers.Right > 0)
            {
                if (soundEngineInstance.State == SoundState.Stopped)
                    {
                        soundEngineInstance.Volume = 0.75f;
                        soundEngineInstance.IsLooped = true;
                        soundEngineInstance.Play();
                    }
                    else soundEngineInstance.Resume();
                }
                else if (currentState.Triggers.Right == 0)
                {
                    if (soundEngineInstance.State == SoundState.Playing)
                        soundEngineInstance.Pause();
                }

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Matrix shipTransformMatrix = ship.RotationMatrix * Matrix.CreateTranslation(ship.Position);
            DrawModel(ship.Model, shipTransformMatrix, ship.Transforms);
            base.Draw(gameTime);

            //Matrix[] transforms = new Matrix[myModel.Bones.Count];
            //myModel.CopyAbsoluteBoneTransformsTo(transforms);
            /*
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(modelRotation) * Matrix.CreateTranslation(modelPosition);
                    effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f,10000.0f);
                }
                mesh.Draw();
            }
            */


        }

        

        public static void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)
        {
            //Draw the model, a model can have multiple meshes, so loop    
            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set     
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform;
                }
                //Draw the mesh, will use the effects set above.   
                mesh.Draw();
            }
        }
    }
}
