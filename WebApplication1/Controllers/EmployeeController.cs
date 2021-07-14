using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;

namespace WebApplication1.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeContext _Db;
        public EmployeeController(EmployeeContext _db)
        {
            _Db = _db;
        }
        public IActionResult EmployeeList()
        {
            
            try
            {
                var EmpList = from a in _Db.tbl_Employee
                              join b in _Db.tbl_Department
                              on a.DepID equals b.ID
                              into Dep
                              from b in Dep.DefaultIfEmpty()

                              select new Employee
                              {
                                  ID = a.ID,
                                  EmpName = a.EmpName,
                                  Contact = a.Contact,
                                  Email = a.Email,
                                  Description = a.Description,
                                  DepID = a.DepID,
                                  Department= b==null?"":b.Department
                               
                              };
                          

                return View(EmpList);
            }
            catch (Exception)
            {

                return View();
            }       
            
        }
      
        public IActionResult Create(Employee obj)
        {
            DepLList();
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee Obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Obj.ID == 0)
                    {
                        _Db.tbl_Employee.Add(Obj);
                        await _Db.SaveChangesAsync();                      
                    }
                    else
                    {
                        _Db.Entry(Obj).State = EntityState.Modified;
                        await _Db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("EmployeeList");
                
            }
            catch (Exception)
            {

                return RedirectToAction("EmployeeList");
            }
        }


        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var Emp = await _Db.tbl_Employee.FindAsync(id);
                if (Emp != null)
                {
                    _Db.tbl_Employee.Remove(Emp);
                    await _Db.SaveChangesAsync();
                }
                return RedirectToAction("EmployeeList");
            }
            catch (Exception)
            {

                return RedirectToAction("EmployeeList");
            }

        }

        private void DepLList()
        {
            try
            {
                List<Departments> deptList = new List<Departments>();
                deptList = _Db.tbl_Department.ToList();
                deptList.Insert(0, new Departments { ID = 0, Department = "Please Select" });

                ViewBag.DeptList = deptList;
            }
            catch (Exception)
            {}
            
        }

        public async Task<List<Employee>> Import(IFormFile file)
        {
            var list = new List<Employee>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    for (int row = 2; row < rowcount; row++)
                    {
                        list.Add(new Employee
                        {
                            ID = Convert.ToUInt16(worksheet.Cells[row, 1]),
                            EmpName = worksheet.Cells[row, 2].ToString().Trim(),
                            Contact = Convert.ToUInt32(worksheet.Cells[row, 3]),
                            Email = worksheet.Cells[row, 4].ToString().Trim(),
                            Description = worksheet.Cells[row, 5].ToString().Trim(),
                            DepID = Convert.ToUInt16(worksheet.Cells[row, 6])
                        });
                    }
                }
            }

            return list;
        }
    }


    
}