using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;

namespace FlappyBirdCSharp
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Texture2D texture;
        private Texture2D backgroundTexture;
        private Vector2 backgroundPos;
        private Vector2 position;
        private Vector2 spritePos; 
        private const float gravity = 4.2f;
        private Texture2D spriteSheet;

        private float timer;
        private int threshold;
        private Rectangle[] sourceRectangles;
        private int previousAnimationIndex;
        private int currentAnimationIndex = 0;
        private bool isAnimating = false;
        private float drawTimer = 0f;
        private const float drawInterval = 1f; // 1 second
  
        private Pipes pipe1;
        private Pipes pipe2;
        private Texture2D bottomPipeSprite;
        private Texture2D topPipeSprite;
        private Vector2 pipeOneSpriteVector;
        private Vector2 pipeTwoSpriteVector;
        private Rectangle topPipe;
        private Rectangle bottomPipe;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 288;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            backgroundTexture = Content.Load<Texture2D>("background-day");
            backgroundPos = new Vector2(0, 0);
            texture = Content.Load<Texture2D>("yellowbird-midflap");
            position = new Vector2(graphics.PreferredBackBufferWidth/2-20, graphics.PreferredBackBufferHeight);
            spriteSheet = Content.Load<Texture2D>("yellow_bird_spritesheet");
            spritePos = new Vector2(120, 0);

            threshold = 250;
            timer = 0;

            sourceRectangles = new Rectangle[3];
            sourceRectangles[0] = new Rectangle(0, 0, 35, 64);
            sourceRectangles[1] = new Rectangle(34, 0, 35, 64);
            sourceRectangles[2] = new Rectangle(69, 0, 35, 64);
         
            previousAnimationIndex = 2;

            pipe2 = new Pipes(295, 0, 52, 335);
            pipe1 = new Pipes(295, 0, 52, 335);
            bottomPipe = pipe2.BottomPipe;
            topPipe = pipe1.TopPipe;
            pipeOneSpriteVector = pipe1.PipeSpritePosVector;
            pipeTwoSpriteVector = new Vector2(pipe1.x, pipe2.bottomPipeY);
           

            bottomPipeSprite = Content.Load<Texture2D>("pipe-green");
            topPipeSprite = Content.Load<Texture2D>("top_pipe_green");

        }

        protected override void Update(GameTime gameTime)
        {
            position.Y += gravity;
            spritePos.Y += gravity;
            


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

        
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                position.Y -= gravity+2; 
                spritePos.Y -= gravity+2;
                isAnimating = true;

            }

            if (isAnimating)
            {
                currentAnimationIndex++;
                if (currentAnimationIndex >= sourceRectangles.Length)
                {
                    isAnimating = false;
                    currentAnimationIndex = 0;
                }
            }


            if (position.Y >516)
            {
                position.Y= graphics.PreferredBackBufferHeight/2;
                spritePos.Y = graphics.PreferredBackBufferWidth/2;
            }

            pipeOneSpriteVector.X -= 2;
            pipeTwoSpriteVector.X -= 2;
            if (pipeOneSpriteVector.X < 0 || pipeTwoSpriteVector.X <0)
            {
                pipeOneSpriteVector.X = 295;
                pipeTwoSpriteVector.X = 295;

            }

            //call the updatePipeLocation method from the Pipes class in Pipes.cs
            drawTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Rectangle sourceRectangle = new Rectangle(0, 0, 35, 64);

            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture,backgroundPos,Color.White); 
           

            // Draw only if the timer has reached the interval
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                spriteBatch.Draw(spriteSheet, spritePos, sourceRectangles[currentAnimationIndex], Color.White);
            }
            else if(!Keyboard.GetState().IsKeyDown(Keys.Space) && isAnimating==false)
            {
                spriteBatch.Draw(spriteSheet, spritePos, sourceRectangles[0],Color.White);
            }

            spriteBatch.Draw(topPipeSprite, pipeOneSpriteVector, Color.White);
            spriteBatch.Draw(bottomPipeSprite, pipeTwoSpriteVector, Color.White);

            spriteBatch.End();


            base.Draw(gameTime);
        }
 
    }
}
