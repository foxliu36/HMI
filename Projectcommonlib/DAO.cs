using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projectcommonlib
{
    //singalton
     class DAO
    {
         private static DAO variable = null;

         //SqlConnection conn = new SqlConnection("server=serverName; database=dbName;uid=userName;pwd=userPassword");

         private DAO() { }

         public static DAO getInstence() {

             if (variable == null)
             {
                 return new DAO();
             }
             else
             {
                 return variable;
             }
         }


    }
}
