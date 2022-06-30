using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;
        private int NumberOfReports;

        public ReportingStructureService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            NumberOfReports = 0;
        }
        //return Reporting structure calculate number of reports using recursion
        public ReportingStructure GetById(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                ReportingStructure myReturn = null;
                Employee myEmployee = _employeeRepository.GetById(id);
                if (myEmployee != null)
                {
                    myReturn = new ReportingStructure();
                    myReturn.Employee = myEmployee;
                    if (myEmployee.DirectReports != null)
                    {
                        List<Employee> directReports = myEmployee.DirectReports;
                        //Calc numberOfReports
                        foreach (var report in directReports)
                        {
                            GetDirectReports(report);
                        }
                        myReturn.NumberOfReports = NumberOfReports;
                    }
                    else
                    {
                        myReturn.NumberOfReports = 0; //no direct reports
                    }
                }
                return myReturn;//reporting structure is null if not found
            }
            return null;
        }//get by id
        //recursive goes through any direct reports to add to the number of reports to calculate a total by emp #
        void GetDirectReports(Employee DirectReport)
        {
            NumberOfReports++;

            if (DirectReport.DirectReports!=null)// && DirectReport.DirectReports.Any())
            {
                foreach (Employee directReport in DirectReport.DirectReports)
                    GetDirectReports(directReport);
            }
        }
    }//reporting structure service
}//namespace
