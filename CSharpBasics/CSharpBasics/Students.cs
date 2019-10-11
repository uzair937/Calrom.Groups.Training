using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBasics
{
    public enum Subject
    {
        Maths,

        English,

        Science,

        IT,
    }
    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public int StudentNumber { get; set; }
        public Subject Subject { get; set; }
    }

    public class AllStudents
    {
       public List<Student> GetStudentsWithMaths()
        {
            var returnList = new List<Student>();
            returnList.Where(s => s.Subject == Subject.Maths);
            return returnList;
        }

        public List<Student> GetStudentsWithEnglish()
        {
            var returnList = new List<Student>();
            return returnList;

        }

        public List<Student> GetStudentsWithScience()
        {
            var returnList = new List<Student>();
            return returnList;
        }

        public List<Student> GetStudentsWithIT()
        {
            var returnList = new List<Student>();
            return returnList;

        }
    }
    public class AccessRecord
    {
        public Student MyAccessing()
        {
            Student students = new Student();
            students.Name = "Jason";
            students.Age = 45;
            students.DOB = new DateTime(1974, 03, 26);
            students.Address = "145 Avenue";
            students.StudentNumber = 16045854;
            return students;
        }

    }

}
