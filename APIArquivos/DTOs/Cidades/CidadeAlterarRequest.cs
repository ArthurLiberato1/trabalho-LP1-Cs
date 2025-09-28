namespace APIArquivos.DTOs.Cidades
{
    public class CidadeAlterarRequest
    {
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public int IBGEMunicipio { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
