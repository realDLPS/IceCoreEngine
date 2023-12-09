using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

using System.Collections.Generic;
using System.Reflection.Metadata;
using nkast.Aether.Physics2D;
using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Diagnostics;

namespace IceCoreEngine
{
    /// <summary>
    /// 
    /// </summary>
    public class CollisionSystem
    {
        IceCoreGame _game;

        public CollisionSystem(IceCoreGame game)
        {
            _game = game;
        }

        /// <summary>
        /// Casts a ray from the start to the end
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="callback"></param>
        public void LinetraceSingle(Vector2 start, Vector2 end, FinishedLineTrace callback)
        {
            new RayCaster(_game, start, end, true, callback);
        }

        /// <summary>
        /// Used for doing a trace
        /// </summary>
        private class RayCaster
        {
            IceCoreGame GameInstance;
            List<LinetraceResponse> Hits = new List<LinetraceResponse>();
            List<Category> Categories = new List<Category>();
            Vector2 Start;
            Vector2 End;
            FinishedLineTrace Callback;
            bool Hit = false;
            bool IsSingle;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="game">Game instance</param>
            /// <param name="start">Start position</param>
            /// <param name="end">End position</param>
            /// <param name="single">Should this trace only return the closest hit</param>
            /// <param name="callback">Callback to call once done tracing</param>
            public RayCaster(IceCoreGame game, Vector2 start, Vector2 end, bool single, FinishedLineTrace callback)
            {
                GameInstance = game;
                Start = start;
                End = end;
                Callback = callback;
                IsSingle = single;

                // Not fully implemented yet
                Categories.Add(Category.Cat1);

                // Add the end point to the hits
                // Removed if a hit is gotten
                Hits.Add(new LinetraceResponse(null, End, Vector2.Zero));

                // Actually cast the ray
                GameInstance.GetWorld().RayCast(LinetraceHandler, start, end);
                Done();
            }

            private float LinetraceHandler(Fixture fixture, Vector2 point, Vector2 normal, float fraction)
            {
                // Check is the fixture in a category that should block this ray
                if (Categories.Contains(fixture.CollisionCategories))
                {
                    // If there hasn't been a hit before clear the hits.
                    if (Hit == false)
                    {
                        Hit = true;
                        Hits.Clear();
                    }
                    Hits.Add(new LinetraceResponse(fixture, point, normal));

                    // Continue without clipping the ray
                    return 1;
                }
                else
                {
                    // Just passthrough
                    return -1f;
                }
            }

            private void Done()
            {
                Callback.Invoke(IsSingle ? (new List<LinetraceResponse> { GetClosestHit() }) : Hits);
            }

            /// <summary>
            /// Returns the closest hit to the start position
            /// 
            /// Assumes atleast one hit exists
            /// </summary>
            /// <returns></returns>
            private LinetraceResponse GetClosestHit()
            {
                LinetraceResponse ClosestHit;

                // Lets assume for a start that the first hit is the closest, although it might not be...
                ClosestHit = Hits[0];
                // Remove the first hit from the hits
                Hits.RemoveAt(0);

                // Loop over all of the other hits
                foreach (LinetraceResponse hit in Hits)
                {
                    if (Vector2.DistanceSquared(ClosestHit.Position, Start) > Vector2.DistanceSquared(hit.Position, Start))
                    {
                        ClosestHit = hit;
                    }
                }
                return ClosestHit;
            }
        }

        /// <summary>
        /// Corners and edges count as overlap, makes some stuff easier, and the function way simpler
        /// 
        /// Currently not used. May be used for UI in the future.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>True if boxes overlap</returns>
        public static bool IceCoreAABB(Square a, Square b)
        {
            bool IsARight = false;
            if (a.Position.X >= b.Position.X)
            {
                IsARight = true;
            }

            // Checks is the box to the left too far away from the box on the right on the X-axis
            if (IsARight && a.Position.X - a.Scale.X / 2 > b.Position.X + b.Scale.X / 2)
            {
                return false;
            }
            else if (b.Position.X - b.Scale.X / 2 > a.Position.X + a.Scale.X / 2)
            {
                return false;
            }

            bool IsAHigh = false;
            if (a.Position.Y >= b.Position.Y)
            {
                IsAHigh = true;
            }

            // Checks if the lower box is too low from the box that is higher on the Y-axis
            if (IsAHigh && a.Position.Y - a.Scale.Y / 2 > b.Position.Y + b.Scale.Y / 2)
            {
                return false;
            }
            else if (b.Position.Y - b.Scale.Y / 2 > a.Position.Y + a.Scale.Y / 2)
            {
                return false;
            }

            return true;
        }
        public static bool IsPointInSquare(Square square, Vector2 point, float squareRotation)
        {
            Vector2 LeftTop = new Vector2(-0.5f * square.Scale.X, 0.5f * square.Scale.Y);
            Vector2 RightBottom = new Vector2(0.5f * square.Scale.X, -0.5f * square.Scale.Y);

            Vector2 LocalSpacePoint = point - square.Position;
            Vector2 RotatedPoint = ICVec2Math.RotateVector(LocalSpacePoint, squareRotation);

            if(RotatedPoint.X < LeftTop.X || RotatedPoint.X > RightBottom.X)
            {
                return false;
            }
            if(RotatedPoint.Y > LeftTop.Y || RotatedPoint.Y < RightBottom.Y)
            {
                return false;
            }
            return true;
        }
    }
}
