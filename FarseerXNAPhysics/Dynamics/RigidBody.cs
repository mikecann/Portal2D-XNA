using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using FarseerGames.FarseerXNAPhysics.Collisions;



namespace FarseerGames.FarseerXNAPhysics.Dynamics {
    public class RigidBody : Body, ICollideable<RigidBody>, IEquatable<RigidBody>{
        protected Geometry geometry;
        protected Grid grid;
        private int id;
        private int collisionGroup = 0;
        public iObjectCollisionListner _collisionListner;
        public string _actionCommand;



        public void setCollisionListner(iObjectCollisionListner ocl, string command)
        {
            _collisionListner = ocl;
            _actionCommand = command;
        }
                     
        public RigidBody() {
           id = RigidBody.GetNextId();
           _collisionListner = null;
        }

        public RigidBody(Geometry geometry, Grid grid) {
            id = RigidBody.GetNextId();
            this.grid = grid;
            this.geometry = geometry;
            _collisionListner = null;
        }

        public sealed override Vector2 Position {
            get {
                return base.Position;
            }
            set {
                base.Position = value;
                if (geometry != null) {
                    geometry.Update(value, Orientation);
                }
            }
        }

        public sealed override float Orientation {
            get {
                return base.Orientation;
            }
            set {
                base.Orientation = value;
                if (geometry != null) {
                    geometry.Update(Position, value);
                }
            }
        }

        public Geometry Geometry {
            get { return geometry; }
            set { 
                geometry = value;
                geometry.Update(Position, Orientation);
            }
        }

        public Grid Grid {
            get { return grid; }
            set { grid = value; }
        }

        public int CollisionGroup {
            get { return collisionGroup; }
            set { collisionGroup = value; }
        }
	

        public override void IntegratePosition(float dt) {
            base.IntegratePosition(dt);
            geometry.Update(Position, Orientation);
        }



        public void Collide(RigidBody rigidBody, ContactList contactList) {
            Feature feature; ;
            Vector2 localVertex;
            int vertexIndex = -1;
            foreach (Vector2 vertex in rigidBody.geometry.WorldVertices) {
                if (contactList.Count == contactList.Capacity) { return; }
                if (grid == null) { break; }//grid can be null for "one-way" collision (points)
                vertexIndex += 1;
                localVertex = geometry.ConvertToLocalCoordinates(vertex);
                feature = grid.Evaluate(localVertex);
                if (feature.Distance < 0f) {
                    feature.Normal = geometry.ConvertToWorldOrientation(feature.Normal);
                    Contact contact = new Contact(vertex, feature.Normal, feature.Distance, new ContactId(2, vertexIndex, 1));
                    contactList.Add(contact);
                                                 
					// ADDED BY MC 24/11/06
                    if (contact.Normal.Y == 1) { OnGround = true; }
                    if (contact.Normal.Y == -1) { rigidBody.OnGround = true; }  
                }
            }
            foreach (Vector2 vertex in geometry.WorldVertices) {
                if (rigidBody.grid == null) { return; } //grid can be null for "one-way" collision(points)
                vertexIndex += 1;
                localVertex = rigidBody.geometry.ConvertToLocalCoordinates(vertex);
                feature = rigidBody.grid.Evaluate(localVertex);
                if (feature.Distance < 0f) {
                    feature.Normal = rigidBody.geometry.ConvertToWorldOrientation(feature.Normal);
                    feature.Normal = -feature.Normal; //normals must point in same direction.
                    Contact contact = new Contact(vertex, feature.Normal, feature.Distance, new ContactId(1, vertexIndex, 2));
                    contactList.Add(contact);
                    
					// ADDED BY MC 24/11/06
                    if (contact.Normal.Y == 1) { OnGround = true; }
                    if (contact.Normal.Y == -1) { rigidBody.OnGround = true; }  					
                }
            }
        }

        public bool Equals(RigidBody other) {
            return id == other.id;
        }

        public override int GetHashCode() {
           return base.GetHashCode();
        }

        public override bool Equals(object obj) {
            if (!(obj is RigidBody)) { throw new ArgumentException("The object being compared must be of type 'RigidBody'"); }
            return Equals((RigidBody)obj);
        }

        public static bool operator ==(RigidBody rigidBody1, RigidBody rigidBody2) {
            return rigidBody1.Equals(rigidBody2);
        }

        public static bool operator !=(RigidBody rigidBody1, RigidBody rigidBody2) {
            return !rigidBody1.Equals(rigidBody2);
        }

        public static bool operator <(RigidBody rigidBody1, RigidBody rigidBody2) {
            return rigidBody1.Id < rigidBody2.Id;
        }

        public static bool operator >(RigidBody rigidBody1, RigidBody rigidBody2) {
            return rigidBody1.Id > rigidBody2.Id;
        }

        internal int Id {
            get { return id; }
        }

        private static int _newId = -1;
        
        public static int GetNextId() {
            _newId += 1;
            return _newId;
        }
    }
}
