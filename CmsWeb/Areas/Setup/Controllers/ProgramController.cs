using System.Linq;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Areas.Setup.Controllers
{
    [Authorize(Roles = "Admin")]
    [RouteArea("Setup", AreaPrefix = "Program"), Route("{action}/{id?}")]
    public class ProgramController : CmsStaffController
    {
        [Route("~/Programs")]
        public ActionResult Index()
        {
            var m = from p in DbUtil.Db.Programs
                    orderby p.RptGroup, p.Name
                    select p;
            return View(m);
        }

        [HttpPost]
        public ActionResult Create()
        {
            var p = new Program { Name = "new program" };
            DbUtil.Db.Programs.InsertOnSubmit(p);
            DbUtil.Db.SubmitChanges();
            return Redirect("/Programs/#{0}".Fmt(p.Id));
        }

        [HttpPost]
        public ContentResult Edit(string id, string value)
        {
            var a = id.Split('.');
            var c = new ContentResult();
            c.Content = value;
            var p = DbUtil.Db.Programs.SingleOrDefault(m => m.Id == a[1].ToInt());
            if (p == null)
                return c;
            switch (a[0])
            {
                case "ProgramName":
                    p.Name = value;
                    break;
                case "RptGroup":
                    p.RptGroup = value;
                    break;
                case "StartHours":
                    p.StartHoursOffset = value.ToDecimal();
                    break;
                case "EndHours":
                    p.EndHoursOffset = value.ToDecimal();
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return c;
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            id = id.Substring(1);
            var p = DbUtil.Db.Programs.SingleOrDefault(m => m.Id == id.ToInt());
            if (p == null)
                return new EmptyResult();

            foreach (var d in p.Divisions)
                d.ProgId = null;
            DbUtil.Db.ProgDivs.DeleteAllOnSubmit(p.ProgDivs);
            DbUtil.Db.Programs.DeleteOnSubmit(p);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
    }
}
