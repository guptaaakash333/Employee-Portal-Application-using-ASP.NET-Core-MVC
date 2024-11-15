using EmployeePortal.DataService;
using EmployeePortal.Models;
using EmployeePortal.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeePortal.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;
        public EmployeeController()
        {
            _employeeService = new EmployeeService();
        }
        [HttpGet]
        public async Task<IActionResult> List(
        [FromQuery] string SearchTerm, // Model binding from the query string
        [FromQuery] string SelectedDepartment, // Model binding from the query string
        [FromQuery] string SelectedType, // Model binding from the query string
        [FromQuery] int PageNumber = 1, // Model binding from the query string with a default value
        [FromQuery] int PageSize = 5) // Model binding from the query string with a default value
        {
            // Retrieve the filtered and paginated list of employees
            var (employees, totalCount) = await _employeeService.GetEmployees(SearchTerm, SelectedDepartment, SelectedType, PageNumber, PageSize);
            var viewModel = new EmployeeListViewModel
            {
                Employees = employees,
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / PageSize),
                SearchTerm = SearchTerm,
                SelectedDepartment = SelectedDepartment,
                SelectedType = SelectedType
            };
            // Generate SelectLists for dropdowns
            GetSelectLists();

            // Set page size options in ViewBag for use in the view
            ViewBag.PageSizeOptions = new SelectList(new List<int> { 3, 5, 10, 15, 20, 25 }, PageSize);
            return View(viewModel);
        }


        [HttpGet]
        public IActionResult Create()
        {
            // Prepare dropdown options before rendering the Create view
            GetSelectLists();
            return View();
        }
        // Model binding from the form data
        [HttpPost]
        public IActionResult Create([FromForm] Employee employee)
        {
            if (ModelState.IsValid)
            {
                // Create a new employee and redirect to the Success page
                _employeeService.CreateEmployee(employee);
                return RedirectToAction("Success", new { id = employee.Id });
            }
            // If validation fails, regenerate dropdown options and return the view with validation errors
            GetSelectLists();
            return View(employee);
        }
        // Model binding from the route data
        public IActionResult Success([FromRoute] int id)
        {
            // Retrieve the employee by ID and display the Success view
            var employee = _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        // Model binding from the route data
        public IActionResult Details([FromRoute] int id)
        {
            // Retrieve the employee details by ID and display the Details view
            var employee = _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        // Model binding from the route data
        [HttpGet]
        public IActionResult Update([FromRoute] int id)
        {
            // Retrieve the employee by ID and prepare the Update view
            var employee = _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            // Prepare dropdown options before rendering the Update view
            GetSelectLists();
            return View(employee);
        }
        // Model binding from the form data
        [HttpPost]
        public IActionResult Update([FromForm] Employee employee)
        {
            if (ModelState.IsValid)
            {
                // Update the employee details and redirect to the List view
                _employeeService.UpdateEmployee(employee);
                TempData["Message"] = $"Employee with ID {employee.Id} and Name {employee.FullName} has been updated.";
                return RedirectToAction("List");
            }
            // If validation fails, regenerate dropdown options and return the view with validation errors
            GetSelectLists();
            return View(employee);
        }
        // Model binding from the route data
        [HttpGet]
        public IActionResult Delete([FromRoute] int id)
        {
            // Retrieve the employee by ID and prepare the Delete confirmation view
            var employee = _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        // We use ActionName to map this method to the "Delete" action
        // Model binding from the route data
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed([FromRoute] int id)
        {
            // Retrieve the employee by ID and delete the employee
            var employee = _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            // Perform the deletion and redirect to the List view
            _employeeService.DeleteEmployee(id);
            TempData["Message"] = $"Employee with ID {id} and Name {employee.FullName} has been deleted.";
            return RedirectToAction("List");
        }
        // Model binding from the query string (default binding)
        [HttpGet]
        public JsonResult GetPositions(Department department)
        {
            // This method returns a list of positions based on the department selected by the user
            var positions = new Dictionary<Department, List<string>>
            {
                { Department.IT, new List<string> { "Software Developer", "System Administrator", "Network Engineer" } },
                { Department.HR, new List<string> { "HR Specialist", "HR Manager", "Talent Acquisition Coordinator" } },
                { Department.Sales, new List<string> { "Sales Executive", "Sales Manager", "Account Executive" } },
                { Department.Admin, new List<string> { "Office Manager", "Executive Assistant", "Receptionist" } }
            };
            // Check if the department exists in the dictionary, and return the corresponding positions
            var result = positions.ContainsKey(department) ? positions[department] : new List<string>();
            // Return the positions as a JSON response to be used in client-side scripts
            return Json(result);
        }
        // Private method to generate SelectLists for dropdowns in the views

        private void GetSelectLists()
        {
            ViewBag.DepartmentOptions = new SelectList(Enum.GetValues(typeof(Department)).Cast<Department>());
            ViewBag.EmployeeTypeOptions = new SelectList(Enum.GetValues(typeof(EmployeeType)).Cast<EmployeeType>());
        }
    }
}
