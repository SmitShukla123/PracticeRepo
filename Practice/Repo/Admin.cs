using Microsoft.EntityFrameworkCore;
using Practice.Model;
using Practice.Models;

namespace Practice.Repo
{
    public class Admin:IAdmin
    {
        private readonly Exam1Context _exam1Context;
        public Admin(Exam1Context exam1Context)
        {
            _exam1Context = exam1Context;
        }
        public async Task<Response<string>> InsertAdmin(Produ produc)
        {
            Response<string> res = new();
            try
            {
                if (produc == null)
                {
                    res.IsSuccess = false;
                    res.Status = Response_Status.Fail;
                    res.Error_Message = "Please a Enter a details";
                    return res;
                }
                var exit = await _exam1Context.Products.Where(x => x.Pname == produc.pname && x.Isdeleted == false).Select(xm => xm).FirstOrDefaultAsync();
                if (exit != null)
                {
                    res.IsSuccess = false;
                    res.Status = Response_Status.Fail;
                    res.Error_Message = "Product Name is already exits";
                    return res;
                }

                var catid = await _exam1Context.Categories.Where(x => x.Cid == produc.cid).FirstOrDefaultAsync();
                if (catid == null)
                {
                    res.IsSuccess = false;
                    res.Status = Response_Status.Fail;
                    res.Error_Message = "Category Not a Exits";
                    return res;
                }

                Product data = new()
                {
                    Pname = produc.pname,
                    Pimage = produc.pimage,
                    Pcost = produc.pcost,
                    Astock = produc.astock,
                    Cid = produc.cid,
                    Istock = produc.istock,
                    Isdeleted = true,
                    CreateAt = DateTime.Now,
                    IsEnable = false,
                };

                _exam1Context.Products.Add(data);
                _exam1Context.SaveChanges();
                res.IsSuccess = true;
                res.Status = Response_Status.Success;
                res.Error_Message = "Product Add Succefully";
                return res;

            }

            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Status = Response_Status.Fail;
                res.Error_Message = "Something Wrong";
                return res;
            }

        }
        public async Task<Response<Produ>> GetOneAd(int id)
        {
            try
            {
                Product pro = await _exam1Context.Products.Where(x => x.Pid == id && x.Isdeleted == false).FirstOrDefaultAsync();
                if (pro != null)
                {
                    Produ pe = new()
                    {
                        pid = pro.Pid,
                        pname = pro.Pname,
                        astock = pro.Astock,
                        cid = pro.Cid,
                        istock = pro.Istock,
                        pcost = pro.Pcost,
                        pimage = pro.Pimage,

                    };
                    return new Response<Produ>
                    {
                        IsSuccess = true,
                        Status = Response_Status.Success,
                        Success_Message = "Fetch Data Succefully",
                        Data = pe
                    };
                }
                return new Response<Produ>
                {
                    IsSuccess = false,
                    Status = Response_Status.Fail,
                    Error_Message = "Envalid Id",

                };
            }
            catch (Exception ex)
            {
                return new Response<Produ>
                {
                    IsSuccess = false,
                    Status = Response_Status.Fail,
                    Error_Message = ex.Message,
                    Display_Error_Message = "Something Wrong",
                };
            }
        }
        public async Task<Response<List<Produ>>> GetAllAdmin(int pagenumber, int pagesize)
        {
            try
            {
                //List<Product> pro = await _exam1Context.Products.Where(x => x.Isdeleted == false).Select(xm => xm).ToListAsync();
                var query = _exam1Context.Products.Where(x => x.Isdeleted == false).AsNoTracking();
                int totalRecords = await query.CountAsync();
                List<Product> pro = await query.Skip((pagenumber - 1) * pagesize).Take(pagesize).ToListAsync();
                List<Produ> data = pro.Select(item => new Produ
                {
                    pid = item.Pid,
                    astock = item.Astock,
                    cid = item.Cid,
                    istock = item.Istock,
                    pcost = item.Pcost,
                    pname = item.Pname,
                    pimage = item.Pimage,
                    createAt = item.CreateAt,
                    isdeleted = item.Isdeleted,
                }).ToList();

                if (pro.Count > 0)

                {
                    return new Response<List<Produ>>
                    {
                        IsSuccess = true,
                        Status = Response_Status.Success,
                        Data = data,
                        PageSize = pagesize,
                        TotalRecords = totalRecords,
                        PageNumber = pagenumber

                    };
                }
                return new Response<List<Produ>>
                {
                    IsSuccess = false,
                    Status = Response_Status.Fail,
                    Error_Message = "DataNot Avilable",
                };
            }
            catch (Exception ex)
            {
                return new Response<List<Produ>>
                {
                    IsSuccess = false,
                    Status = Response_Status.Fail,
                    Error_Message = ex.Message,
                    Display_Error_Message = "Something Wrong",
                };
            }
        }
    }


}
