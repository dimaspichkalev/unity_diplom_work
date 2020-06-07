using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace ProBuilder.Examples
{
	public class CreatePolyShape
	{

        public static void CreatePortPlane(Vector3[] points, Material ground, Material underground, float height)
		{
            ProBuilderMesh m_Mesh;
            bool m_FlipNormals = false;
            var go = new GameObject("Port Plane");
			m_Mesh = go.gameObject.AddComponent<ProBuilderMesh>();

            m_Mesh.CreateShapeFromPolygon(points, height, m_FlipNormals);
            m_Mesh.SetMaterial(new Face[] {m_Mesh.faces[1]}, ground);

            Face[] otherFaces = new Face[m_Mesh.faceCount - 2];
            for (int i = 2; i < m_Mesh.faceCount; i++)
            {
                otherFaces[i - 2] = m_Mesh.faces[i];
            }
            m_Mesh.SetMaterial(otherFaces, underground);
            m_Mesh.ToMesh();
            m_Mesh.Refresh();
        }
	}
}
