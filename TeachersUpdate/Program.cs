using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersUpdate {
  class Program {

  /// <summary>
  /// Main application start
  /// </summary>
  /// <param name="args">Command-line arguments</param>
  static void Main(string[] args) {
  using (IMainApp oMain = new ConsoleTeachersUpdate()) {
    oMain.DoIt();
    }
  }


  }
}
