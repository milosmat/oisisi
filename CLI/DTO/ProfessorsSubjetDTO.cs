using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.DTO
{
    public class ProfessorsSubjectDTO
    {
        public string FullName { get; set; }
        public string SubjectName { get; set; }

        public ProfessorsSubjectDTO(string firstName, string lastName, string subjectName)
        {
            FullName = $"{firstName} {lastName}";
            SubjectName = subjectName;
        }
    }
}
