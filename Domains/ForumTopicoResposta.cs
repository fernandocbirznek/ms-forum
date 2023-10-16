namespace ms_forum.Domains
{
    public class ForumTopicoResposta : Entity
    {
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public long ForumTopicoId { get; set; }

        private ForumTopico _ForumTopico;
        public virtual ForumTopico ForumTopico { get { return _ForumTopico; } set { _ForumTopico = value; SetForumTopico(value); } }

        public virtual ICollection<ForumTopicoReplica>? Replicas { get; set; }

        private void SetForumTopico(ForumTopico value)
        {
            ForumTopicoId = value is null ? 0 : value.Id;
        }
    }
}
