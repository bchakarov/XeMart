namespace XeMart.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using XeMart.Data.Common.Models;

    public class ChatRoom : BaseDeletableModel<string>
    {
        public ChatRoom()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Messages = new HashSet<RoomMessage>();
        }

        [Required]
        [ForeignKey(nameof(Owner))]
        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<RoomMessage> Messages { get; set; }
    }
}
