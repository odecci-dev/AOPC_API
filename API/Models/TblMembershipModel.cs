﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace API.Models;

public partial class TblMembershipModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime? DateUsed { get; set; }

    public DateTime? DateEnded { get; set; }

    public DateTime DateCreated { get; set; }

    public int? Status { get; set; }

    public string MembershipId { get; set; }

    public int? UserCount { get; set; }

    public int? Vipcount { get; set; }

    public string MembershipCard { get; set; }

    public string Vipcard { get; set; }

    public string Qrframe { get; set; }

    public string Vipbadge { get; set; }
}