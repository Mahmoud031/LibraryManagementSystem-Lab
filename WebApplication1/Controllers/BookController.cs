using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IWebHostEnvironment _environment;

        public BookController(IBookRepository bookRepository, IWebHostEnvironment environment)
        {
            _bookRepository = bookRepository;
            _environment = environment;
        }

        public IActionResult Index(string? searchText, decimal? minPrice, decimal? maxPrice, int? selectedCategoryId, int page = 1)
        {
            TempData["CurrentDate"] = DateTime.Now.ToString("dddd, MMMM dd, yyyy");

            int pageSize = 5;

            var books = _bookRepository.GetAll(searchText, minPrice, maxPrice, selectedCategoryId, page, pageSize);
            var totalCount = _bookRepository.GetCount(searchText, minPrice, maxPrice, selectedCategoryId);

            var categories = _bookRepository.GetCategorySelectList();
            categories.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "All Categories"
            });

            var vm = new BookIndexViewModel
            {
                Books = books.Select(b => new BookListItemViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    PublishYear = b.PublishYear,
                    AuthorName = b.Author != null ? b.Author.Name : "",
                    CategoryName = b.Category != null ? b.Category.Name : "",
                    Price = b.Price,
                    IsRestricted = b.IsRestricted,
                    Status = b.Status.ToString(),
                    AttachmentsCount = b.Attachments.Count
                }).ToList(),
                Categories = categories,
                SearchText = searchText,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SelectedCategoryId = selectedCategoryId,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                CurrentDate = TempData["CurrentDate"]?.ToString()
            };

            TempData.Keep("CurrentDate");

            return View(vm);
        }

        public IActionResult Details(int id)
        {
            var book = _bookRepository.GetDetails(id);

            if (book == null)
                return NotFound();

            var vm = new BookDetailsViewModel
            {
                Id = book.Id,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                Title = book.Title,
                ISBN = book.ISBN,
                PublishYear = book.PublishYear,
                Price = book.Price,
                IsRestricted = book.IsRestricted,
                Status = book.Status.ToString(),
                AuthorName = book.Author?.Name ?? "",
                CategoryName = book.Category?.Name ?? "",
                CurrentDate = TempData["CurrentDate"]?.ToString(),
                Attachments = book.Attachments.Select(a => new BookAttachmentViewModel
                {
                    Id = a.Id,
                    FileName = a.FileName,
                    FilePath = a.FilePath
                }).ToList()
            };

            TempData.Keep("CurrentDate");

            return View(vm);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var vm = new BookCreateViewModel();
            LoadDropDownData(vm);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BookCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownData(vm);
                return View(vm);
            }

            var book = new Book
            {
                Title = vm.Title,
                ISBN = vm.ISBN,
                PublishYear = vm.PublishYear,
                Price = vm.Price,
                IsRestricted = vm.IsRestricted,
                Status = vm.Status,
                AuthorId = vm.AuthorId,
                CategoryId = vm.CategoryId,
                Attachments = new List<BookAttachment>()
            };

            _bookRepository.Add(book);
            _bookRepository.Save();

            if (vm.Files != null && vm.Files.Any())
            {
                var attachments = await SaveFilesAsync(vm.Files, book.Id);

                foreach (var attachment in attachments)
                {
                    book.Attachments.Add(attachment);
                }

                _bookRepository.Update(book);
                _bookRepository.Save();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book = _bookRepository.GetById(id);

            if (book == null)
                return NotFound();

            var vm = new BookEditViewModel
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                PublishYear = book.PublishYear,
                Price = book.Price,
                IsRestricted = book.IsRestricted,
                Status = book.Status,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                Version = book.Version,
                ExistingAttachments = book.Attachments.Select(a => new BookAttachmentViewModel
                {
                    Id = a.Id,
                    FileName = a.FileName,
                    FilePath = a.FilePath
                }).ToList()
            };

            LoadDropDownData(vm);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownData(vm);
                return View(vm);
            }

            var book = _bookRepository.GetById(vm.Id);

            if (book == null)
                return NotFound();

            book.Title = vm.Title;
            book.ISBN = vm.ISBN;
            book.PublishYear = vm.PublishYear;
            book.Price = vm.Price;
            book.IsRestricted = vm.IsRestricted;
            book.Status = vm.Status;
            book.AuthorId = vm.AuthorId;
            book.CategoryId = vm.CategoryId;
            book.Version = vm.Version;

            if (vm.Files != null && vm.Files.Any())
            {
                DeletePhysicalFiles(book.Attachments.ToList());
                book.Attachments.Clear();

                var newAttachments = await SaveFilesAsync(vm.Files, book.Id);

                foreach (var attachment in newAttachments)
                {
                    book.Attachments.Add(attachment);
                }
            }

            _bookRepository.Update(book);
            _bookRepository.Save();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var book = _bookRepository.GetDetails(id);

            if (book == null)
                return NotFound();

            var vm = new BookDetailsViewModel
            {
                Id = book.Id,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                Title = book.Title,
                ISBN = book.ISBN,
                PublishYear = book.PublishYear,
                Price = book.Price,
                IsRestricted = book.IsRestricted,
                Status = book.Status.ToString(),
                AuthorName = book.Author?.Name ?? "",
                CategoryName = book.Category?.Name ?? "",
                Attachments = book.Attachments.Select(a => new BookAttachmentViewModel
                {
                    Id = a.Id,
                    FileName = a.FileName,
                    FilePath = a.FilePath
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _bookRepository.GetById(id);

            if (book != null)
            {
                DeletePhysicalFiles(book.Attachments.ToList());
                _bookRepository.Delete(book);
                _bookRepository.Save();
            }

            return RedirectToAction("Index");
        }

        private void LoadDropDownData(BookCreateViewModel vm)
        {
            vm.Authors = _bookRepository.GetAuthorSelectList();
            vm.Categories = _bookRepository.GetCategorySelectList();
            vm.Statuses = _bookRepository.GetStatusSelectList();
        }

        private void LoadDropDownData(BookEditViewModel vm)
        {
            vm.Authors = _bookRepository.GetAuthorSelectList();
            vm.Categories = _bookRepository.GetCategorySelectList();
            vm.Statuses = _bookRepository.GetStatusSelectList();
        }

        private async Task<List<BookAttachment>> SaveFilesAsync(List<IFormFile> files, int bookId)
        {
            var attachments = new List<BookAttachment>();

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "books");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            foreach (var file in files)
            {
                if (file.Length <= 0)
                    continue;

                var storedFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var physicalPath = Path.Combine(uploadsFolder, storedFileName);

                using var stream = new FileStream(physicalPath, FileMode.Create);
                await file.CopyToAsync(stream);

                attachments.Add(new BookAttachment
                {
                    FileName = file.FileName,
                    StoredFileName = storedFileName,
                    FilePath = $"/uploads/books/{storedFileName}",
                    ContentType = file.ContentType,
                    BookId = bookId
                });
            }

            return attachments;
        }

        private void DeletePhysicalFiles(List<BookAttachment> attachments)
        {
            foreach (var attachment in attachments)
            {
                var relativePath = attachment.FilePath.TrimStart('/')
                    .Replace("/", Path.DirectorySeparatorChar.ToString());

                var physicalPath = Path.Combine(_environment.WebRootPath, relativePath);

                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
            }
        }
    }
}