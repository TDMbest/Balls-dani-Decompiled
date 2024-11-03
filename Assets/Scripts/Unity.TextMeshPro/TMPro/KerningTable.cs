using System;
using System.Collections.Generic;
using System.Linq;

namespace TMPro
{
	[Serializable]
	public class KerningTable
	{
		public List<KerningPair> kerningPairs;

		public KerningTable()
		{
			kerningPairs = new List<KerningPair>();
		}

		public void AddKerningPair()
		{
			if (kerningPairs.Count == 0)
			{
				kerningPairs.Add(new KerningPair(0u, 0u, 0f));
				return;
			}
			uint firstGlyph = Enumerable.Last(kerningPairs).firstGlyph;
			uint secondGlyph = Enumerable.Last(kerningPairs).secondGlyph;
			float xOffset = Enumerable.Last(kerningPairs).xOffset;
			kerningPairs.Add(new KerningPair(firstGlyph, secondGlyph, xOffset));
		}

		public int AddKerningPair(uint first, uint second, float offset)
		{
			if (kerningPairs.FindIndex((KerningPair item) => item.firstGlyph == first && item.secondGlyph == second) == -1)
			{
				kerningPairs.Add(new KerningPair(first, second, offset));
				return 0;
			}
			return -1;
		}

		public int AddGlyphPairAdjustmentRecord(uint first, GlyphValueRecord_Legacy firstAdjustments, uint second, GlyphValueRecord_Legacy secondAdjustments)
		{
			if (kerningPairs.FindIndex((KerningPair item) => item.firstGlyph == first && item.secondGlyph == second) == -1)
			{
				kerningPairs.Add(new KerningPair(first, firstAdjustments, second, secondAdjustments));
				return 0;
			}
			return -1;
		}

		public void RemoveKerningPair(int left, int right)
		{
			int num = kerningPairs.FindIndex((KerningPair item) => item.firstGlyph == left && item.secondGlyph == right);
			if (num != -1)
			{
				kerningPairs.RemoveAt(num);
			}
		}

		public void RemoveKerningPair(int index)
		{
			kerningPairs.RemoveAt(index);
		}

		public void SortKerningPairs()
		{
			if (kerningPairs.Count > 0)
			{
				kerningPairs = Enumerable.ToList(Enumerable.ThenBy(Enumerable.OrderBy(kerningPairs, (KerningPair s) => s.firstGlyph), (KerningPair s) => s.secondGlyph));
			}
		}
	}
}
