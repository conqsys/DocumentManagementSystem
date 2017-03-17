using Ticket.Base;
using Ticket.Base.Entities;
using SimpleStack.Orm.Attributes;

namespace Ticket.DataAccess.TicketService
{
    [TableWithSequence("email_template", SequenceName = "email_template_id_seq")]
    [Alias("email_template")]
    public class EmailTemplate : IEmailTemplate
    {
        [AutoIncrement]
        [Alias("id")]
        public long Id { get; set; }

        [Alias("default_mail_from_name")]
        public string DefaultMailFromName { get; set; }

        [Alias("default_mail_from_id")]
        public string DefaultMailFromId { get; set; }

        [Alias("default_mail_to_name")]
        public string DefaultMailToName { get; set; }

        [Alias("default_mail_to_id")]
        public string DefaultMailToId { get; set; }

        [Alias("default_mail_subject")]
        public string DefaultMailSubject { get; set; }

        [Alias("default_mail_body")]
        public string DefaultMailBody { get; set; }

        [Alias("default_mail_cc")]
        public string DefaultMailCC { get; set; }

        [Alias("event_type")]
        public int EventType { get; set; }
    }
}
