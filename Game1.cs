using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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

        
        private Rectangle[] sourceRectangles;
        private int previousAnimationIndex;
        private int currentAnimationIndex = 0;
        private bool isAnimating = false;
        private float drawTimer = 0f;
        private const float drawInterval = 1f; // 1 second
  
        private Texture2D bottomPipeSprite;
        private Texture2D topPipeSprite;
	private Vector2 topPipeSpriteVector;
	private Vector2 bottomPipeSpriteVector;

        private Pipes topPipe;
        private Pipes bottomPipe;
	private Rectangle topPipeRect;
	private Rectangle bottomPipeRect;
	private List<Pipes> topPipesList;
        private List<Pipes> bottomPipesList;
        private List<Texture2D> topPipeTexturesList;
        private List<Texture2D> bottomPipeTexturesList;
        private bool isIntersecting = false;
        private const int numberOfPipes = 10;

	private int initialXPosition = 295; // Starting X position for the first pipe
	private const int pipeSpacing = 200; // Space between each pipe
	private const int pipeStartPosition = 295;

	protected bool Collide()
        {
            for(int i=0; i<sourceRectangles.Length; i++)
            {
                if (sourceRectangles[i].Intersects(topPipeRect))
                {
                    isIntersecting = true;
					return isIntersecting;
				}
            }
            return isIntersecting;
        }
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


            sourceRectangles = new Rectangle[3];
            sourceRectangles[0] = new Rectangle(0, 0, 35, 64);
            sourceRectangles[1] = new Rectangle(34, 0, 35, 64);
            sourceRectangles[2] = new Rectangle(69, 0, 35, 64);
         
            previousAnimationIndex = 2;

			topPipeSprite = Content.Load<Texture2D>("top_pipe_green");
			bottomPipeSprite = Content.Load<Texture2D>("pipe-green");

            topPipesList = new List<Pipes>();
            bottomPipesList = new List<Pipes>(); 
            topPipeTexturesList = new List<Texture2D>();
            bottomPipeTexturesList = new List<Texture2D>();

			for (int i = 0; i < numberOfPipes; i++)
			{
				topPipe = new Pipes(initialXPosition + (i * pipeSpacing), 0, 52, 335, 5);
				bottomPipe = new Pipes(initialXPosition + (i * pipeSpacing), 0, 52, 335, 5);
				topPipesList.Add(topPipe);
				bottomPipesList.Add(bottomPipe);
				topPipe.PipeSpritePosVector = new Vector2(topPipe.x, topPipe.topPipeY);
				bottomPipe.BottomPipeSpritePosVector = new Vector2(topPipe.x, bottomPipe.bottomPipeY);
				bottomPipeTexturesList.Add(bottomPipeSprite);
				topPipeTexturesList.Add(topPipeSprite);
			}
        }

        protected override void Update(GameTime gameTime)
        {
            position.Y += gravity;
            spritePos.Y += gravity;


	    Collide();


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



			foreach (Pipes pipe in topPipesList)
			{
				// Update the X position
				Vector2 newPosition = new Vector2(pipe.PipeSpritePosVector.X - 2, pipe.PipeSpritePosVector.Y);
				pipe.PipeSpritePosVector = newPosition;

				// Reset the position if the X coordinate is less than 0
				if (pipe.PipeSpritePosVector.X < 0)
				{
					pipe.PipeSpritePosVector = new Vector2(pipeStartPosition, pipe.PipeSpritePosVector.Y);
				}
			}


			foreach (Pipes pipe in bottomPipesList)
			{
				Vector2 newPosition = new Vector2(pipe.PipeSpritePosVector.X - 2, pipe.BottomPipeSpritePosVector.Y);
				pipe.PipeSpritePosVector = newPosition;
				if (pipe.PipeSpritePosVector.X < 0)
				{
					pipe.PipeSpritePosVector = new Vector2(pipeStartPosition, pipe.BottomPipeSpritePosVector.Y);
				}
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
            else if(!(Keyboard.GetState().IsKeyDown(Keys.Space)) && isAnimating == false)
            {
                spriteBatch.Draw(spriteSheet, spritePos, sourceRectangles[0],Color.White);
            }

			for (int i = 0; i < numberOfPipes; i++)
			{
				spriteBatch.Draw(topPipeTexturesList[i], topPipesList[i].PipeSpritePosVector, Color.White);
				spriteBatch.Draw(bottomPipeTexturesList[i], bottomPipesList[i].PipeSpritePosVector, Color.White);
			}

			spriteBatch.End();


            base.Draw(gameTime);
        }
 
    }
}
