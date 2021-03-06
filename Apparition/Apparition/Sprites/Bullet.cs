﻿#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace DesktopBattle
{
    /// <summary>
    /// Creates a bullet shot from a gun.
    /// </summary>
    //This class inherits from Sprite instead of Gun because it shares
    //more things in common with Sprite.
    public class Bullet : Sprite
    {
        #region Class Variables
        private int bulletSpeed = 1300; //the base speed of the hero
        private float angle; //angle of trajectory
        private string bulletName = "pictures/bullet"; //name of the bullet picture
        public int bulletDamage;
        #endregion

        /// <summary>
        /// Gives the bullet info to spawn on screen.
        /// </summary>
        /// <param name="firingAngle">Angle of the gun</param>
        /// <param name="gunPosition">Position of the gun on-screen</param>
        public void LoadContent(float firingAngle, Vector2 gunPosition, int damage)
        {
            isAlive = true;
            angle = firingAngle;
            Position = gunPosition;
            bulletDamage = damage;
            newlyCreated = false;
            base.LoadContent(bulletName);
        }

        /// <summary>
        /// Creates a basic bullet to be used later
        /// </summary>
        /// <param name="firingAngle">Angle of the gun</param>
        /// <param name="gunPosition">Position of the gun on-screen</param>
        public Bullet()
        {
            newlyCreated = false;
            base.LoadContent(bulletName);
        }

        public override void Update(GameTime gameTime)
        {
            if (Position.X < maxX && Position.X > 0 && Position.Y < maxY && Position.Y > 0)
            {
                spriteSpeed.X = bulletSpeed;
                spriteSpeed.Y = bulletSpeed;
                Vector2 up = new Vector2(1, 0);
                Matrix rotationMatrix = Matrix.CreateRotationZ(angle);
                spriteDirection = Vector2.Transform(up, rotationMatrix);
                spriteAngle = angle;
                base.Update(gameTime, spriteSpeed, spriteDirection);
            }
            else
            {
                isAlive = false;
            }
        }
    }
}
