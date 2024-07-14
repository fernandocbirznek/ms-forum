using static ms_forum.Service.UsuarioService;

namespace ms_forum.Interface
{
    public interface IUsuarioService
    {
        Task<UsuarioResponse> GetUsuarioByIdAsync(long id);
    }
}
