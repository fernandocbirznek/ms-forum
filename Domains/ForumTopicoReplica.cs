namespace ms_forum.Domains
{
    public class ForumTopicoReplica : Entity
    {
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public long ForumTopicoRespostaId { get; set; }

        private ForumTopicoResposta _ForumTopicoResposta;
        public virtual ForumTopicoResposta ForumTopicoResposta { get { return _ForumTopicoResposta; } set { _ForumTopicoResposta = value; SetForumTopicoResposta(value); } }

        private void SetForumTopicoResposta(ForumTopicoResposta value)
        {
            ForumTopicoRespostaId = value is null ? 0 : value.Id;
        }
    }
}
