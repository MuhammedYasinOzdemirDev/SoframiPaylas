using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoframiPaylas.Domain.Entities
{
    public class Message
    {
        public int MessageID { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        // Navigation properties
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}