using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.IO;
using System.Numerics;
using System.Security.Policy;
using UserProfileWebAPI.Data;
using UserProfileWebAPI.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserProfileWebAPI.Controllers
{
    //[Route("api/[controller]")]
    //[Route("api/[controller]/[action]")]
    [Route("[controller]/[Action]")]
    [ApiController]
    public class InputTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InputTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. FromRoute(URL Path Parameters) : Used to extract values from the URL route.
        // Request URL : GET /inputtypes/GetById/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var user = await _context.Users.FindAsync(id); // find only works for Primary key attribute
            //var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id); // OR
            //var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == id); // OR

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
            // return Ok($"Received ID: {id}");
        }

        // 1.1 FromRoute(URL Path Parameters) : Used to extract values from the URL route.
        // Request URL : GET /inputtypes/GetById2/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById2(Guid id) // Automatically from route
        {
            var user = await _context.Users.FindAsync(id); // find only works for Primary key attribute
            //var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id); // OR
            //var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == id); // OR

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
            // return Ok($"Received ID: {id}");
        }

        // 1.2 Request URL : GET /inputtypes/GetById3/{id}
/*      [HttpGet]
        public async Task<IActionResult> GetById3([FromRoute] Guid id) // ---------> this doesnt work
        {
        }*/


        // 2. FromQuery(Query Parameters) : Used to extract values from the query string.
        // Request URL : GET /inputtypes/GetByQuery?name={name}
        [HttpGet]
        public async Task<IActionResult> GetByQuery([FromQuery] Guid id)
        {
            var user = await _context.Users.FindAsync(id); 
            //var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id); // OR
            //var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == id); // OR

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
            // return Ok($"Received Id: {id}");
        }


        // 2.1 FromQuery(Query string Parameters) : Used to extract values from the query string.
        // Request URL : GET /api/products? name = phone
        [HttpGet]
        public async Task<IActionResult> GetByQuery2([FromQuery] string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == name); // OR
            //var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == id); // OR

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
            // return Ok($"Received Name: {name}");
        }


        // 2.2 Default Binding (Without Attributes) : Used to extract values from the query string.
        // Request URL : GET /api/products? name = phone
        [HttpGet]
        public async Task<IActionResult> GetByQuery3(string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == name); // OR
            //var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == id); // OR

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
            // return Ok($"Received Name: {name}");
        }


        // 3. FromBody(Request Body - JSON/XML) : Used for complex objects sent in the request body.
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        // public async Task<ActionResult<User>> PostUser(User user)
        {        
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
            // return Ok($"Received Product: {product.Name}");
        }
        // Request Body (JSON):

        // 4. FromHeader(HTTP Headers) : Used to extract values from request headers.
        [HttpGet]
        public IActionResult GetFromHeader([FromHeader(Name = "Authorization")] string token)
        {
            return Ok($"Received Token: {token}");
        }
        // Request Header: Authorization: Bearer some_token_value



        // 5. (Form Data - multipart/form-data) : Used to accept form data, often for file uploads.
        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            return Ok($"Received File: {file.FileName}");
        }

        // 6. FromForm : Basic Form Data Handling coming from HTML form submission
        // (Can be applied to individual parameters or entire models.)
        [HttpPost("register")]
        public IActionResult RegisterUser([FromForm] string username, [FromForm] string email)
        {
            return Ok($"User {username} with email {email} registered successfully.");
        }

        // 6.1 : Using [FromForm] with a Model 
        [HttpPost]
        public async Task<IActionResult> PostRoleUsingForm([FromForm] Role role)
        {
            return Ok($"Role {role} registered successfully.");
        }


        // 7 : When using HTTP headers → Use [FromHeader]
        /*
         *  Common Use Cases for [FromHeader] :  Content-Type
            Authorization: Extracting JWT tokens or API keys from headers.
            Custom Headers: Reading custom headers like X-Request-ID for logging or tracing., 
            User-Agent: Identifying the type of client making the request (e.g., browser or mobile app).,  X-Request-ID, X-Session-ID, X-Correlation-ID, etc.
            
        [FromHeader] binding works only with request headers; it cannot bind data from query parameters or body.
         */
        // 7.1 Extracting Authorization Token
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromHeader] string authorization)
        {
            if (string.IsNullOrEmpty(authorization))
            {
                return BadRequest("Authorization header missing.");
            }

            // Process the token
            var token = authorization.Replace("Bearer ", string.Empty);
            return Ok($"Received JWT Token: {token}");
        }

        // 7.2 Reading a Custom Header : Let’s say you want to read a custom header like X-Correlation-ID, often used for tracing requests across systems.
        [HttpPost("track")]
        public IActionResult TrackRequest([FromHeader] string xCorrelationId)
        {
            if (string.IsNullOrEmpty(xCorrelationId))
            {
                return BadRequest("Correlation ID is missing.");
            }

            // Log the Correlation ID or use it for tracing
            return Ok($"Request tracked with Correlation ID: {xCorrelationId}");
        }

        // 6. FromServices (Dependency Injection) : Used to inject services directly into the controller action.
        /*        [HttpGet]
                public IActionResult GetService([FromServices] IProductService productService)
                {
                    var product = productService.GetProduct();
                    return Ok(product);
        }*/
    }
}