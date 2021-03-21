using System;
using System.Collections.Generic;
using System.Text;

namespace TeachersUpdate {
  class SortByFirstName : ITeacherSort {

  /// <inheritdoc />
  public bool IsInOrder(ITeacher Teacher1,ITeacher Teacher2) {
  return (string.Compare(Teacher1.FullName(false),Teacher2.FullName(false))<=0);
  }

  }
}
