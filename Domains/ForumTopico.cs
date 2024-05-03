using ms_forum.Enum;

namespace ms_forum.Domains
{
    public class ForumTopico : Entity
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public ForumTopicoEnum ForumTopicoEnum { get; set; }
        public long UsuarioId { get; set; }
        public virtual ICollection<ForumTopicoTag>? ForumTopicoTags { get; set; }

        public long ForumId { get; set; }
        private Forum _Forum;
        public virtual Forum Forum { get { return _Forum; } set { _Forum = value; SetForum(value); } }

        public virtual ICollection<ForumTopicoResposta>? Respostas { get; set; }

        private void SetForum(Forum value)
        {
            ForumId = value is null ? 0 : value.Id;
        }
    }
}
