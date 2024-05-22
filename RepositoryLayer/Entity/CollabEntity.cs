using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class CollabEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollabId { get; set; }
        public string CollabEmail { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }
        public virtual UserEntity Users { get; set; }
        [ForeignKey("UserNotes")]
        public int NoteId { get; set; }
        [JsonIgnore]
        public virtual NotesEntity UserNotes { get; set; }

    }
}
