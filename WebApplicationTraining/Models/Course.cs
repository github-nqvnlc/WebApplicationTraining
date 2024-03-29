﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationTraining.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}