using System;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities;
using Microsoft.AspNetCore.Http;

namespace Data.Models
{
    public class ReportOfMonthModel
    {
        public int ReceiptCompleted { get; set; }
        public int ReceiptPending { get; set; }
        public int PickingRequestCompleted { get; set; }
        public int PickingRequestPending{ get; set; }

    }
}

