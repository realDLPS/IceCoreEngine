using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

using System.Collections.Generic;
using System.Reflection.Metadata;

namespace MonoGameLearningProject
{
    public class QuadTreeNode
    {
        // Constant variables

        public Vector2[] ChildOffsets = { new Vector2(-0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-0.5f, -0.5f), new Vector2(0.5f, -0.5f) };


        // Static variables

        public QuadTreeNode Root;
        public bool IsRoot;
        /// <summary>
        /// Used to determine which child this is
        /// 0 is top left
        /// 1 is top right
        /// 2 is bottom left
        /// 3 is bottom right
        /// </summary>
        public int ChildIndex;
        public Vector2 Position;
        public float Size;


        // Tree related variables

        public QuadTreeNode Parent;
        public QuadTreeNode[] Children = new QuadTreeNode[4];
        public bool HasSplit = false;
        public List<QuadTreeNode> Neighbours = new List<QuadTreeNode>();

        
        // Debug

        public Texture2D debugTexture;
        public bool DrawNeighbourInfo = false;


        public QuadTreeNode(QuadTreeNode parent, int childIndex, Vector2 position, float size, bool isRoot, QuadTreeNode root)
        {
            Parent = parent;
            ChildIndex = childIndex;
            Position = position;
            Size = size;
            IsRoot = isRoot;
            Root = root;
        }


        //// Functions start here


        /// <summary>
        /// Splits the node if it hasn't split already
        /// TODO: Add check that the new child won't be smaller that the smallest allowed node
        /// </summary>
        public void Split()
        {
            if (!HasSplit)
            {
                float ChildSize = Size / 2;
                for (int i = 0; i < 4; i++)
                {
                    Children[i] = new QuadTreeNode(this, i, Position + (ChildOffsets[i] * ChildSize), ChildSize, false, (IsRoot ? this : Root));
                }
                HasSplit = true;
            }
        }

        /// <summary>
        /// WARNING! Possible performance hazard
        /// 
        /// Updates the neighbours of this node
        /// </summary>
        public void UpdateNeighbours()
        {
            if (!IsRoot)
            {
                Neighbours = Root.FindAdjacentNodes(this);
            }
        }

        /// <summary>
        /// A recursive function that finds all the adjacent nodes of the "origin" node under the node that this function is called on
        /// Usually called on the root to find all neighbours.
        /// </summary>
        /// <param name="origin">The node to compare against</param>
        /// <returns></returns>
        public List<QuadTreeNode> FindAdjacentNodes(QuadTreeNode origin)
        {
            List<QuadTreeNode> FoundNodes = new List<QuadTreeNode>();

            if (HasSplit)
            {
                foreach (QuadTreeNode Child in Children)
                {
                    FoundNodes.AddRange(Child.FindAdjacentNodes(origin));
                }
            }
            else
            {
                if (AreNodesAdjacent(origin))
                {
                    FoundNodes.Add(this);
                }
            }
            return FoundNodes;
        }

        /// <summary>
        /// Checks if this node is adjacent with the other node
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Returns true if nodes a adjacent, corners touching also counts</returns>
        public bool AreNodesAdjacent(QuadTreeNode other)
        {
            // Nodes cannot be adjacent if they are the same...
            if (other == this)
            {
                return false;
            }
            return CollisionSystem.AABB(new Square(Position, new Vector2(Size)), new Square(other.Position, new Vector2(other.Size)));
        }
        
        //// Debug functions

        public void DebugDraw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (HasSplit)
            {
                spriteBatch.Draw(texture, Position + new Vector2(256.0f), null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), Vector2.One / 256 * Size, SpriteEffects.None, 0f);
                foreach (QuadTreeNode node in Children)
                {
                    node.DebugDraw(spriteBatch, texture);
                }
            }
            if (DrawNeighbourInfo && !IsRoot)
            {
                DebugDrawSingle(spriteBatch, Root.debugTexture, 2f);
            }
        }
        public void DebugDrawSingle(SpriteBatch spriteBatch, Texture2D texture, float scaleMultiplier)
        {
            spriteBatch.Draw(texture, Position + new Vector2(256.0f), null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), Vector2.One / 256 * Size * scaleMultiplier, SpriteEffects.None, 0f);
        }
    }
}
