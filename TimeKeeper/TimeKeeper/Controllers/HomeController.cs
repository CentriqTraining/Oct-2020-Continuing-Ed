using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using TimeKeeper.Models;

namespace TimeKeeper.Controllers
{
    public class HomeController : Controller
    {
        private TimeKeeperDb _db;
        public HomeController(TimeKeeperDb db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var data = _db.TimeCharges
                .Include(t => t.Department)
                .Include(t => t.Project)
                .AsEnumerable();
            return View(data);
        }

        public IActionResult Create()
        {
            ChargeViewModel data = GetViewModelDataNormal();
            return View(data);
        }
        private ChargeViewModel GetViewModelDataNormal()
        {
            return new ChargeViewModel()
            {
                Projects = _db.Projects.Select(p => new SelectListItem()
                {
                    Text = p.ProjectName,
                    Value = p.ID.ToString()
                }).ToList(),
                Departments = _db.Departments.Select(d => new SelectListItem()
                {
                    Text = d.Name,
                    Value = d.ID.ToString()
                }).ToList()
            };
        }

        [HttpPost]
        public IActionResult Create(ChargeViewModel data)
        {
            if (ModelState.IsValid)
            {
                return AddNewTimeCharge(data);
            }
            return View(data);
        }
        private IActionResult AddNewTimeCharge(ChargeViewModel data)
        {
            var Dept = _db.Departments.FirstOrDefault(d => d.ID == int.Parse(data.Department));
            var Proj = _db.Projects.FirstOrDefault(d => d.ID == int.Parse(data.Project));
            if (Dept != null && Proj != null)
            {
                _db.TimeCharges.Add(new Charge()
                {
                    EmpID = data.EmpID,
                    Hours = data.Hours,
                    DateEntered = DateTime.Now,
                    Department = Dept,
                    Project = Proj
                });
            }
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return View(data);
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
