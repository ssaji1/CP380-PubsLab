using System;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CP380_PubsLab
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dbcontext = new Models.PubsDbContext())
            {
                if (dbcontext.Database.CanConnect())
                {
                    Console.WriteLine("Yes, I can connect");
                }

                // 1:Many practice
                //
                // TODO: - Loop through each employee
                //       - For each employee, list their job description (job_desc, in the jobs table)
                var employee = dbcontext.Employee
                    .Select(emp => new {
                        emp_id = emp.emp_id,
                        fname = emp.fname,
                        lname = emp.lname,
                        job_desc = emp.Job.job_desc
                    })
                    .ToList();                         
                Console.WriteLine("EMPLOYEE JOB DESCRIPTION\n\n");
                Console.WriteLine("Name  \t\t\t Job Description\n\n");
                foreach (var emp in employee)          
                {
                    Console.WriteLine(emp.fname + " " + emp.lname + "\t\t\t" + emp.job_desc); 
                }

                var job_list = dbcontext.Jobs.ToList();
                Console.WriteLine("\n\nLIST OF EMPLOYEES BASED ON JOB\n\n");
                foreach (var job in job_list)         
                {
                    Console.WriteLine("\nList of Employees in " + job.job_desc);
                    var job_emp = dbcontext.Employee
                                  .Where(emp => emp.job_id == job.job_id)
                                  .ToList();
                    foreach (var emp in job_emp)   
                    {
                        Console.WriteLine(emp.fname + " " + emp.lname);
                    }
                }
                // TODO: - Loop through all of the jobs
                //       - For each job, list the employees (first name, last name) that have that job


                // Many:many practice
                //
                // TODO: - Loop through each Store
                //       - For each store, list all the titles sold at that store
                //
                // e.g.
                //  Bookbeat -> The Gourmet Microwave, The Busy Executive's Database Guide, Cooking with Computers: Surreptitious Balance Sheets, But Is It User Friendly?
                var stores = dbcontext.Stores.ToList();
                var titles = dbcontext.Titles.ToList();
                var sales = dbcontext.Sales.ToList();

                Console.WriteLine("\n\n\n Details of Books Sold at Every Store\n");
                foreach (var store in stores)
                {
                    Console.Write(store.stor_name + " -> ");
                    var sale_list = sales.Where(str => str.stor_id == store.stor_id).ToList();
                    var i = 0;
                    foreach (var sale in sale_list)
                    {
                        if (i != 0)
                        {
                            Console.Write(", ");
                        }
                        Console.Write(titles.First(t => t.title_id == sale.title_id).title);
                        i++;
                    }
                    Console.WriteLine("\n");
                }

                Console.WriteLine("\n\n\n Details of  Available Stores With This Book Details\n\n");
                foreach (var title in titles)
                {
                    Console.Write(title.title + " -> ");
                    var sale_list = sales.Where(str => str.title_id == title.title_id).ToList();
                    var i = 0;
                    foreach (var sale in sale_list)
                    {
                        if (i != 0)
                        {
                            Console.Write(", ");
                        }
                        Console.Write(stores.First(t => t.stor_id == sale.stor_id).stor_name);
                        i++;
                    }
                    Console.WriteLine("\n");
                }
                // TODO: - Loop through each Title
                //       - For each title, list all the stores it was sold at
                //
                // e.g.
                //  The Gourmet Microwave -> Doc-U-Mat: Quality Laundry and Books, Bookbeat
            }
        }
    }
}
