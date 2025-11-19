using Microsoft.AspNetCore.Mvc;
using ProjetBrima.Models;
using ProjetBrima.Services;

namespace ProjetBrima.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalleController : ControllerBase
    {
        private readonly ISalleService _salleService;

        public SalleController(ISalleService salleService)
        {
            _salleService = salleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var salles = await _salleService.GetAllSallesAsync();
            return Ok(salles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var salle = await _salleService.GetSalleByIdAsync(id);
            if (salle == null) return NotFound();
            return Ok(salle);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Salle salle)
        {
            var createdSalle = await _salleService.AddSalleAsync(salle);
            return Ok(createdSalle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] Salle salle)
        {
            var updatedSalle = await _salleService.UpdateSalleAsync(id, salle);
            if (updatedSalle == null) return NotFound();
            return Ok(updatedSalle);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _salleService.DeleteSalleAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }

}
