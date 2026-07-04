using BusinessLogicLayer;
using CommonDataLayer.DTO;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentsManagement.Ntier.Controllers
{
    [ApiController]
    public class BasesController<T> : ControllerBase where T : BaseEntity
     {
        private readonly IBaseBL<T> _baseBL;

        public BasesController(IBaseBL<T> baseBL) => _baseBL = baseBL;

        [HttpPost("filter")]
        public IActionResult FilterData([FromBody] FilterData filterData)
        {
            try
            {
                IEnumerable<T> list = _baseBL.FilterData(filterData);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_baseBL.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add([FromBody] T data)
        {
            try
            {
                Guid id = _baseBL.Add(data);
                return CreatedAtAction(nameof(Add), id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            try
            {
                T result = _baseBL.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] T data)
        {
            try
            {
                Guid getId = _baseBL.Update(id, data);
                return Ok(getId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("delete")]
        public IActionResult Delete([FromBody] List<Guid> ids)
        {
            try
            {
                int count = _baseBL.Delete(ids);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("restore")]
        public IActionResult Restore([FromBody] List<Guid> ids)
        {
            try
            {
                int count = _baseBL.Restore(ids);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("delete-hard")]
        public IActionResult DeleteHard([FromBody] List<Guid> ids)
        {
            try
            {
                int count = _baseBL.DeleteHard(ids);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
