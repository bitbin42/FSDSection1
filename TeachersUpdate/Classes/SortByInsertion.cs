using System;
using System.Collections.Generic;
using System.Text;

namespace TeachersUpdate {
  public class SortByInsertion : ITeacherSortMethod {

  /// <inheritdoc />
  public IEnumerable<ITeacher> Sort(IEnumerable<ITeacher> Teachers,ITeacherSort SortKey) {
  List<ITeacher> cSorted=new List<ITeacher>();
  foreach (ITeacher oTeacher1 in Teachers) {
    Int32 p=0;
    while (p>=0 && p<cSorted.Count) {
      if (SortKey.IsInOrder(oTeacher1,cSorted[p])) {
        cSorted.Insert(p,oTeacher1); // we found the place where this one goes
        p=-1;
        }
      else
        p++;
      }
    if (p==cSorted.Count)
      cSorted.Add(oTeacher1); // this one goes to the end
    }
  return cSorted;
  }

  }
}
