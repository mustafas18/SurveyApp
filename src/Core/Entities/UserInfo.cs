using Ardalis.GuardClauses;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserInfo : BaseEntity, IAggregateRoot
    {
        public UserInfo(string userName, string firstName, string lastName)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
        }

        public string UserName { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public GenderEnum? Gender { get; private set; }
        public string? Birthday { get; private set; }
        public string? PictureBase64 { get; private set; }
        public string? Country { get; private set; }
        public string? City { get; private set; }
        public int? ResearchInterestId { get; private set; }
        private List<UserDegree> _userDegrees = new List<UserDegree>();
        public IReadOnlyCollection<UserDegree>? EducationDegree => _userDegrees.AsReadOnly();
        public string? Grade { get;private set; }
        public string? Job { get;private set; }
        public string? Mobile { get;private set; }
        public List<Sheet>? Sheets { get; set; }
        public string? Address { get;private set; }
        public string? AtmCard { get;private set; }
        public byte[]? CVFileData { get;private set; }
        public string? FileContent { get;private set; }
        public bool Deleted { get;private set; }
        public void AddDegree(UserDegree degree)
        {
            _userDegrees.Add(degree);

        }
    }
}
