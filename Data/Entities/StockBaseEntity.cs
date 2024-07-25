using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Entities;

public class StockBaseEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid? CreateUid { get; set; }

    public Guid? WriteUid { get; set; }

    public DateTime? CreateDate { get; set; } = DateTime.Now;

    public DateTime? WriteDate { get; set; } = DateTime.Now;

    public bool? Active { get; set; } = true;

    [ForeignKey("CreateUid")]
    public virtual User CreateUser { get; set; }

    [ForeignKey("WriteUid")]
    public virtual User WriteUser { get; set; }
}
