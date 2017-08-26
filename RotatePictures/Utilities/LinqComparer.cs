using System;
using System.Collections.Generic;


namespace RotatePictures.Utilities
{
	public class LinqComparer<TSource> : IEqualityComparer<TSource>
	{
		private readonly Func<TSource, TSource, bool> _linqCmp;
		private readonly Func<TSource, int> _hashCode;

		public LinqComparer(Func<TSource, TSource, bool> cmp, Func<TSource, int> hashCode = null)
		{
			_linqCmp = cmp ?? throw new ArgumentException(@"comparison function must be a non-null function", nameof(cmp));
			_hashCode = hashCode ?? (T => 0);
		}

		/// <summary>
		/// Determines whether the specified objects are equal.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool Equals(TSource x, TSource y) => _linqCmp(x, y);

		/// <summary>
		/// Returns a hash code for the specified object.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public int GetHashCode(TSource x) => _hashCode(x);
	}
}
