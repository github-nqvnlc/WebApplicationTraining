﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationTraining.Models;
using WebApplicationTraining.ViewModels;

namespace WebApplicationTraining.Controllers
{
    public class CoursesController : Controller
    {
		// GET: Courses
		private ApplicationDbContext _context;
		public CoursesController()
		{
			_context = new ApplicationDbContext();
		}
		// GET: Staff
		[HttpGet]
		public ActionResult Index(string searchString)
		{
			var courses = _context.Courses
			.Include(p => p.Category);//.Include(c => c.Topic);

			if (!String.IsNullOrEmpty(searchString))
			{
				courses = courses.Where(
					s => s.Name.Contains(searchString) ||
					s.Category.Name.Contains(searchString));
					//s.Topic.Name.Contains(searchString));

			}

			return View(courses.ToList());
		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create()
		{
			var viewModel = new CourseCategoryViewModel
			{
				Categories = _context.Categories.ToList(),
				//Topics = _context.Topics.ToList()
			};
			return View(viewModel);
		}

		[HttpPost]
		[Authorize(Roles = "TrainingStaff")]
		public ActionResult Create(Course course)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			if (_context.Courses.Any(p => p.Name.Contains(course.Name)))
			{
				ModelState.AddModelError("Name", "Course Name Already Exists.");
				return View();
			}

			var newCourse = new Course
			{
				Name = course.Name,
				CategoryId = course.CategoryId,
				TopicId = 11,
			};

			_context.Courses.Add(newCourse);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]

		public ActionResult Delete(int id)
		{
			var courseInDb = _context.Courses.SingleOrDefault(p => p.Id == id);

			if (courseInDb == null)
			{
				return HttpNotFound();
			}

			_context.Courses.Remove(courseInDb);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		[HttpGet]
		[Authorize(Roles = "TrainingStaff")]

		public ActionResult Edit(int id)
		{
			var courseInDb = _context.Courses.SingleOrDefault(p => p.Id == id);

			if (courseInDb == null)
			{
				return HttpNotFound();
			}

			var viewModel = new CourseCategoryViewModel
			{
				Course = courseInDb,
				Categories = _context.Categories.ToList(),
				//Topics = _context.Topics.ToList()
			};

			return View(viewModel);
		}

		[HttpPost]
		[Authorize(Roles = "TrainingStaff")]

		public ActionResult Edit(Course course)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var courseInDb = _context.Courses.SingleOrDefault(p => p.Id == course.Id);

			if (courseInDb == null)
			{
				return HttpNotFound();
			}

			courseInDb.Name = course.Name;
			courseInDb.CategoryId = course.CategoryId;
			//courseInDb.TopicId = course.TopicId;
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}