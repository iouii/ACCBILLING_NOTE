using BOI_QUO.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using BOI_QUO.Models;

namespace BOI_QUO.Controllers
{
    public class SystemController : Controller
    {
        OCTIIS_WEBAPPEntities1 dbObj = new OCTIIS_WEBAPPEntities1();
        OCT01Entities dbObjUser = new OCT01Entities();
        public ActionResult Index()
        {

            return View();
        }

    
        public ActionResult Login()
        {


            return View();
        }

        [HttpPost]
        public ActionResult Login(string JsonUser)
        {
            string responseString, myString,message;

            JObject loginData = new JObject();
            loginData = JObject.Parse(JsonUser);
            var QuoId = loginData["QuoId"].ToString();
            var QuoPass = loginData["QuoPass"].ToString();
           List<SetPermission> quePer = new List<SetPermission>();
           List<SetPermission> quePurchase = new List<SetPermission>();
           List<SetPermission> queSeales = new List<SetPermission>();
           List<SetPermission> queMain = new List<SetPermission>();
           List<SetPermission> queAdmin = new List<SetPermission>();
            
            using (var client = new WebClient())
            {
                responseString = client.DownloadString("http://192.168.10.8:81/oct/main_encodepassword2.php/?string=" + QuoPass);
            }

            myString = responseString.Replace("\r\n", string.Empty).Trim();

            var queLog = dbObjUser.UserDetails.Where(ud => ud.USE_Account == QuoId && ud.USE_Password == myString).FirstOrDefault();

            if (queLog != null)
            {
                Session["userCode"] = queLog.USE_Usercode.ToString();
                Session["userFname"] = queLog.USE_FName.ToString();
                Session["userLname"] = queLog.USE_LName.ToString();

                var sql = dbObj.Set_Permission
                    .Join(dbObj.System_Permissions, c => c.permissionId, b => b.id.ToString(), (c, b) => new { c, b })
                    .Where(e => e.c.userCode == queLog.USE_Usercode.ToString()).ToList();
                    

                foreach (var data in sql)
                {
                    quePer.Add(new SetPermission { 
                    
                        permissionName = data.b.typePermission,
                        remarkName = data.b.remarkPermission,
                        controllerPermissin = data.b.controllerPermission,
                        
                    
                    });
                }



                foreach (var dataPur in sql.Where(c=> c.b.status.ToString() == "1" ).ToList())
                {
                    quePurchase.Add(new SetPermission
                    {

                        permissionName = dataPur.b.typePermission,
                        remarkName = dataPur.b.remarkPermission,
                        controllerPermissin = dataPur.b.controllerPermission,


                    });
                }

                foreach (var dataSales in sql.Where(c => c.b.status.ToString() == "2").ToList())
                {
                    queSeales.Add(new SetPermission
                    {

                        permissionName = dataSales.b.typePermission,
                        remarkName = dataSales.b.remarkPermission,
                        controllerPermissin = dataSales.b.controllerPermission,


                    });
                }


                foreach (var dataMain in sql.Where(c => c.b.status.ToString() == "4").ToList())
                {
                    queMain.Add(new SetPermission
                    {

                        permissionName = dataMain.b.typePermission,
                        remarkName = dataMain.b.remarkPermission,
                        controllerPermissin = dataMain.b.controllerPermission,


                    });
                }

                foreach (var dataAdmin in sql.Where(c => c.b.status.ToString() == "3").ToList())
                {
                    queAdmin.Add(new SetPermission
                    {

                        permissionName = dataAdmin.b.typePermission,
                        remarkName = dataAdmin.b.remarkPermission,
                        controllerPermissin = dataAdmin.b.controllerPermission,


                    });
                }

                Session["Permission"] = quePer;
                Session["PermissionPurchase"] = quePurchase;
                Session["PermissionSales"] = queSeales;
                Session["PermissionMain"] = queMain;
                Session["PermissionAdmin"] = queAdmin;

                if (quePurchase.Count() > 0)
                {
                    Session["pur"] = "Yes";

                }
                else
                {
                    Session["pur"] = "No";
                }

                if (queSeales.Count() > 0)
                {
                    Session["sa"] = "Yes";

                }
                else
                {
                    Session["sa"] = "No";
                }

                if (queMain.Count() > 0)
                {
                    Session["ma"] = "Yes";

                }
                else
                {
                    Session["ma"] = "No";
                }

                if (queAdmin.Count() > 0)
                {
                    Session["ad"] = "Yes";

                }
                else
                {
                    Session["ad"] = "No";
                }



                var sqll = dbObjUser.EmpLists.Where(c => c.EmpID.ToString() == queLog.USE_Usercode.ToString()).FirstOrDefault();
                Session["position"] = sqll.Position;
                Session["department"] = sqll.DeptDesc;

                message = "1";
              
            }
            else
            {
                message = "0";
            }


            return Json(String.Format(message));
           
        }

       

