﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace API.Models;

public partial class TblCorporatePrivilegeLogsModel
{
    public int Id { get; set; }

    public string RedeemedBy { get; set; }

    public int? PrivilegeOwnerId { get; set; }

    public int? BusinessId { get; set; }

    public int? VendorId { get; set; }

    public int? PrivilegeId { get; set; }

    public DateTime? DateRedeemed { get; set; }

    public DateTime? DateCreated { get; set; }
}