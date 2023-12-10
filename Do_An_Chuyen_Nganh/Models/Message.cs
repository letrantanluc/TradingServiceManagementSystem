using Do_An_Chuyen_Nganh.Data;
using System.ComponentModel.DataAnnotations;

namespace Do_An_Chuyen_Nganh.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime When { get; set; }
        public string SenderID { get; set; }
        public string? SenderUsername { get; set; }
        public string? ReceiverUsername { get; set; }
        public string ReceiverID { get; set; }
        public Message()
        {
            When = DateTime.Now;
        }

        internal object ToListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
