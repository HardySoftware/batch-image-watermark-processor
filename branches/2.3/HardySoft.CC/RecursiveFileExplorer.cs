using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;

namespace HardySoft.CC.File {
	/// <summary>
	/// This class provides a simple means of returning a collection of files present 
	/// in a directory, with the option of having the search recurse down all the 
	/// directories sub-directories. 
	/// </summary>
	public class RecursiveFileExplorer {
		/// <summary>
		/// File collection
		/// </summary>
		private FileDataCollection fileList = new FileDataCollection();
		/// <summary>
		/// Wildcard to match file name
		/// </summary>
		private string wildcard = "";
		/// <summary>
		/// Initial path to search
		/// </summary>
		private string path = "";
		/// <summary>
		/// Whether to use recursive to search
		/// </summary>
		private bool Recursive = true;

		/// <overloads>The contructor has 4 overloads.</overloads>
		/// <summary>
		/// Constructor called if path argument given, default to not recursive.
		/// </summary>
		/// <param name="path">Initial path to search from.</param>
		public RecursiveFileExplorer(string path) {
			this.path = path;
			this.Recursive = false;
			//this.fileList = this.GetFiles();
		}

		/// <summary>
		/// Constructor called if path and wildcard arguments given, default to not recursive.
		/// </summary>
		/// <param name="path">Initial path to search from.</param>
		/// <param name="wildcard">Wildcard to filter results by.</param>
		public RecursiveFileExplorer(string path, string wildcard) {
			this.path = path;
			this.wildcard = wildcard;
			this.Recursive = false;
			//this.fileList = this.GetFiles();
		}

		/// <summary>
		/// Constructor called if file path and isRecursive arguments given.
		/// </summary>
		/// <param name="path">Initial path to search from.</param>
		/// <param name="isRecursive">Specifies whether to use recursion to search sub-folders.</param>
		public RecursiveFileExplorer(string path, bool isRecursive) {
			this.path = path;
			this.Recursive = isRecursive;
			//this.fileList = this.GetFiles();
		}

		/// <summary>
		/// Constructor called if filepath, wildcard and isRecursive arguments given.
		/// </summary>
		/// <param name="path">Initial path to search from.</param>
		/// <param name="wildcard">Wildcard to filter results by.</param>
		/// <param name="isRecursive">Specifies whether to use recursion to search sub-folders.</param>
		public RecursiveFileExplorer(string path, string wildcard, bool isRecursive) {
			this.path = path;
			this.wildcard = wildcard;
			this.Recursive = isRecursive;
			//this.fileList = this.GetFiles();
		}

		/// <summary>
		/// Get the FileDataCollection which contains all the files found.
		/// </summary>
		public FileDataCollection FileList {
			get {
				return fileList;
			}
		}

		/// <summary>
		/// Searches through directory and sub-directories for files.
		/// </summary>
		/// <returns>ArrayList of files found.</returns>
		public FileDataCollection GetFiles() {
			DirectoryInfo dir = new DirectoryInfo(this.path);
			FileInfo[] files;
			DirectoryInfo[] dirs;
			FileDataCollection List = new FileDataCollection();

			//if the source dir doesn't exist, throw
			if (!dir.Exists) {
				throw new Exception("Source directory doesn't exist: " + this.path);
			}

			if (this.wildcard.Length == 0) {
				//get all files in the current dir
				files = dir.GetFiles();
			} else {
				//get files maching pattern in the current dir
				files = dir.GetFiles(wildcard);
			}

			//loop through each file
			foreach (FileInfo file in files) {
				//if(this.wildcard.Length == 0) {
				//	List.Add(new FileInfo(file.FullName, file.Name, file.Extension, file.Length));
				//} else {
				//	if(this.checkFile(file.Name)) {
				List.Add(file);
				//	}
				//}
			}

			//cleanup
			files = null;

			//if not isRecursive, all work is done
			if (!this.Recursive) {
				return List;
			}

			//otherwise, get dirs
			dirs = dir.GetDirectories();

			//loop through each sub directory in the current dir
			foreach (DirectoryInfo subdir in dirs) {
				this.path = subdir.FullName;
				FileDataCollection temp = new FileDataCollection();
				temp = GetFiles();
				for (int i = 0; i < temp.Count; i++) {
					FileInfo TempData = (FileInfo)temp[i];
					List.Add(TempData);
				}
			}

			//cleanup
			dirs = null;
			dir = null;

			return List;
		}
	}

