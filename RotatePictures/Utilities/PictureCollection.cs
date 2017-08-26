using System.Collections;
using System.Collections.Generic;

namespace RotatePictures.Utilities
{
	public class PictureCollection : IList<string>
	{
		private readonly SynchronizedCollection<string> _picCollection = new SynchronizedCollection<string>();

		public string this[int index]
		{
			get => _picCollection[index];
			set => _picCollection[index] = value;
		}

		public int Count => _picCollection.Count;

		public bool IsReadOnly => false;

		public void Add(string item) => _picCollection.Add(item);

		public void Clear() => _picCollection.Clear();

		public bool Contains(string item) => _picCollection.Contains(item);

		public void CopyTo(string[] array, int arrayIndex) => _picCollection.CopyTo(array, arrayIndex);

		public IEnumerator<string> GetEnumerator() => _picCollection.GetEnumerator();

		public int IndexOf(string item) => _picCollection.IndexOf(item);

		public void Insert(int index, string item) => _picCollection.Insert(index, item);

		public bool Remove(string item) => _picCollection.Remove(item);

		public void RemoveAt(int index) => _picCollection.RemoveAt(index);

		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_picCollection).GetEnumerator();
	}
}
