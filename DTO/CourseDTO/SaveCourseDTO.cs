﻿using Lombeo.Api.Authorize.Infra.Enums;

namespace Lombeo.Api.Authorize.DTO.CourseDTO
{
	public class SaveCourseDTO : BaseRequest
	{
		public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public int AuthorId { get; set; }
        public string[] Skills { get; set; }
        public string[] WhatYouWillLearn { get; set; }
        public bool HasCert { get; set; }
        public decimal Price { get; set; }
    }
}
