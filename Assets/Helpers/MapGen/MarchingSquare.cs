using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawnfall.Helper.MapGen
{
    public class MarchingSquare
    {
        public readonly MSNode TL, TR, BL, BR;
        public readonly NodeBase CT, CB, CL, CR;

        public MarchingSquare(MSNode tl, MSNode tr, MSNode bl, MSNode br)
        {
            TL = tl;
            TR = tr;
            BL = bl;
            BR = br;
            CT = tl.right;
            CB = bl.right;
            CL = bl.top;
            CR = br.top;
        }

        public int GetConfiguration()
        {
            return
                ((TL.m_isWall) ? 1 : 0) * 1 +
                ((TR.m_isWall) ? 1 : 0) * 2 +
                ((BR.m_isWall) ? 1 : 0) * 4 +
                ((BL.m_isWall) ? 1 : 0) * 8
                ;
        }
    }


    public class MSNode : NodeBase
    {
        public readonly NodeBase right, top;
        public readonly bool m_isWall;

        public MSNode(Vector3 pos, bool isWall, float nodeSize) : base(pos)
        {
            m_isWall = isWall;
            right = new NodeBase(m_position + Vector3.right * nodeSize / 2.0f);
            top = new NodeBase(m_position + Vector3.up * nodeSize / 2.0f);
        }
    }
}