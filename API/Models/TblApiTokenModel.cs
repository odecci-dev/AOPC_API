﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace API.Models;

public partial class TblApiTokenModel
{
    public int Id { get; set; }

    public string ApiToken { get; set; }

    public string Role { get; set; }

    public string Name { get; set; }

    public int? Status { get; set; }
}