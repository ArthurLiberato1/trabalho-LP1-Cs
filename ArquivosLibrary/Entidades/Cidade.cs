using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquivosLibrary.Entidades
{
    public class Cidade
    {
        public int CidadeId { get; set; }
        public string Nome {  get; set; }
        public string Sigla { get; set; }
        public int IBGEMunicipio {  get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }
}
