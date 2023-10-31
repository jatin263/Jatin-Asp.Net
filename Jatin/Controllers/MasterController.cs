using Microsoft.AspNetCore.Mvc;
using Jatin.CommonFunction;
using Jatin.Data;

namespace Jatin.Controllers
{
    public class MasterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetMasterData(string typeOfData,string methodAction,string subid,string pageNum,string itemsRow,string searchText)
        {
            string dataJson;
            dataJson = "{}";
            switch (typeOfData)
            {
                case "Division":
                    dataJson = CommonFunction.CommonFunction.DataTableToJSON(DataSP.GetAllDivision());
                    break;
                case "District":
                    dataJson = CommonFunction.CommonFunction.DataTableToJSON(DataSP.GetDistrict(Convert.ToInt32(subid)));
                    break;
                case "Block":
                    dataJson=CommonFunction.CommonFunction.DataTableToJSON(DataSP.GetBlock(methodAction, Convert.ToInt32(subid),Convert.ToInt32(pageNum)*Convert.ToInt32(itemsRow),Convert.ToInt32(itemsRow),searchText));    
                    break;
                case "BlockCount":
                    
                    break;
                default:
                    break;


            }
            //if(typeOfData == "Division")
            //{
            //    dataJson = CommonFunction.CommonFunction.DataTableToJSON(DataSP.GetAllDivision());
            //}
            return Json(new { dataJson });
        }
    }
}
