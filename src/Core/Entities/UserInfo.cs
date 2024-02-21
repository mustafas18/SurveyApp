using Ardalis.GuardClauses;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Index(nameof(UserName), Name = "Index_UserName")]
    public class UserInfo : BaseEntity, IAggregateRoot
    {
        public UserInfo()
        {
                
        }
        public UserInfo(string userName, string firstName, string lastName)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
        }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        [NotMapped]
        public int? CategoryId { get; set; }
        public UserCategory? Category { get; set; }
        public GenderEnum? Gender { get; set; }
        public string? Birthday { get; set; }
        public string? PictureBase64 { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? ResearchInterests { get; set; }
        private List<UserDegree> _userDegrees = new List<UserDegree>();
        public IReadOnlyCollection<UserDegree>? EducationDegree => _userDegrees.AsReadOnly();
        public IReadOnlyCollection<UserDegreeMajor>? UserDegreeMajor => _userMajor.AsReadOnly();
        private List<UserDegreeMajor> _userMajor = new List<UserDegreeMajor>();
        public string? Grade { get; set; }
        public string? Job { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        [JsonIgnore]
        public List<Sheet>? Sheets { get; set; }
        public string? Address { get; set; }
        public string? AtmCard { get; set; }
        public byte[]? CVFileData { get; set; }
        public string? FileContent { get; set; }
        public bool Deleted { get; set; }
        [DefaultValue(false)]
        public bool IsVerified { get; set; }
        public void AddDegree(UserDegree degree)
        {
            _userDegrees.Add(degree);

        }
        public void RemoveDegree(UserDegree degree)
        {
            _userDegrees.Remove(degree);

        }
        public void SetCategory(UserCategory category) {
            IsVerified = true;
            Category = category;
        }
        public void UpdateUserInfo(UserInfoDetails infoDetails)
        {
            Category = infoDetails.Category;
            Gender = infoDetails.Gender;
            Birthday = infoDetails.Birthday;
            Country = infoDetails.Country;
            City = infoDetails.City;
            ResearchInterests = infoDetails.ResearchInterest;
            Grade = infoDetails.Grade;
            Job = infoDetails.Job;
            Mobile = infoDetails.Mobile;
            Email = infoDetails.Email;
            Address = infoDetails.Address;
            AtmCard = infoDetails.AtmCard;
            CVFileData = infoDetails.CVFileData;
            FileContent = infoDetails.FileContent;
        }
    }
    public record struct UserInfoDetails
    {
        public UserInfoDetails()
        {
        }
        public GenderEnum? Gender { get; set; }
        public string? Birthday { get; set; }
        public string? PictureBase64 { get; set; }
        public string? Country { get; set; }
        public UserCategory? Category { get; set; }  
        public string? City { get; set; }
        public string? ResearchInterest { get; set; }
        public string? Grade { get; set; }
        public string? Job { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? AtmCard { get; set; }
        public byte[]? CVFileData { get; set; }
        public string? FileContent { get; set; }

    }
}
