

using UnityEngine;

namespace LethalMusic.Utilities {
	public static class TransformExtensions {

		/// <summary>
		/// Check if the transform is a child of another transform (recursively)
		/// </summary>
		/// <param name="child"></param>
		/// <param name="parent"></param>
		/// <returns></returns>
		public static bool IsRecursiveChildOf(this Transform child, Transform parent) {
			if (child == parent) return true;
			if (child.parent == null) return false;
			return child.parent.IsRecursiveChildOf(parent);
		}

		/// <summary>
		/// Check if the transform contains the child (recursively)
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="child"></param>
		/// <returns>Whether the transform contains the child</returns>
		public static bool ContainsChild(this Transform parent, Transform child) {
			foreach (Transform t in parent) {
				if (t == child) return true;
				if (t.childCount > 0 && t.ContainsChild(child)) return true;
			}
			return false;
		}
	}
}