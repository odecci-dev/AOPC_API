﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public partial class SP_GetPrivilegeByPIDResult
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public DateTime? Validity { get; set; }
        public string Status { get; set; }
        public string PrivilegeID { get; set; }
        public string Mechanics { get; set; }
        public string BusinessTypeName { get; set; }
        public string Description { get; set; }
        public int? VendorID { get; set; }
        public string TMC { get; set; }
        public string VendorName { get; set; }
        public int? isVIP { get; set; }
    }
}
