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

        public static Student GetStudentById(int id)
        {
            return studentDAO.UzmiStudentaPoID(id);
        }

        public static void AzurirajStudenta(Student s)
        {
            studentDAO.AzurirajStudenta(s);
        }
    }
}
