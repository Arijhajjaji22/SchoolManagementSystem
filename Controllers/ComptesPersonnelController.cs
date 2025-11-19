using Microsoft.AspNetCore.Mvc;
using ProjetBrima.Models;
using ProjetBrima.Services;

namespace ProjetBrima.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComptesPersonnelController : ControllerBase
    {
        private readonly IComptesPersonnelService _service;

        public ComptesPersonnelController(IComptesPersonnelService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComptesPersonnel>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComptesPersonnel>> Get(int id)
        {
            var personnel = await _service.GetByIdAsync(id);
            if (personnel == null) return NotFound();
            return personnel;
        }

        [HttpPost]
        public async Task<ActionResult<ComptesPersonnel>> Create(ComptesPersonnel personnel)
        {
            var created = await _service.AddAsync(personnel);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ComptesPersonnel personnel)
        {
            var updated = await _service.UpdateAsync(id, personnel);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }

}
