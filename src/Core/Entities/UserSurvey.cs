﻿using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserSurvey : BaseEntity
    {
        public string Guid { get; set; }
        public int Version { get; set; }
        public string SheetId { get; set; }
        public int SheetVersion { get; set; }
        public string? SurveyTitle { get; set; }
        public string? UserName { get; set; }
        public int? CategoryId { get; set; }
        public string? Link { get; set; }
        public DateTime? DeadLine { get; set; }
        public DateTime? ParticipateTime { get; set; }
        public DateTime? CreatedTime { get; set; }
        [Comment($"0: Pending, 1: Completed")]
        public SurveyStatusEnum? Status { get; set; }
        [Comment("0: Participant, 1: Categoty")]
        public InviteTypeEnum? InviteType { get; set; }
        public bool IsTemplate { get; set; }
        public bool IsDelete { get; set; }
    }

}

