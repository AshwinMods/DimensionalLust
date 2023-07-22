using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DimConfig", menuName = "DATA/Dimension Config", order = 0)]
[System.Serializable]
public class DimensionConfig : ScriptableObject
{
	public Vector4 m_TileConfig;
	public Transform[] m_PlatformPrefabs;

	public void Generate(Transform m_Container, Vector4 a_WorldRange)
	{
		// Ground
		var m_Platform = m_PlatformPrefabs;
		for (float z = a_WorldRange.x; z < a_WorldRange.y; z++)
		{
			for (float x = a_WorldRange.z; x < a_WorldRange.w; x++)
			{
				var l_U = x * m_TileConfig.x + m_TileConfig.z;
				var l_V = z * m_TileConfig.y + m_TileConfig.w;
				var l_Noise = Mathf.PerlinNoise(l_U, l_V);
				var l_Index = Mathf.Clamp((int)(m_Platform.Length * l_Noise), 0, m_Platform.Length - 1);// Random.Range(0, (int)(m_Ground.Length * l_Noise));
				if (!m_Platform[l_Index]) continue; // Empty
				var l_Tile = Instantiate(m_Platform[l_Index], m_Container);
				l_Tile.position = new Vector3(x, 0, z);
			}
		}
	}
}
