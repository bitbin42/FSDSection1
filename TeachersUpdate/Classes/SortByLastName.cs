using System;
using System.Collections.Generic;
using System.Text;

namespace TeachersUpdate {
  class SortByLastName : ITeacherSort {

  /// <inheritdoc />
  public bool IsInOrder(ITeacher Teacher1,ITeacher Teacher2) {
  return (string.Compare(Teacher1.FullName(true),Teacher2.FullName(true))<0);
  }

  }
}
