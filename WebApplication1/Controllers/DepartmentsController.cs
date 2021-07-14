using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly EmployeeContext _Db;
        public DepartmentsController(EmployeeContext _db)
        {
            _Db = _db;
        }

        public IActionResult DepartmentList()
        {
            try
            {
                var DepList = _Db.tbl_Department.ToList();

                return View(DepList);
            }
            catch (Exception)
            {

                return View();
            }
            
        }


        public IActionResult Create(Departments obj)
        {
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment(Departments obj)
        {
            try
            {
                if (ModelState.IsValid)
                {                         
                    if(obj.ID == 0)
                    {
                        _Db.tbl_Department.Add(obj);
                        await _Db.SaveChangesAsync();                   
                    }
                    else
                    {
                        _Db.Entry(obj).State = EntityState.Modified;
                        await _Db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("DepartmentList");
            }
            catch (Exception)
            {

                return RedirectToAction("DepartmentList");
            }
        }

        
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var Dep = await _Db.tbl_Department.FindAsync(id);
                if(Dep != null)
                {
                    _Db.tbl_Department.Remove(Dep);
                    await _Db.SaveChangesAsync();
                }
                return RedirectToAction("DepartmentList");
            }
            catch (Exception)
            {

                return RedirectToAction("DepartmentList");
            }
        }

    }
}