        public ActionResult PermissionInsert()
        {
            Array insertPermission;
            List<SystemPermission> permissionList = new List<SystemPermission>();

            var quryPermissin = dbObj.System_Permissions.ToList();

            foreach (var pm in quryPermissin)
            {
                permissionList.Add(new SystemPermission { 
                
                id = pm.id,
                typePermission = pm.typePermission,
                remarkPermission = pm.remarkPermission,
                controller = pm.controllerPermission,
                status  = pm.status.ToString()
                
                });
            }

            insertPermission = permissionList.ToArray();

            ViewBag.Permission = insertPermission;

            return View();
        }
        [HttpPost]
        public void PermissionInsertQuery(string permissionRemark, string permissionName, string permissionController, string statusT)
        {
            /*statut 1 = Purchase ,2 = Sales */
            var sql = new System_Permissions()
            {

                typePermission = permissionName,
                remarkPermission = permissionRemark,
                controllerPermission = permissionController,
                status = Convert.ToInt32(statusT)
            };

            dbObj.System_Permissions.Add(sql);
            dbObj.SaveChanges();

           

   
        }
        [HttpPost]
        public void DeletePermission(string permissionId)
        {
            var id = permissionId;

            var sql = dbObj.System_Permissions.Single(permis => permis.id.ToString() == id);

            dbObj.System_Permissions.Remove(sql);
            dbObj.SaveChanges();

        }
        [HttpPost]
        public void DeleteUserPermission(string permissionId)
        {
            var id = permissionId;

            var sql = dbObj.Set_Permission.Single(permis => permis.id.ToString() == id);

            dbObj.Set_Permission.Remove(sql);
            dbObj.SaveChanges();

        }

     
        public ActionResult UserPermissionInsert(string code)
        {
            Array userPermission,sdrdr;
      
            List<string> listToFill = new List<string>();
            List<SetPermission> userPermissionList = new List<SetPermission>();
            List<SystemPermission> userPermissionListque = new List<SystemPermission>();
            var sql = dbObj.Set_Permission
                .Join(dbObj.System_Permissions, sep => sep.permissionId, syp => syp.id.ToString(), (sep, syp) => new { sep, syp })
                .Where(ac => ac.sep.userCode == code)
                .ToList();

            foreach (var data in sql)
            {
                userPermissionList.Add(new SetPermission
                {
                    userCode = data.sep.userCode,
                    id = data.sep.id,
                    permissionName  = data.syp.typePermission

                });

                listToFill.Add(data.syp.id.ToString());

                //userPermissionListque.Add(new SystemPermission
                //{
                //    id = data.syp.id
                   

                //});

            }

            userPermission = userPermissionList.ToArray();
            sdrdr = userPermissionListque.ToArray();
            ViewBag.UserPermis = userPermission;
            
           

            Array insertPermission;
            List<SystemPermission> permissionList = new List<SystemPermission>();

            var quryPermissin = dbObj.System_Permissions.Where(x => !listToFill.Contains(x.id.ToString())).ToList();

            foreach (var pm in quryPermissin)
            {
                permissionList.Add(new SystemPermission
                {

                    id = pm.id,
                    typePermission = pm.typePermission,
                    remarkPermission = pm.remarkPermission

                });
            }

            insertPermission = permissionList.ToArray();

            ViewBag.Permission = insertPermission;

            ViewBag.userCode = code;

            return View();

        }

        [HttpPost]
        public void PermissionUserAdd(string code, string permission)
        {
            string[] PermisUser = permission.Split(',');

            var coi = PermisUser.Count();

            for (var i = 0; i < coi; i++)
            {
                var sql = new Set_Permission()
                {

                    userCode = code,
                    permissionId = PermisUser[i],
                    dateCreate = DateTime.Now.ToString()
                };
                dbObj.Set_Permission.Add(sql);
                dbObj.SaveChanges();

            }
      



        }

        public ActionResult Profile(){
            Array ProfileArray;
            List<UsersProfile> UserProfileList = new List<UsersProfile>();

            var sql = dbObjUser.UserDetails.ToList();

            foreach (var data in sql)
            {

                UserProfileList.Add(new UsersProfile{
            
                    userId = data.USE_Id,
                    userFname = data.USE_FName,
                    userLname = data.USE_LName,
                    userCode =  data.USE_Usercode.ToString(),
                    DepartId = data.Dep_Id.ToString()

            
            
            });
            }

            ProfileArray = UserProfileList.ToArray();

            ViewBag.ProfileList = ProfileArray;


            return View();
        }
        [HttpPost]
        public ActionResult Profile(string profileCode)
        {
            var query = ProfileQuery(profileCode);

            return Json(query, JsonRequestBehavior.AllowGet);
        }


        public List<UsersProfile> ProfileQuery(string profileCode)
        {
            List<UsersProfile> queryPro = new List<UsersProfile>();
            var query = dbObjUser.UserDetails.Where(c => c.USE_Usercode.ToString().Contains(profileCode)).ToList();

            foreach (var data in query)
            {
                queryPro.Add(new UsersProfile {


                    userId = data.USE_Id,
                    userFname = data.USE_FName,
                    userLname = data.USE_LName,
                    userCode = data.USE_Usercode.ToString(),
                    DepartId = data.Dep_Id.ToString()


                
                });
                
            }

            return queryPro;
        }


        public ActionResult EditPermission(string code)
        {
            List<SystemPermission> EditPer = new List<SystemPermission>();
            Array PerEdit;

            var sql = dbObj.System_Permissions.Where(c => c.id.ToString() == code).ToList();

            foreach (var daata in sql)
            {
                EditPer.Add(new SystemPermission { 
                
                id = daata.id,
                typePermission = daata.typePermission,
                remarkPermission = daata.remarkPermission,
                controller = daata.controllerPermission,
                status = daata.status.ToString()
                
                });
            }

            PerEdit = EditPer.ToArray();
            ViewBag.editPermiss = PerEdit;
            ViewBag.codeadat = code;

            return View();
        }
            
        [HttpPost]
        public ActionResult EditPermissionQuery(string codeId, string typePer, string PerCon, string PerRemark, string statusT)
        {
            var sql = dbObj.System_Permissions.Where(c => c.id.ToString() == codeId).FirstOrDefault();

            if(sql.id.ToString() !=null){

                sql.typePermission = typePer.ToString();
                sql.controllerPermission = PerCon.ToString();
                sql.remarkPermission = PerRemark.ToString();
                sql.status = Convert.ToInt32(statusT);

            }
            dbObj.SaveChanges();

            return Json("Succes", JsonRequestBehavior.AllowGet);
        }
    

    



    }
}