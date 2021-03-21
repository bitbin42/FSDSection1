using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersUpdate {

  /// <inheritdoc />
  public class Faculty : IFaculty {

    /// <inheritdoc />
    public Exception LastException {get; private set;}

    /// <summary>
    /// Last used ID (this should be pulled from a separate store to avoid re-using IDs)
    /// </summary>
    private Int32 mlMaxID=0;

    /// <summary>
    /// Reference to the global storage object
    /// </summary>
    private IPersist moStorage=null;

    /// <summary>
    /// keep track of changes
    /// </summary>
    private bool mbDirty=false;

    /// <summary>
    /// save the list of teachers we know about
    /// </summary>
    private Dictionary<Int32,ITeacher> mcTeachers=new Dictionary<Int32, ITeacher>();

    /// <summary>
    /// save the list of teacher names we know about
    /// </summary>
    private Dictionary<string,Int32> mcTeacherNames=new Dictionary<string, Int32>();

    /// <summary>
    /// Initialize faculty list
    /// </summary>
    /// <param name="Storage">Persistent storage object</param>
    public Faculty(IPersist Storage) {
    moStorage=Storage;
    IEnumerable<ITeacher> cTeachers=moStorage.LoadTeachers();
    if (cTeachers==null || moStorage.LastException!=null)
      SetException(moStorage.LastException.Message);
    else {
      mlMaxID=0;
      foreach (ITeacher oTeacher in cTeachers) {
        RememberTeacher(oTeacher);
        }
      }
    mbDirty=false;
    }

    /// <summary>
    /// Determine if the data has changed
    /// </summary>
    public bool IsDirty {
    get {
      bool bDirty=mbDirty;
      foreach (ITeacher oTeacher in mcTeachers.Values)
        bDirty=bDirty || oTeacher.IsDirty;
      return bDirty;
      }
    }

    /// <inheritdoc />
    public ITeacher FindTeacher(string FirstName, string LastName) {
    string sName=(LastName+","+FirstName).ToLower();
    return mcTeacherNames.ContainsKey(sName)?mcTeachers[mcTeacherNames[sName]]:null;
    }

    /// <inheritdoc />
    public ITeacher FindTeacher(Int32 ID) {
    return mcTeachers.ContainsKey(ID)?mcTeachers[ID]:null;
    }

    /// <inheritdoc />
    public bool AddTeacher(string FirstName, string LastName, string Department) {
    bool bOK=false;
    ClearException();
    FirstName=FirstName.Trim();
    LastName=LastName.Trim();
    Department=Department.Trim();
    if (FirstName.Length>0 && LastName.Length>0 && Department.Length>0) {
      Int32 lID=mlMaxID+1;
      ITeacher oTeacher=new Teacher(lID,FirstName,LastName,Department);
      if (RememberTeacher(oTeacher)) {
        bOK=true;
        mlMaxID=lID;
        mbDirty=true;
        }
      }
    else
      SetException("First name, Last name, and Department are all required");
    return bOK;
    }

    /// <inheritdoc />
    public bool UpdateTeacher(Int32 ID,string FirstName, string LastName, string Department) {
    bool bOK=false;
    ClearException();
    FirstName=FirstName.Trim();
    LastName=LastName.Trim();
    Department=Department.Trim();
    ITeacher oTeacher=FindTeacher(ID);
    if (oTeacher==null)
      SetException($"Teacher ID {ID} not found");
    else if (oTeacher.FirstName.Equals(FirstName) && oTeacher.LastName.Equals(LastName) && oTeacher.Department.Equals(Department)) {
      // no action needed if no change was made
      bOK=true;
      }
    else if (FirstName.Length>0 && LastName.Length>0 && Department.Length>0) {
      ITeacher oNew=FindTeacher(FirstName,LastName);
      if (oNew==null) {
        mcTeacherNames.Remove(oTeacher.FullName(true).ToLower());
        mcTeachers.Remove(ID);
        oNew=new Teacher(ID,FirstName,LastName,Department);
        bOK=RememberTeacher(oNew);
        }
      else
        SetException($"There is already a teacher with the name {FirstName} {LastName}");
      }
    else
      SetException("First name, Last name, and Department are all required");
    return bOK;
    }

    /// <summary>
    /// Add this teacher to the internal list
    /// </summary>
    /// <param name="Teacher">Teacher object</param>
    /// <returns>True if added</returns>
    private bool RememberTeacher(ITeacher Teacher) {
    bool bOK=false;
    string sName=Teacher.FullName(true).ToLower();
    if (mcTeacherNames.ContainsKey(sName) || mcTeachers.ContainsKey(Teacher.ID)) {
      string sError=$"Duplicate teacher found: {Teacher.ID} {Teacher.FullName()}";
      SetException(sName,LastException);
      }
    else {
      mcTeachers.Add(Teacher.ID,Teacher);
      mcTeacherNames.Add(sName,Teacher.ID);
      if (Teacher.ID>mlMaxID)
        mlMaxID=Teacher.ID;
      bOK=true;
      }
    return bOK;
    }

    /// <inheritdoc />
    public bool RemoveTeacher(ITeacher Teacher) {
    bool bOK=false;
    ClearException();
    if (mcTeachers.ContainsKey(Teacher.ID)) {
      mcTeachers.Remove(Teacher.ID);
      mcTeacherNames.Remove(Teacher.FullName(true).ToLower());
      mbDirty=true;
      bOK=true;
      }
    else
      SetException("Teacher not found");
    return bOK;
    }

    /// <inheritdoc />
    public bool Save() {
    bool bOK=false;
    ClearException();
    if (mbDirty) {
      if (moStorage.SaveTeachers(mcTeachers.Values)) {
        mbDirty=false;
        foreach (ITeacher oTeacher in mcTeachers.Values)
          oTeacher.IsDirty=false;
        bOK=true;
        }
      else
        SetException("Unable to save teachers data",moStorage.LastException);
      }
    else
      bOK=true; // no need to save
    return bOK;
    }

    /// <inheritdoc />
    public IEnumerable<ITeacher> Teachers {
    get {
      return mcTeachers.Values;
      }
    }

    /// <inheritdoc />
    public IEnumerable<string> Departments {
    get {
      Dictionary<string,string> cDepts=new Dictionary<string, string>();
      foreach (ITeacher oTeacher in mcTeachers.Values) {
        string sKey=oTeacher.Department.ToLower();
        if (!cDepts.ContainsKey(sKey))
          cDepts.Add(sKey,oTeacher.Department);
        }
      return cDepts.Values;
      }
    }

    /// <inheritdoc />
    public bool SortTeachers(IFaculty.SortOptions SortOrder) {
    bool bOK=true;
    ITeacherSort oSort=null;
    ClearException();
    switch (SortOrder) {
      case IFaculty.SortOptions.ID:
        oSort=new SortByID();
        break;
      case IFaculty.SortOptions.LastName:
        oSort=new SortByLastName();
        break;
      case IFaculty.SortOptions.FirstName:
        oSort=new SortByFirstName();
        break;
    }
  if (oSort==null) {
    SetException("Invalid sort order specified");
    bOK=false;
    }
  else
    SortTeacherList(oSort);
  return bOK;
  }

  /// <summary>
  /// Run an insertion sort to rebuild the list in order (this does not  count as a change in the data for saving)
  /// </summary>
  /// <param name="SortOption">ITeacherSort object</param>
  public void SortTeacherList(ITeacherSort SortOption) {
  List<ITeacher> cNew=new List<ITeacher>();
  foreach (ITeacher  oTeacher1 in mcTeachers.Values) {
    Int32 p=0;
    while (p>=0 && p<cNew.Count) {
      if (SortOption.IsInOrder(oTeacher1,cNew[p])) {
        cNew.Insert(p,oTeacher1); // we found the place where this one goes
        p=-1;
        }
      else
        p++;
      }
    if (p==cNew.Count)
      cNew.Add(oTeacher1); // this one goes to the end
    }
  // now rebuild the list
  mcTeachers=new Dictionary<int, ITeacher>();
  foreach (ITeacher oTeacher in cNew)
    mcTeachers.Add(oTeacher.ID,oTeacher);
  }

  /// <summary>
  /// Lose any existing exception information
  /// </summary>
  private void ClearException() {
  LastException=null;
  }

  /// <summary>
  /// Save the error information for the caller
  /// </summary>
  /// <param name="NewException">Error message</param>
  /// <param name="InnerException">Secondary/older Exception to preserve</param>
  private void SetException(string NewException,Exception InnerException=null) {
  LastException=new Exception(NewException,InnerException);
  }


  }

}
