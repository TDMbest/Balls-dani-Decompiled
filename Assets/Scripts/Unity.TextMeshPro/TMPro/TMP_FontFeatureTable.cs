using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	public class TMP_FontFeatureTable
	{
		[SerializeField]
		internal List<TMP_GlyphPairAdjustmentRecord> m_GlyphPairAdjustmentRecords;

		internal Dictionary<long, TMP_GlyphPairAdjustmentRecord> m_GlyphPairAdjustmentRecordLookupDictionary;

		internal List<TMP_GlyphPairAdjustmentRecord> glyphPairAdjustmentRecords
		{
			get
			{
				return m_GlyphPairAdjustmentRecords;
			}
			set
			{
				m_GlyphPairAdjustmentRecords = value;
			}
		}

		public TMP_FontFeatureTable()
		{
			m_GlyphPairAdjustmentRecords = new List<TMP_GlyphPairAdjustmentRecord>();
			m_GlyphPairAdjustmentRecordLookupDictionary = new Dictionary<long, TMP_GlyphPairAdjustmentRecord>();
		}

		public void SortGlyphPairAdjustmentRecords()
		{
			if (m_GlyphPairAdjustmentRecords.Count > 0)
			{
				m_GlyphPairAdjustmentRecords = Enumerable.ToList(Enumerable.ThenBy(Enumerable.OrderBy(m_GlyphPairAdjustmentRecords, (TMP_GlyphPairAdjustmentRecord s) => s.firstAdjustmentRecord.glyphIndex), (TMP_GlyphPairAdjustmentRecord s) => s.secondAdjustmentRecord.glyphIndex));
			}
		}
	}
}
