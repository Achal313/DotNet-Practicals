using Microsoft.AspNetCore.Mvc;
using VehicleRentalManagementSystem.Models;
using VehicleRentalManagementSystem.Services.Interfaces;

namespace VehicleRentalManagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllCustomersAsync();

            return View(customers);
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var customer = await _customerService.GetCustomerByIdAsync(id.Value);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            await _customerService.AddCustomerAsync(customer);

            TempData["Success"] = "Customer added successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var customer = await _customerService.GetCustomerByIdAsync(id.Value);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Customer customer)
        {
            if (id != customer.CustomerId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(customer);

            try
            {
                await _customerService.UpdateCustomerAsync(customer);

                TempData["Success"] =
                    "Customer updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var customer =
                await _customerService.GetCustomerByIdAsync(id.Value);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(id);

                TempData["Success"] =
                    "Customer deleted successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}