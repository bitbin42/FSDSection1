using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io=System.IO;

namespace TeachersUpdate {

    /// <inheritdoc />
    public class FileStorage : IPersist, IDisposable {

    /// <inheritdoc />
    public Exception LastException {get; private set;}

    /// <summary>
    /// Clean up
    /// </summary>
    public void Dispose() {
    // not needed (yet)
    }

    /// <inheritdoc />
    public IEnumerable<ITeacher> LoadTeachers() {
    Dictionary<string,ITeacher> cTeachers=null;
    LastException=null;
    if (io.File.Exists("DBTeachers.txt"))
      cTeachers=LoadTeachersFromFile();
    else
      cTeachers= new Dictionary<string, ITeacher>();
    return cTeachers.Values;
    }

    /// <inheritdoc />
    public bool SaveTeachers(IEnumerable<ITeacher> Teachers) {
    LastException=null;
    return SaveTeachersToFile(Teachers);
    }

  /// <summary>
  /// Read last known teacher list
  /// </summary>
  private Dictionary<string,ITeacher> LoadTeachersFromFile() {
  Dictionary<string,ITeacher> cTeachers=new Dictionary<string, ITeacher>();
  using (io.StreamReader oSaved = new io.StreamReader("DBTeachers.txt")) {
    string sLine=""; // expect first\tlast\tdept
    while (LastException==null && (sLine=oSaved.ReadLine())!=null) {
      string[] sParts=sLine.Split('|');
      if (sParts==null || sParts.Length!=4)
        LastException=new Exception($"Warning: Unable to parse teacher record {sLine}");
      else {
        Int32 lID=Int32.Parse(sParts[0]); // need error handling around this (actually all of this)
        string sFirst=sParts[1];
        string sLast=sParts[2];
        string sDept=sParts[3];
        if (lID<1 || sFirst.Length==0 || sLast.Length==0 || sDept.Length==0) 
          LastException=new Exception($"Warning: Missing fields on teacher record {sLine}");
        else {
          ITeacher oTeacher=new Teacher(lID,sFirst,sLast,sDept); // add the teacher
          string sKey=(oTeacher.LastName+","+oTeacher.FirstName).ToLower();
          if (cTeachers.ContainsKey(sKey))
            LastException=new Exception($"Warning: duplicate teacher record ignored {sLine}");
          else {
            cTeachers.Add(sKey,oTeacher);
            }
          }
        }
      }
    oSaved.Close();
    }
  return cTeachers;
  }


  /// <summary>
  /// Save the current info
  /// </summary>
  private bool SaveTeachersToFile(IEnumerable<ITeacher> Teachers) {
  bool bOK=false;
  string sState="";
  try {
    sState="Removing old temp files";
    DeleteFiles("DBTeachers.tmp"); // clean out temp files
    sState="Creating and writing files";
    using (io.TextWriter fTeachers=io.File.CreateText("DBTeachers.tmp")) {
      foreach (ITeacher oTeacher in Teachers) { 
        fTeachers.WriteLine($"{oTeacher.ID}|{oTeacher.FirstName}|{oTeacher.LastName}|{oTeacher.Department}");
        }
      fTeachers.Close();
      }
    sState="Creating backup";
    DeleteFiles("DBTeachers.bkp"); 
    if (io.File.Exists("DBTeachers.txt"))
      io.File.Move("DBTeachers.txt","DBTeachers.bkp");
    sState="Updating text";
    io.File.Move("DBTeachers.tmp","DBTeachers.txt");
    bOK=true;
    }
  catch (Exception ex) {
    LastException=new Exception("Save teachers data failed at step: "+sState,ex);
    }
  return bOK;
  }

  /// <summary>
  /// Delete one or more files
  /// </summary>
  /// <param name="FilePaths">List of files to delete</param>
  private static void DeleteFiles(params string[] FilePaths) {
  foreach (string sPath in FilePaths) {
    if (io.File.Exists(sPath))
      io.File.Delete(sPath);
    }
  }

  }

}
