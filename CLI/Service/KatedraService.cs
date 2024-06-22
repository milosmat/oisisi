using CLI.DAO;
using StudentskaSluzba.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Service
{
    public static class KatedraService
    {
        private static readonly KatedraDAO _katedraDao = new KatedraDAO();
        public static List<Katedra> GetKatedre()
        {
            return _katedraDao.UzmiSveKatedre();
        }
    }
}
