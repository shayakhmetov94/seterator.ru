﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rauthor.Models
{
    [Table("poem")]
    public class Poem
    {
        [Key]
        [Column("GUID")]
        public Guid Guid { get; set; }
        
        [Column("Text")]
        public string Text { get; set; }

        [Column("author_GUID")]
        public Guid UserGuid { get; set; }

        [ForeignKey("UserGuid")]
        public User Author { get; set; }

        public Poem()
        {
            Guid = Guid.NewGuid();
        }
    }
}