﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace API.Models;

public partial class TblVouchersModel
{
    public int Id { get; set; }

    public string VoucherName { get; set; }

    public string Description { get; set; }

    public int? VendorId { get; set; }

    public DateTime? Expiry { get; set; }

    public string Count { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? CreatedBy { get; set; }
}