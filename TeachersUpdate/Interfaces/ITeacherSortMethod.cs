using System;
using System.Collections.Generic;
using System.Text;

namespace TeachersUpdate {
  public interface ITeacherSortMethod {

  /// <summary>
  /// Sort the list of teachers by the requested key
  /// </summary>
  /// <param name="Teachers">Unsorted list</param>
  /// <param name="SortKey">Sort Key specifier</param>
  /// <returns>Sorted list</returns>
  public IEnumerable<ITeacher> Sort(IEnumerable<ITeacher> Teachers,ITeacherSort SortKey);

  }
}
