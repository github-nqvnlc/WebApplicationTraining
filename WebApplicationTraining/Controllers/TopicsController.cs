﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using WebApplicationTraining.Models;

namespace WebApplicationTraining.Controllers
{
    public class TopicsController : Controller
    {
		// GET: Topics
		private ApplicationDbContext _context;
		public TopicsController()
		{
			_context = new ApplicationDbContext();
		}
		
		[HttpGet]
		public ActionResult Index()
		{
			var topics = _context.Topics.ToList();
			return View(topics);
		}

		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(Topic topic)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			if (_context.Topics.Any(p => p.Name.Contains(topic.Name)))
			{
				ModelState.AddModelError("Name", "Topic Name Already Exists.");
				return View();
			}

			var newTopic = new Topic
			{
				Name = topic.Name,
				Description = topic.Description,


			};

			_context.Topics.Add(newTopic);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Delete(int id)
		{
			var topicInDb = _context.Topics.SingleOrDefault(p => p.Id == id);

			if (topicInDb == null)
			{
				return HttpNotFound();
			}

			_context.Topics.Remove(topicInDb);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Edit(int id)
		{
			var topicInDb = _context.Topics.SingleOrDefault(p => p.Id == id);

			if (topicInDb == null)
			{
				return HttpNotFound();
			}

			return View(topicInDb);
		}

		[HttpPost]
		public ActionResult Edit(Topic topic)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var topicInDb = _context.Topics.SingleOrDefault(p => p.Id == topic.Id);

			if (topicInDb == null)
			{
				return HttpNotFound();
			}
			topicInDb.Name = topicInDb.Name;
			topicInDb.Description = topicInDb.Description;
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}