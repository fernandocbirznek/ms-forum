using ms_forum.Domains;
using ms_forum.Features.ForumTopicoTagFeature.Commands;

namespace ms_forum.Extensions
{
    public static class ForumTopicoTagExtensions
    {
        public static ForumTopicoTag ToDomain(this InserirForumTopicoTagCommand request)
        {
            return new()
            {
                ForumTagId = request.ForumTagId,
                ForumTopicoId = request.ForumTopicoId,
                DataCadastro = DateTime.Now
            };
        }

        public static ForumTopicoTag ToUpdate(this ForumTopicoTag request)
        {
            return new()
            {
                ForumTagId = request.ForumTagId,
                ForumTopicoId = request.ForumTopicoId,
                DataAtualizacao = DateTime.Now
            };
        }
    }
}
