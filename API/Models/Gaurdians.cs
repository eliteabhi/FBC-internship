﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace test2.Models
{
    public partial class Gaurdians
    {
        [JsonIgnore]
        public int GaurdianId { get; set; }
        public int Sdid { get; set; }
        public string GaurdianFirstName { get; set; } = null!;
        public string GaurdianLastName { get; set; } = null!;
        public string GaurdianEmail { get; set; } = null!;
        public long? GaurdianPhone { get; set; }
        public string UserCreated { get; set; } = null!;
        public DateTime? UserCreatedDateTime { get; set; }
        public string? UserUpdated { get; set; }
        public DateTime? UserUpdatedDateTime { get; set; }

        [JsonIgnore]
        public virtual SurveyInfo? Sd { get; } = null!;
    }
}