	/// <summary>
	/// This class holds colletion of each individual file.
	/// </summary>
	public class FileDataCollection : IEnumerable, IEnumerator {
		/// <summary>
		/// File collection
		/// </summary>
		private ArrayList datas = new ArrayList();
		/// <summary>
		/// Current position of the list.
		/// </summary>
		private int position = -1;

		/// <summary>
		/// Adds a FileInfo object to FileDataCollection.
		/// </summary>
		/// <param name="data">The FileInfo to add to the end of the FileDataCollection. </param>
		/// <returns>The zero-based index at which the new element is inserted.</returns>
		public int Add(FileInfo data) {
			return datas.Add(data);
		}

		/// <summary>
		/// Removes the first occurrence of a specific FileInfo object from the FileDataCollection.
		/// </summary>
		/// <param name="data">The FileInfo object to remove from the FileDataCollection.</param>
		public void Remove(FileInfo data) {
			datas.Remove(data);
		}

		/// <summary>
		/// Gets the number of FileInfo objects contained in the FileDataCollection.
		/// </summary>
		public int Count {
			get {
				return datas.Count;
			}
		}

		/// <summary>
		/// Gets the FileInfo at the specified index.
		/// </summary>
		public FileInfo this[int count] {
			get {
				if (count > datas.Count) {
					return null;
				} else {
					return (FileInfo)datas[count];
				}
			}
		}

		/// <summary>
		/// Sort the result list by file creation date ascending.
		/// </summary>
		public void SortByCreationDate() {
			if (datas.Count > 0) {
				datas.Sort(new SortByCreationDate());
			}
		}

		/// <summary>
		/// Sort result list by file name ascending.
		/// </summary>
		public void SortByFileName() {
			if (datas.Count > 0) {
				datas.Sort(new SortByFileName());
			}
		}

		/// <summary>
		/// Sort result list by file name descending.
		/// </summary>
		public void SortByFileNameDesc() {
			if (datas.Count > 0) {
				datas.Sort(new SortByFileNameDesc());
			}
		}

		/// <summary>
		/// Returns an IEnumerator that can iterate through the FileDataCollection instance.
		/// </summary>
		/// <returns>An IEnumerator for the FileDataCollection.</returns>
		public IEnumerator GetEnumerator() {
			return (IEnumerator)this;
		}

		/// <summary>
		/// Advances the enumerator to the next element of the collection.
		/// </summary>
		/// <returns><b>true</b> if the index is successfully incremented and within the enumerated string; otherwise, <b>false</b>.</returns>
		public bool MoveNext() {
			if (position < datas.Count - 1) {
				position++;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Initializes the index to a position logically before the first FileInfo element of the enumerated collection.
		/// </summary>
		public void Reset() {
			position = -1;
		}

		/// <summary>
		/// Gets the FileInfo object in the enumerated collection currently indexed by this instance.
		/// </summary>
		public object Current {
			get {
				return datas[position];
			}
		}
	}

	/// <summary>
	/// Compares two files for equivalence by comparing the creation date.
	/// </summary>
	internal class SortByCreationDate : IComparer {
		int IComparer.Compare(object x, object y) {
			FileInfo a = (FileInfo)x;
			FileInfo b = (FileInfo)y;
			if (a.CreationTime.Ticks > b.CreationTime.Ticks) {
				return 1;
			} else if (a.CreationTime.Ticks < b.CreationTime.Ticks) {
				return -1;
			} else {
				return 0;
			}
		}
	}

	/// <summary>
	/// Compares two files for equivalence in ascending order by comparing the file name, ignoring the case of names.
	/// </summary>
	internal class SortByFileName : IComparer {
		int IComparer.Compare(object x, object y) {
			return (new CaseInsensitiveComparer()).Compare(((FileInfo)x).FullName, ((FileInfo)y).FullName);
		}
	}

	/// <summary>
	/// Compares two files for equivalence in descending order by comparing the file name, ignoring the case of names.
	/// </summary>
	internal class SortByFileNameDesc : IComparer {
		int IComparer.Compare(object x, object y) {
			return (new CaseInsensitiveComparer()).Compare(((FileInfo)y).FullName, ((FileInfo)x).FullName);
		}
	}
}