using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace FlappyBirdCSharp
{
     class Pipes
    {
        //declare variables
        public int x;
        public int topPipeY;
        public int bottomPipeY;
        public Random randomNum = new Random();
        public int distanceBetweenPipes;
        public Rectangle topPipe;
        public Rectangle bottomPipe;
        public List<Rectangle> bottomRectanglesList= new List<Rectangle>();
        public List<Rectangle> topRectanglesList = new List<Rectangle>();
        private int pipeSpacing;
        public Rectangle BottomPipe
        {
            get { return bottomPipe; }
            set { bottomPipe = value; } 
        }
        public Rectangle TopPipe
        {
            get { return topPipe; }
            set { topPipe = value; }
        }
        public int width;
        public int height;
        private Vector2 pipeSpritePosVector;
        private Vector2 bottomPipeSpritePosVector;

        public Vector2 PipeSpritePosVector
        {
            get { return pipeSpritePosVector; }
            set {  pipeSpritePosVector = value; }
        }
        public Vector2 BottomPipeSpritePosVector
        {
			get { return bottomPipeSpritePosVector; }
			set { bottomPipeSpritePosVector = value; }
		}

        //initialize variables
        public Pipes(int x, int topPipeY, int width, int height, int pipeSpacing) {
             this.x=x;
             this.topPipeY=topPipeY; 
             this.width = width;
             this.height = height;
             this.pipeSpacing=pipeSpacing;
             distanceBetweenPipes = randomNum.Next(60,70);
             bottomPipeY = topPipeY + height+ distanceBetweenPipes;
             topPipe = new Rectangle(x,topPipeY,width,height);
             bottomPipe = new Rectangle(x,bottomPipeY,width, height);
             
             
        }
       


    }
}
