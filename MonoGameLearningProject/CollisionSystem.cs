using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameVectorMath;
using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

using System.Collections.Generic;
using System.Reflection.Metadata;

namespace MonoGameLearningProject
{
    /// <summary>
    /// Handles collision using a quadtree and AABB collision detection.
    /// </summary>
    public class CollisionSystem
    {
        public QuadTreeNode QuadTreeRoot;
        public float QuadTreeSize;

        public CollisionSystem()
        {

        }

        /// <summary>
        /// Corners count as overlap, makes some stuff easier, and the function way simpler
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>True if boxes overlap</returns>
        public static bool AABB(Square a, Square b)
        {
            bool IsARight = false;
            if (a.Position.X >= b.Position.X)
            {
                IsARight = true;
            }

            // Checks is the box to the left too far away from the box on the right on the X-axis
            if (IsARight && a.Position.X - a.Size / 2 > b.Position.X + b.Size / 2)
            {
                return false;
            }
            else if (b.Position.X - b.Size / 2 > a.Position.X + a.Size / 2)
            {
                return false;
            }

            bool IsAHigh = false;
            if (a.Position.Y >= b.Position.Y)
            {
                IsAHigh = true;
            }

            // Checks if the lower box is too low from the box that is higher on the Y-axis
            if (IsAHigh && a.Position.Y - a.Size / 2 > b.Position.Y + b.Size / 2)
            {
                return false;
            }
            else if (b.Position.Y - b.Size / 2 > a.Position.Y + a.Size / 2)
            {
                return false;
            }

            return true;
        }


        public void CreateQuadTree(float size)
        {
            QuadTreeSize = size;
            QuadTreeRoot = new QuadTreeNode(null, 0, new Vector2(0.0f), QuadTreeSize, true, null);
        }

        public void RebuildQuadTree(QuadTreeNode node)
        {
            node.UpdateNeighbours();
            if (node.HasSplit)
            {
                foreach (QuadTreeNode item in node.Children)
                {
                    item.DrawNeighbourInfo = false;
                    RebuildQuadTree(item);
                }
            }
        }
    }
}
