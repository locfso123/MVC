using Microsoft.AspNetCore.Mvc;
using mvc01.Models.Contact;
using Microsoft.EntityFrameworkCore;
using mvc01.Models;
using Microsoft.AspNetCore.Authorization;
using mvc01.Data;

namespace mvc01.Areas.Contact.Controllers
{
    [Area("Contact")]
    [Authorize(Roles = RoleName.Administrator)]
    public class ContactController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ContactController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Contact
        [HttpGet("/admin/contact")]
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.Contacts.ToListAsync());
        }

        // GET: Contact/Details/5
        [HttpGet("/admin/contact/detail/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _dbContext.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: Contact/Create
        //public IActionResult Create()
        [HttpGet("/contact/")]
        [AllowAnonymous]
        public IActionResult SendContact()
        {
            return View();
        }

        // POST: Contact/Create
        [HttpPost("/contact/")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,FullName,Email,DateSent,Message,phone")] Contact contact)
        public async Task<IActionResult> SendContact([Bind("Id,FullName,Email,Message,phone")] Models.Contact.Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.DateSent = DateTime.Now;

                _dbContext.Add(contact);
                await _dbContext.SaveChangesAsync();

                StatusMessage = "Lien he cua ban da duoc gui";

               /* return RedirectToAction(nameof(Index));*/
                return RedirectToAction("Index", "Home");
            }
            return View(contact);
        }

        /*// GET: Contact/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _dbContext.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contact/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,DateSent,Message,phone")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(contact);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }*/

        // GET: Contact/Delete/5
        [HttpGet("/admin/contact/delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _dbContext.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contact/Delete/5
        [HttpPost("/admin/contact/delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _dbContext.Contacts.FindAsync(id);
            if (contact != null)
            {
                _dbContext.Contacts.Remove(contact);
            }
            
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

         /*private bool ContactExists(int id)
        {
            return _dbContext.Contacts.Any(e => e.Id == id);
        }*/
    }
}
