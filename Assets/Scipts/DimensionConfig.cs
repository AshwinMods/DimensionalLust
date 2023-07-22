using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DimConfig", menuName = "DATA/Dimension Config", order = 0)]
[System.Serializable]
public class DimensionConfig : ScriptableObject
{
	[System.Serializable]
	public class Noise
	{
		public string m_Name;
		public Vector4[] m_TileOffset;
		public Vector4[] m_MulMinMaxAdd;
		public float Evaluate(float x, float y) 
		{
			Vector4 vec;
			float res = 0, val;
			for (int i = 0, iMax = m_TileOffset.Length; i < iMax; i++)
			{
				vec = m_TileOffset[i];
				val = Mathf.PerlinNoise(x * vec.x + vec.z, y * vec.y + vec.w);

				vec = m_MulMinMaxAdd[i];
				val *= vec.x;
				val = Mathf.Clamp(val, vec.y, vec.z);
				// Is Additive or Mul
				if(vec.w == 0) res += val;
				else res += val * vec.w;
			}
			return res;
		}
	}
	
	[System.Serializable]
	public class AssetRef
	{
		public string m_Name;
		public Transform[] m_Variants;
		public Transform GetRandomRef() => m_Variants.Length == 0 ? null : m_Variants[Random.Range(0, m_Variants.Length)];
	}

	[System.Serializable]
	public class Placement
	{
		public string m_Name;
		[Space]
		public int m_AssetNoise;
		[Space]
		public int m_HeightNoise;
		public float m_HeightFactor;
		[Space]
		public AssetRef[] m_Assets;
	}

	[System.Serializable]
	public struct Tile
	{
		public Vector3 m_Height;
		public Vector2Int m_GroundIndex;
		public Vector2Int m_AssetIndex;
	}

	[System.Serializable]
	public class WorldChunk
	{
		public Bounds m_Bounds;
		public Transform m_Container;
		public Tile[] m_Tiles;

		public WorldChunk(Bounds a_Bounds)
		{
			m_Bounds = a_Bounds;
			m_Container = new GameObject("Chunk").transform;
			m_Container.position = m_Bounds.min;
			m_Tiles = new Tile[(int)(m_Bounds.size.x * m_Bounds.size.z)];
		}
	}

	public Noise[] m_Noise;
	public Placement[] m_Placements;
	[Space]
	public Vector4 m_TileConfig;
	public Transform[] m_PlatformPrefabs;

	public WorldChunk Generate(Bounds a_Bounds)
	{
		var l_WChunk = new WorldChunk(a_Bounds);
		var l_Tiles = l_WChunk.m_Tiles;

		int l_Idx;
		float x, z, xMax, zMax, xPos, zPos;
		for (z = 0, zMax = a_Bounds.size.z; z < zMax; z++)
		{
			zPos = a_Bounds.min.z + z;
			for (x = 0, xMax = a_Bounds.size.x; x < xMax; x++)
			{
				xPos = a_Bounds.min.x + x;
				l_Idx = (int)(z * zMax + x);
				for (int w = 0; w < m_Placements.Length; w++)
				{
					var l_P = m_Placements[w];

					float l_HeightNoise = m_Noise[l_P.m_HeightNoise].Evaluate(xPos, zPos);
					l_Tiles[l_Idx].m_Height.y = l_HeightNoise;

					float l_AssetNoise = m_Noise[l_P.m_AssetNoise].Evaluate(xPos, zPos);
					var l_AssLen = l_P.m_Assets.Length;
					if (l_AssLen > 0)
					{
						int l_AssetIndex = (int)Mathf.Clamp(l_AssetNoise * l_AssLen, 0, l_AssLen - 1);

						var l_AssetRef = l_P.m_Assets[l_AssetIndex].GetRandomRef();
						if (l_AssetRef)
						{
							var l_AssetInst = Instantiate(l_AssetRef, l_WChunk.m_Container);
							l_AssetInst.position = new Vector3(xPos, l_HeightNoise * l_P.m_HeightFactor, zPos);
							//l_Tiles[l_Idx].m_AssetIndex.x = l_AssetIndex; l_P.m_Assets;
						}
					}
				}

			}
		}
		return l_WChunk;
		/*
		// Ground
		var m_Platform = m_PlatformPrefabs;
		for (float z = a_Bounds.min.z; z < a_Bounds.max.x; z++)
		{
			for (float x = a_Bounds.min.x; x < a_Bounds.max.x; x++)
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
		*/
	}
}
