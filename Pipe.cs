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

namespace Pong
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

        public Vector2 PipeSpritePosVector
        {
            get { return pipeSpritePosVector; }
            set {  pipeSpritePosVector = value; }
        }

        //initialize variables
        public Pipes(int x, int topPipeY, int width, int height) {
             this.x=x;
             this.topPipeY=topPipeY; 
             this.width = width;
             this.height = height;
             distanceBetweenPipes = randomNum.Next(60,70);
             bottomPipeY = topPipeY + height+ distanceBetweenPipes;
             topPipe = new Rectangle(x,topPipeY,width,height);
             bottomPipe = new Rectangle(x,bottomPipeY,width, height);
             
             
        }


    }
}
