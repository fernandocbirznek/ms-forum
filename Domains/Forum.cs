namespace ms_forum.Domains
{
    public class Forum : Entity
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public virtual ICollection<ForumTopico>? ForumTopicos { get; set; }
    }
}
