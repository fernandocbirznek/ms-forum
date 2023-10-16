namespace ms_forum.Domains
{
    public class ForumTopicoTag : Entity
    {
        public long ForumTagId { get; set; }
        private ForumTag _ForumTag;
        public virtual ForumTag ForumTag { get { return _ForumTag; } set { _ForumTag = value; SetForumTag(value); } }

        private void SetForumTag(ForumTag value)
        {
            ForumTagId = value is null ? 0 : value.Id;
        }

        public long ForumTopicoId { get; set; }
        private ForumTopico _ForumTopico;
        public virtual ForumTopico ForumTopico { get { return _ForumTopico; } set { _ForumTopico = value; SetForumTopico(value); } }

        private void SetForumTopico(ForumTopico value)
        {
            ForumTopicoId = value is null ? 0 : value.Id;
        }
    }
}
