using Microsoft.AspNetCore.Http;
using OneRegister.Data.Contract;
using OneRegister.Data.SuperEntities;
using OneRegister.Domain.Validation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Model.StudentRegistration
{
    public class StudentRegisterModel
    {
        public Guid Id { get; set; }

        [CustomRequired]
        [Display(Name = "Student Number")]
        public string StudentNumber { get; set; }

        [CustomRequired]
        [Display(Name = "School")]
        public Guid? SchoolId { get; set; }

        [CustomRequired]
        public int? Year { get; set; }

        [CustomRequired]
        [StudentUniqueness]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Identity Type")]
        [CustomRequired]
        public string IdentityType { get; set; }

        [Display(Name = "Gender")]
        [CustomRequired]
        public Gender? Gender { get; set; }

        [Display(Name = "Nationality")]
        [CustomRequired]
        public string Nationality { get; set; }

        [Display(Name = "IC /Passport number")]
        [CustomRequired]
        public string IdentityNumber { get; set; }

        [CustomRequired]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [Image(MaxSize = 1)]
        public IFormFile Photo { get; set; }

        public long? PhotoId { get; set; }
        public string PhotoUrl { get; set; }

        [Image(MaxSize = 1)]
        public IFormFile Thumbnail { get; set; }

        public long? ThumbnailId { get; set; }

        [CustomRequired]
        [Display(Name = "Class")]
        public Guid? ClassId { get; set; }

        [Display(Name = "HomeRoom")]
        public Guid? HomeRoomId { get; set; }

        [CustomRequired]
        [Display(Name = "Parent Name")]
        public string ParentName { get; set; }

        [CustomRequired]
        [Display(Name = "Parent Phone Number")]
        public string ParentPhone { get; set; }

        [CustomRequired]
        [Display(Name = "Residential/Mailing Address")]
        public string Address { get; set; }

        public StateOfEntity State { get; set; }
    }
}
