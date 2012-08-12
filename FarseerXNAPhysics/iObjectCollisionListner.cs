using System;
using System.Collections.Generic;
using System.Text;
using FarseerGames.FarseerXNAPhysics.Dynamics;


namespace FarseerGames.FarseerXNAPhysics
{
    public interface iObjectCollisionListner
    {
        void objectCollisionOccured(RigidBody obj, string command);
    }
}
