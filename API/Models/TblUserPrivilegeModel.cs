﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace API.Models;

public partial class TblUserPrivilegeModel
{
    public int Id { get; set; }

    public int? PrivilegeId { get; set; }

    public int? UserId { get; set; }

    public DateTime Validity { get; set; }

    public DateTime DateCreated { get; set; }
}