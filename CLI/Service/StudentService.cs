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
    }
}
