﻿using IW4MAdmin.Plugins.Stats.Models;
using SharedLibraryCore.Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IW4MAdmin.Plugins.Stats.Models
{
    public class EFClientStatHistory : SharedEntity
    {
        [Key]
        public int StatHistoryId { get; set; }
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual EFClient Client { get; set; }
        public int ServerId { get; set; }
        [ForeignKey("ServerId")]
        public virtual EFServer Server { get; set; }
        public virtual ICollection<EFRating> Ratings { get; set; }
    }
}
