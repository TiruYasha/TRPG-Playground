using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public class Participant
    {
        public Guid Id { get; set; }
        public virtual DndUser User { get; set; }
        public virtual Game Game { get; set; }
    }
}
