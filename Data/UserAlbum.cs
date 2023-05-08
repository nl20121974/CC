﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CC.Data;

[Table("UserAlbum")]
public partial class UserAlbum
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(450)]
    public string UserId { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [ForeignKey("Id")]
    [InverseProperty("UserAlbum")]
    public virtual UserMedium IdNavigation { get; set; }
}