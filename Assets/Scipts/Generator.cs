using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	private void Update()
	{
		if (m_Generate)
		{
			m_Generate = false;
			Clear(m_Container);
			var l_WorldRange = new Vector4(0, m_WorldSize.x, 0, m_WorldSize.y);
			m_Dimensions[m_DimIndex].Generate(m_Container, l_WorldRange);
		}
	}

	void Clear(Transform a_Container)
	{
		foreach (Transform item in a_Container)
			Destroy(item.gameObject);
	}
}
