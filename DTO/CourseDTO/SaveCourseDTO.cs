﻿using Lombeo.Api.Authorize.Infra.Enums;

namespace Lombeo.Api.Authorize.DTO.CourseDTO
{
	public class SaveCourseDTO : BaseRequest
	{
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string SubDescription { get; set; }
        public string Description { get; set; }
        public string CourseImage { get; set; }
        public string[] Skills { get; set; }
        public string[] WhatYouWillLearn { get; set; }
        public decimal Price { get; set; }
        public int PercentOff { get; set; }
    }
}
