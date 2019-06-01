using UnityEngine;

public class MeshVertex
{
    public MeshVertex(Vector3 position)
    {
        m_position = position;
    }
    public MeshVertex(Vector3 position, Color color)
    {
        m_position = position;
        m_color = color;
    }
    public MeshVertex(Vector3 position, Vector2 uv0)
    {
        m_position = position;
        m_uv0 = uv0;
    }
    public MeshVertex(Vector3 position, Color color, Vector2 uv0)
    {
        m_position = position;
        m_color = color;
        m_uv0 = uv0;
    }

    public Vector3 m_position = new Vector3();
    public Color m_color = new Color();
    public Vector2 m_uv0 = new Vector2();
}
