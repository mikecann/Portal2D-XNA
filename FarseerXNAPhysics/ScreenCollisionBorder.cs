using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using FarseerGames.FarseerXNAPhysics;
using FarseerGames.FarseerXNAPhysics.Dynamics;

namespace FarseerDemo6 {
    public class ScreenCollisionBorder {
        public RectangleRigidBody LeftBorder;
        public RectangleRigidBody RightBorder;
        public RectangleRigidBody TopBorder;
        public RectangleRigidBody BottomBorder;
        public float borderThickness = 40;

        public ScreenCollisionBorder(float screenWidth, float screenHeight, PhysicsSimulator physicsSimulator ) {
            LeftBorder = new RectangleRigidBody(borderThickness, screenHeight, 1);
            LeftBorder.Position = new Vector2(-borderThickness/2,screenHeight/2);
            LeftBorder.FrictionCoefficient = .3f;
            LeftBorder.IsStatic = true;

            RightBorder = new RectangleRigidBody(borderThickness, screenHeight, 1);
            RightBorder.Position = new Vector2(screenWidth + borderThickness / 2, screenHeight / 2);
            RightBorder.FrictionCoefficient = .3f;
            RightBorder.IsStatic = true;

            TopBorder = new RectangleRigidBody(screenWidth, borderThickness, 1);
            TopBorder.Position = new Vector2(screenWidth / 2, -borderThickness / 2);
            TopBorder.FrictionCoefficient = .3f;
            TopBorder.IsStatic = true;

            BottomBorder = new RectangleRigidBody(screenWidth, borderThickness, 1);
            BottomBorder.Position = new Vector2(screenWidth / 2, screenHeight + borderThickness / 2);
            BottomBorder.FrictionCoefficient = .3f;
            BottomBorder.IsStatic = true;

            physicsSimulator.Add(LeftBorder);
            physicsSimulator.Add(RightBorder);
            physicsSimulator.Add(TopBorder);
            physicsSimulator.Add(BottomBorder);
        }
    }
}
