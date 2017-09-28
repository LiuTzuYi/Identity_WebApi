using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Identity_WebApi.Controllers
{
    public class Employee {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Salary { get; set; }
    }

    [Authorize]
    public class EmployeesController : ApiController
    {
        public IEnumerable<Employee> GetEmployees() {

            List<Employee> employees = new List<Employee>() {
              new Employee(){ ID="1", FirstName="AAA", LastName="000", Gender="M", Salary="40000" },
              new Employee(){ ID="2", FirstName="BBB", LastName="111", Gender="F", Salary="40000" },
              new Employee(){ ID="3", FirstName="CCC", LastName="222", Gender="M", Salary="40000" },
              new Employee(){ ID="4", FirstName="DDD", LastName="333", Gender="F", Salary="40000" },
              new Employee(){ ID="5", FirstName="EEE", LastName="444", Gender="M", Salary="40000" },
              new Employee(){ ID="6", FirstName="FFF", LastName="555", Gender="F", Salary="40000" },
              new Employee(){ ID="7", FirstName="GGG", LastName="666", Gender="M", Salary="40000" },
              new Employee(){ ID="8", FirstName="HHH", LastName="777", Gender="F", Salary="40000" },
              new Employee(){ ID="9", FirstName="III", LastName="888", Gender="M", Salary="40000" },
              new Employee(){ ID="10", FirstName="JJJ", LastName="999", Gender="F", Salary="40000" }
            };

            return employees.ToList();
        }
    }
}
