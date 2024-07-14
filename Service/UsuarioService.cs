using ms_forum.Domains;
using ms_forum.Interface;

namespace ms_forum.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly HttpClient _httpClient;
        public UsuarioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UsuarioResponse> GetUsuarioByIdAsync(long id)
        {
            var response = await _httpClient.GetAsync($"/api/usuario/selecionar-usuario/{id}");

            if (response.IsSuccessStatusCode)
            {
                var usuario = await response.Content.ReadFromJsonAsync<UsuarioResponse>();
                return usuario;
            }

            // Trate os erros conforme necessário
            return null;
        }

        public class UsuarioResponse : Entity
        {
            public string Nome { get; set; }
            public string Email { get; set; }
            public int TipoUsuario { get; set; }
            public virtual IEnumerable<AreaInteresse> UsuarioAreaInteresses { get; set; }
            public virtual IEnumerable<Conquistas> UsuarioConquistas { get; set; }
            public long ComentarioForum { get; set; }
            public long TopicoForum { get; set; }
            public long ComentarioAula { get; set; }
            public long CurtirAula { get; set; }
            public long NoticiaVisualizada { get; set; }
            public long? PerfilId { get; set; }
            public long? SociedadeId { get; set; }
            public DateTime? DataNascimento { get; set; }
            public byte[]? Foto { get; set; }
            public string? Hobbie { get; set; }
        }

        public class AreaInteresse : Entity
        {
            public string Nome { get; set; }
        }

        public class Conquistas : Entity
        {
            public string Nome { get; set; }
        }
    }
}
