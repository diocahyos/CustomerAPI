using CustomerAPI.DataContext;
using CustomerAPI.Dtos.Customer;
using CustomerAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CustomerAPI.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDataContext _context;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(AppDataContext context, ILogger<CustomerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var data = await _context.Customers.ToListAsync();
                if (data == null || data.Count == 0)
                {
                    _logger.Log(LogLevel.Error, "Get data but still empty");
                    return NotFound();
                }

                var list = new List<CustomerResponseDto>();
                foreach (var customer in data)
                {
                    list.Add(new CustomerResponseDto
                    {
                        Id = customer.Id,
                        Name = customer.Name,
                        Address = customer.Address,
                        Email = customer.Email,
                    });
                }
                return Ok(list);
            }catch(Exception ex) { 
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomerbyId(int id)
        {
            try
            {
                if (id == 0)
                {
                    //_logger.Log(LogLevel.Error, $"Get Custommer with id {id}");
                    //_logger.Log(LogLevel.Information, DateTime.Now.ToString());
                    return BadRequest();
                }

                var data = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
                if (data == null)
                {
                    _logger.Log(LogLevel.Error, $"Get Custommer with id {id} are not found");
                    return NotFound();
                }

                var list = new List<CustomerResponseDto>();
                list.Add(new CustomerResponseDto
                {
                    Id = data.Id,
                    Name = data.Name,
                    Address = data.Address,
                    Email = data.Email,
                });

                return Ok(list);
            }catch (Exception ex) {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostCustomer(CustomerInsertDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //var cek = await _context.Customers.FirstOrDefaultAsync(x => x.Email.ToLower() == dto.Email.ToLower());
                //if (cek != null)
                //{
                //    ModelState.AddModelError("message", "Email already exist");
                //    return BadRequest(ModelState);
                //}

                var data = new Customer()
                {
                    Name = dto.Name,
                    Address = dto.Address,
                    Email = dto.Email,
                };

                await _context.Customers.AddAsync(data);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    var resp = new CustomerResponseDto()
                    {
                        Id = data.Id,
                        Name = data.Name,
                        Address = data.Address,
                        Email = data.Email,
                    };
                    return StatusCode(201, resp);
                }

                return BadRequest();
            } catch(Exception ex) {
                var message = ex.InnerException?.Message != null?ex.InnerException.Message:ex.Message;
                _logger.Log(LogLevel.Error, message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutCustomer(int id, CustomerInsertDto dto)
        {
            try
            {
                var data = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
                if (data == null)
                {
                    return NotFound();
                }

                data.Id = id;
                data.Name = dto.Name;
                data.Address = dto.Address;
                data.Email = dto.Email;
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return Ok(dto);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCustome(int id)
        {
            try
            {
                var data = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
                if (data == null)
                {
                    return NotFound();
                }

                _context.Customers.Remove(data);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return NoContent();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
