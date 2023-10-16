namespace ms_forum.Domains
{
    public class ForumTag : Entity
    {
        public string Titulo { get; set; }
        public virtual ICollection<ForumTopicoTag>? ForumTopicoTags { get; set; }
    }
}
