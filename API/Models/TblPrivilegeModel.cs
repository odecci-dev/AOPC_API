﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace API.Models;

public partial class TblPrivilegeModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Mechanics { get; set; }

    public DateTime? Validity { get; set; }

    public int? NoExpiry { get; set; }

    public DateTime DateCreated { get; set; }

    public string ImgUrl { get; set; }

    public int? VendorId { get; set; }

    public int? Active { get; set; }

    public string PrivilegeId { get; set; }

    public int? IsVip { get; set; }

    public int? BusinessTypeId { get; set; }

    public string Tmc { get; set; }
}