using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BassoLegnami.Extensions
{
	public static class ArrayExtensions
	{
		public static int[] IndicesOf(this int[] array, int value)
		{
			int index = 0;
			List<int> indexpositions = new();
			foreach (int val in array)
			{
				if (val == value)
				{
					indexpositions.Add(index);
				}
				index++;
			}
			return indexpositions.ToArray();
		}
	}
}
