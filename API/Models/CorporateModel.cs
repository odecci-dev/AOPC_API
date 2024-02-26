﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Models
{
    public class CorporateModel
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string CorporateName { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? Address { get; set; }  
        
        [Column(TypeName = "varchar(MAX)")]
        public string? CNo { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? EmailAddress { get; set; }

        [Column(TypeName = "int")]
        public int? Status { get; set; }

        [Column(TypeName = "int")]
        public int? MembershipID { get; set; }
        [Column(TypeName = "int")]
        public int Count { get; set; }
        [Column(TypeName = "int")]
        public int? VipCount { get; set; }


        public DateTime? DateUsed { get; set; }
        public DateTime? DateEnded { get; set; }


    }
}
