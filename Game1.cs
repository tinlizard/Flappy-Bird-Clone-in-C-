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

        private Texture2D backgroundTexture; 
        private Vector2 backgroundPos;
        private Vector2 position;
        private Vector2 spritePos; 
        private const float gravity = 4.2f;
        private Texture2D spriteSheet;

        
        private Rectangle[] sourceRectangles;
        private int currentAnimationIndex = 0;
        private bool isAnimating = false;
        private float drawTimer = 0f;
        private const float drawInterval = 0.1f; // 1 second
  
        private Texture2D bottomPipeSprite;
        private Texture2D topPipeSprite;
		

        private Pipes topPipe;
        private Pipes bottomPipe;
	private Rectangle topPipeRect;
	private List<Pipes> topPipesList;
        private List<Pipes> bottomPipesList;
        private List<Texture2D> topPipeTexturesList;
        private List<Texture2D> bottomPipeTexturesList;
        private bool isIntersecting = false;
        private const int numberOfPipes = 5;

	private int initialXPosition = 295; // Starting X position for the first pipe
	private const int pipeSpacing = 90; // Space between each pipe

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
            //Set screen width and height
            graphics.PreferredBackBufferWidth = 288;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load background textures
            backgroundTexture = Content.Load<Texture2D>("background-day");
            backgroundPos = new Vector2(0, 0);

            //Load bird spritesheet
            spriteSheet = Content.Load<Texture2D>("yellow_bird_spritesheet");
            spritePos = new Vector2(120, 0);

            //create sourceRectangles elements
            sourceRectangles = new Rectangle[3];
            sourceRectangles[0] = new Rectangle(0, 0, 35, 64);
            sourceRectangles[1] = new Rectangle(34, 0, 35, 64);
            sourceRectangles[2] = new Rectangle(69, 0, 35, 64);
         

            //load pipe sprite
	    topPipeSprite = Content.Load<Texture2D>("top_pipe_green");
	    bottomPipeSprite = Content.Load<Texture2D>("pipe-green");

            topPipesList = new List<Pipes>();
            bottomPipesList = new List<Pipes>(); 
            topPipeTexturesList = new List<Texture2D>();
            bottomPipeTexturesList = new List<Texture2D>();

        }

        protected override void Update(GameTime gameTime)
        {
            //pull bird down
            position.Y += gravity;
            spritePos.Y += gravity;


			Collide();


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //if the space key is pressed, pull the bird up and set isAnimating to true
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                position.Y -= gravity+2; 
                spritePos.Y -= gravity+2;
                isAnimating = true;

            }

            /*if isAnimating is true and drawTimer is greater than 0.2 seconds, and reset the drawTimer and reset the 
             currentAnimationIndex*/
            if (isAnimating && (drawTimer>drawInterval))
            {
                currentAnimationIndex++;
                if (currentAnimationIndex >= sourceRectangles.Length)
                {
                    isAnimating = false;
                    currentAnimationIndex = 0;
                }
                drawTimer = 0;
            }

            //if the bird y position is greater than 516, set the bird height to halfway down the screen
            if (position.Y >516)
            {
                position.Y= graphics.PreferredBackBufferHeight/2;
                spritePos.Y = graphics.PreferredBackBufferWidth/2;
            }

            //create ten pipes 
	    for (int i = 0; i < numberOfPipes; i++)
	    {
                /*create new instance of pipe, add top pipes to top pipes list, then set the topPipe vector, and set the 
                 sprites to the textures list. Do the same for the bottom pipes.*/
		topPipe = new Pipes(initialXPosition + (i * pipeSpacing), 0, 52, 335, 5);
		bottomPipe = new Pipes(initialXPosition + (i * pipeSpacing), 0, 52, 335, 5);
		topPipesList.Add(topPipe);
		bottomPipesList.Add(bottomPipe);
		topPipe.PipeSpritePosVector = new Vector2(topPipe.x, topPipe.topPipeY);
		bottomPipe.BottomPipeSpritePosVector = new Vector2(topPipe.x, bottomPipe.bottomPipeY);
		bottomPipeTexturesList.Add(bottomPipeSprite);
		topPipeTexturesList.Add(topPipeSprite);
	   }

            //for every pipe in topPipesList, update the position of the topPipes
	    foreach (Pipes pipe in topPipesList)
	    {
	    // Update the X position
	    Vector2 newPosition = new Vector2(pipe.PipeSpritePosVector.X - 2, pipe.PipeSpritePosVector.Y);
	    pipe.PipeSpritePosVector = newPosition;

	    // Reset the position if the X coordinate is less than 0
	   	 if (pipe.PipeSpritePosVector.X < 0)
	   	 {
	      		pipe.PipeSpritePosVector = new Vector2(pipe.x, pipe.PipeSpritePosVector.Y);
	     	 }
	}

            //same code but for bottom pipes
			foreach (Pipes pipe in bottomPipesList)
			{
				// Update the X position
				Vector2 newPosition = new Vector2(pipe.PipeSpritePosVector.X - 2, pipe.BottomPipeSpritePosVector.Y);
				pipe.PipeSpritePosVector = newPosition;

				// Reset the position if the X coordinate is less than 0
				if (pipe.PipeSpritePosVector.X < 0)
				{
					pipe.PipeSpritePosVector = new Vector2(pipe.x , pipe.BottomPipeSpritePosVector.Y);
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

            //begine the spriteBatch instance and start drawing
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture,backgroundPos,Color.White);



			//Draw the currentAnimationIndex of the sourceRectangles when space is pressed
			if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                spriteBatch.Draw(spriteSheet, spritePos, sourceRectangles[currentAnimationIndex], Color.White);
            }
            //If the space key is pressed and is animating is false, draw only the first index of the sourceRectangle
            else if(!(Keyboard.GetState().IsKeyDown(Keys.Space)))
            {
                spriteBatch.Draw(spriteSheet, spritePos, sourceRectangles[0],Color.White);
            }

            //draw each element in the topPipesTexturesList list and bottomPipeTexturesList
			for (int i = 0; i < numberOfPipes; i++)
			{
				
				spriteBatch.Draw(topPipeTexturesList[i], topPipesList[i].PipeSpritePosVector, Color.White);
				spriteBatch.Draw(bottomPipeTexturesList[i], bottomPipesList[i].PipeSpritePosVector, Color.White);
				
			}

            //end drawing
			spriteBatch.End();


            base.Draw(gameTime);
        }
 
    }
}
