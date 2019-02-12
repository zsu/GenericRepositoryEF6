﻿using Example.Entities;
using GenericRepository;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Example.Controllers
{
    public class HomeController : Controller
    {
        private static Logger logger = LogManager.GetLogger(typeof(DataAccess).ToString());//.GetCurrentClassLogger();
        private readonly IUowProvider _uowProvider=new UowProvider(logger.Info);

        public async Task<ActionResult> Index()
        {
            //await Seed();
            IEnumerable<Department> buildings = null;
            using (var uow = _uowProvider.CreateUnitOfWork<AppContext>(true))
            {
                var repository = uow.GetRepository<Department>();
                //var t1=repository.Get(1);
                //**************************************

                var includes = new Includes<Department>(query =>
                {
                    return query.Include(b => b.Employees.Select(c=>c.Addresses));
                });

                buildings = await repository.GetAllAsync(null, includes.Expression);

                //var building = await repository.GetAsync(1, includes.Expression);

                //**************************************

                //Func<IQueryable<Building>, IQueryable<Building>> func = query =>
                //{
                //    return query.Include(b => b.Appartments)
                //                    .ThenInclude(a => a.Rooms);
                //};

                //buildings = await repository.GetAllAsync(null, func);

                //**************************************

                //buildings = await repository.GetAllAsync(null, query =>
                //{
                //    return query.Include(b => b.Appartments)
                //                    .ThenInclude(a => a.Rooms);
                //});


            }

            return View(buildings);
        }

        public async Task<ActionResult> Seed()
        {
            var buildings = new List<Department>
            {
                new Department
                {
                    Name = "IT",
                    Employees = new List<Employee>
                    {
                        new Employee
                        {
                            Age = 31,
                            Name = "Tom",
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    Number = 123,
                                    Street ="Main Street",
                                    City= "Miami",
                                    State="FL",
                                    Zipcode="11031"
                                },
                                new Address
                                {
                                    Number = 124,
                                    Street ="Main Street",
                                    City= "Miami",
                                    State="FL",
                                    Zipcode="11031"
                                },
                                new Address
                                {
                                    Number = 125,
                                    Street ="Main Street",
                                    City= "Miami",
                                    State="FL",
                                    Zipcode="11031"
                                }
                            }
                        },
                        new Employee
                        {
                            Age = 50,
                            Name = "Bill",
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    Number = 386,
                                    Street ="Gilber Street",
                                    City= "Miami",
                                    State="FL",
                                    Zipcode="15461"
                                },
                                new Address
                                {
                                    Number = 387,
                                    Street ="Gilber Street",
                                    City= "Miami",
                                    State="FL",
                                    Zipcode="15461"
                                }
                            }
                        }
                    }
                }
            };

            using (var uow = _uowProvider.CreateUnitOfWork<AppContext>(true))
            {
                var repository = uow.GetRepository<Department>();

                foreach (var item in buildings)
                {
                    repository.Add(item);
                }

                await uow.SaveChangesAsync();
            }
            return View();
        }


        public ActionResult Error()
        {
            return View();
        }
    }
}