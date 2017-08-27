using System;
using System.Collections.Generic;
using System.Linq;


namespace RotatePictures.Utilities
{
	public static class LinqExtensions
	{
		public static IEnumerable<T> Except<T>(this IEnumerable<T> a, IEnumerable<T> b, Func<T, T, bool> cmpr, Func<T, int> hashCode = null) => a.Except(b, new LinqComparer<T>(cmpr, hashCode));

		public static IEnumerable<T> Intersect<T>(this IEnumerable<T> a, IEnumerable<T> b, Func<T, T, bool> comparer, Func<T, int> hashCode = null) => a.Intersect(b, new LinqComparer<T>(comparer, hashCode));

		public static IEnumerable<T> Union<T>(this IEnumerable<T> a, IEnumerable<T> b, Func<T, T, bool> cmpr, Func<T, int> hashCode = null) => a.Union(b, new LinqComparer<T>(cmpr, hashCode));
	}
}
