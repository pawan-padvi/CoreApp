using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Models
{
    public class ReturnModel
    {
            [JsonIgnore]
            public Int64 DbId { get; set; }
            [JsonIgnore]
            public int DbCode { get; set; }
            public int RowNum { get; set; }
            [JsonIgnore]
            public string DbMsg { get; set; }
            public DateTime? toDate { get; set; }
            public DateTime? fromDate { get; set; }
            public int complaintStatus { get; set; }
        
    }
}
