using System;
using dotnetapi.Data;

using dotnetapi.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace dotnetapi.Controllers
{
	[Route("api/VillaAPI")]
	[ApiController]
	public class VillaAPIController: ControllerBase
	{

		[HttpGet]
		public ActionResult<IEnumerable<VillaDTO>> GeVillas()
		{
			return Ok(VillaStore.villaList);

        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> GeVilla(int id)
        {
			if(id == 0)
			{
				return BadRequest();
			}
			var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
			if(villa == null)
			{
				return NotFound();
			} 
            return Ok(villa);

        }


		[HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
		{
			//if(!ModelState.IsValid)
			//{
			//	return BadRequest(ModelState);
			//}
			if(VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
			{

				ModelState.AddModelError("CustomError", "Villa already exist");
				return BadRequest(ModelState);
			}
			{

			}
			if(villaDTO == null)
			{
				return BadRequest(villaDTO);
			}

			if(villaDTO.Id > 0)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
			villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;

			return CreatedAtRoute("GetVilla", new {id = villaDTO.Id }, villaDTO);
		}


		[HttpDelete("{id:int}", Name ="DeleteVilla")]
		public IActionResult DeleteVilla(int id)
		{
			if(id == 0)
			{
				return BadRequest();
			}
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
			if(villa == null)
			{
				return NotFound();
			}

			VillaStore.villaList.Remove(villa);

			return NoContent();
        }


		[HttpPut("{id:int}", Name ="UpdateVilla")]
		public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDTO)
		{
			if(villaDTO == null || id != villaDTO.Id)
			{
				return BadRequest();
			}
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
			villa.Name = villaDTO.Name;
			villa.Sqft = villaDTO.Sqft;
			villa.Occupancy = villaDTO.Occupancy;

			return NoContent();
        }
    }
}

