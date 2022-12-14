using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawnfall.Helper
{
    public class NodeBase
    {
        public int Index = -1;
        public Vector3 m_position;

        public NodeBase(Vector3 pos)
        {
            m_position = pos;
        }
    }
}