using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practice.Model;
using Practice.Models;
using Practice.Repo;

namespace Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly Exam1Context _exam1;
        private readonly IAdmin _admin;
        public AdminController(Exam1Context exam1,IAdmin admin) 
            {
                _exam1 = exam1;
            _admin = admin;
            }


        [HttpPost("Create")]
        public async Task<Response<string>> Create([FromBody] Produ produc)
        {
           
            return await _admin.InsertAdmin(produc);

        }

        [HttpGet("Getone")]
        public async Task<Response<Produ>> Getone(int  id)
        {
            return await _admin.GetOneAd(id);

        }

        [HttpGet("GetALL")]
        public async Task<Response<List<Produ>>> GetAll(int pagenumber,int pagesize)
        {
            return await _admin.GetAllAdmin(pagenumber,pagesize);

        }



    }
}
