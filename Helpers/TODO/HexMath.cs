using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawnfall
{
    public static class HexMath
    {


        public static HexEdgeDir nextEdge(HexEdgeDir edgeDir)
        {
            return (edgeDir == HexEdgeDir.TL) ? HexEdgeDir.TR : edgeDir + 1;
        }
        public static HexEdgeDir nextEdge(HexEdgeDir edgeDir, int distance)
        {
            while (distance > 0)
            {
                edgeDir = nextEdge(edgeDir);
                distance--;
            }
            return edgeDir;
        }
        public static HexEdgeDir prevEdge(HexEdgeDir edgeDir)
        {
            return (edgeDir == HexEdgeDir.TR) ? HexEdgeDir.TL : edgeDir - 1;
        }
        public static HexEdgeDir prevEdge(HexEdgeDir edgeDir, int distance)
        {
            while (distance > 0)
            {
                edgeDir = prevEdge(edgeDir);
                distance--;
            }
            return edgeDir;
        }

        public static HexCornerDir nextCorner(HexCornerDir cornerDir)
        {
            return (cornerDir == HexCornerDir.TL) ? HexCornerDir.T : cornerDir + 1;
        }
        public static HexCornerDir prevCorner(HexCornerDir cornerDir)
        {
            return (cornerDir == HexCornerDir.T) ? HexCornerDir.TL : cornerDir - 1;
        }

        public static HexEdgeDir opositeEdge(HexEdgeDir edgeDir)
        {
            return nextEdge(nextEdge(nextEdge(edgeDir)));
        }
        public static HexCornerDir opositeCorner(HexCornerDir cornerDir)
        {
            return nextCorner(nextCorner(nextCorner(cornerDir)));
        }
        public static HexCornerDir firstCornerOfEdge(HexEdgeDir edgeDir)
        {
            return (HexCornerDir)edgeDir;
        }
        public static HexCornerDir secondCornerOfEdge(HexEdgeDir edgeDir)
        {
            return firstCornerOfEdge(nextEdge(edgeDir));
        }

        public static HexEdgeDir prevEdgeOfCorner(HexCornerDir cornerDir)
        {
            return prevEdge((HexEdgeDir)cornerDir);
        }
        public static HexEdgeDir nextEdgeOfCorner(HexCornerDir cornerDir)
        {
            return (HexEdgeDir)cornerDir;
        }

        public static int getCornerClockwiseDistance(HexCornerDir from, HexCornerDir to)
        {
            if (to >= from)
                return to - from;
            return 6 + (to - from);
        }
        public static int getEdgeClockwiseDistance(HexEdgeDir from, HexEdgeDir to)
        {
            if (to >= from)
                return to - from;
            return 6 + (to - from);
        }
    }



    public enum HexEdgeDir
    {
        TR,
        R,
        BR,
        BL,
        L,
        TL
    }
    public enum HexCornerDir
    {
        T,
        TR,
        BR,
        B,
        BL,
        TL
    }
    public enum RiverFlowDirection
    {
        EMPTY,
        IN,
        OUT
    }

}