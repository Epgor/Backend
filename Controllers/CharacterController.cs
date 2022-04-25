using Microsoft.AspNetCore.Mvc;
using Back.Services;
using Back.Models;
using Back.Entities;
namespace Back.Controllers
{

    [Route("api/character")]
    [ApiController]

    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;
        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;

        }

        [HttpPost]
        public ActionResult CreateCharacter([FromBody] CreateCharacterDto dto)
        {
            _characterService.Create(dto);
            return Created("/api/character/", null);
        }

        [HttpGet("{id}")]
        public ActionResult<Character> Get([FromRoute] int id)
        {
            var character = _characterService.Get(id);

            return Ok(character);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Character>> GetAll([FromQuery]CharacterQuery query)
        {
            var characters = _characterService.GetAll(query);
            return Ok(characters);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateCharacterDto dto, [FromRoute]int id)
        {
            _characterService.Update(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute]int id)
        {
            _characterService.Delete(id);
            return NoContent();
        }
    }

}
