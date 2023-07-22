using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DimensionConfig;

public class Generator : MonoBehaviour
{
	[Header("Reference")]
	[SerializeField] DimensionConfig[] m_Dimensions;
	[SerializeField] Transform m_Container;
	[Header("Config")]
	[SerializeField] Vector2Int m_WorldSize;
	[Header("Editor")]
	[SerializeField] int m_DimIndex;
	[SerializeField] bool m_Generate;

	[SerializeField] WorldChunk m_Chunk;
	private void Update()
	{
		if (m_Generate)
		{
			m_Generate = false;
			if(m_Chunk != null && m_Chunk.m_Container) Clear(m_Chunk.m_Container);

			Bounds l_ChunkBound = new Bounds();
			l_ChunkBound.min = new Vector3(0, 0, 0);
			l_ChunkBound.max = new Vector3(m_WorldSize.x, 0, m_WorldSize.y);
			m_Chunk = m_Dimensions[m_DimIndex].Generate(l_ChunkBound);
		}
	}

	void Clear(Transform a_Container)
	{
		foreach (Transform item in a_Container)
			Destroy(item.gameObject);
	}
}
