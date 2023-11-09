using Authentication_testing.DataAccess;
using Authentication_testing.Enums;
using Authentication_testing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authentication_testing.Controllers {
    public class EmployeesController : Controller {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: Employees
        [Authorize(Roles = AccountRoles.Medewerker + "," + AccountRoles.Manager + "," + AccountRoles.Eigenaar)]

        public async Task<IActionResult> Index() {
            return _context.Employees != null ?
                        View(await _context.Employees.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Employees'  is null.");
        }

        // GET: Employees/Details/5
        [Authorize(Roles = AccountRoles.Medewerker + "," + AccountRoles.Manager + "," + AccountRoles.Eigenaar)]

        public async Task<IActionResult> Details(int? id) {
            if (id == null || _context.Employees == null) {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null) {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        [Authorize(Roles = AccountRoles.Manager + "," + AccountRoles.Eigenaar)]

        public IActionResult Create() {
            return View();
        }

        // POST: Employees/Create
        [Authorize(Roles = AccountRoles.Manager + "," + AccountRoles.Eigenaar)]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Birthdate,Salary,JobTitle")] Employee employee) {
            if (ModelState.IsValid) {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = AccountRoles.Manager + "," + AccountRoles.Eigenaar)]

        public async Task<IActionResult> Edit(int? id) {
            if (id == null || _context.Employees == null) {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        [Authorize(Roles = AccountRoles.Manager + "," + AccountRoles.Eigenaar)]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Birthdate,Salary,JobTitle")] Employee employee) {
            if (id != employee.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!EmployeeExists(employee.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = AccountRoles.Eigenaar)]

        public async Task<IActionResult> Delete(int? id) {
            if (id == null || _context.Employees == null) {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null) {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [Authorize(Roles = AccountRoles.Eigenaar)]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            if (_context.Employees == null) {
                return Problem("Entity set 'ApplicationDbContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null) {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id) {
            return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
