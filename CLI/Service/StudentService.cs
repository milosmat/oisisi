using System.Collections.Generic;
using CLI.DAO;
using StudentskaSluzba.Model;

namespace CLI.Service
{
    public class StudentService
    {
        private static readonly StudentDAO studentDAO = new StudentDAO();

        public static List<Student> GetStudents()
        {
            return studentDAO.UzmiSveStudente();
        }
        public static bool DeleteStudent(int studentId)
        {
            var student = studentDAO.UzmiStudentaPoID(studentId);
            if (student == null) return false;
            return studentDAO.IzbrisiStudenta(studentId) != null;
        }

        public static Student GetStudentById(int id)
        {
            return studentDAO.UzmiStudentaPoID(id);
        }

        public static Student AzurirajStudenta(Student s)
        {
            return studentDAO.AzurirajStudenta(s);
        }

    }
}
