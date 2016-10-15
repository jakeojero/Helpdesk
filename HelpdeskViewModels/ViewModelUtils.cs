using System;
using System.Collections.Generic;
using System.Diagnostics;
using HelpdeskDAL;

namespace HelpdeskViewModels
{
    public class ViewModelUtils
    {
        public static void ErrorRoutine(Exception e, string obj, string method)
        {
            if(e.InnerException != null)
            {
                Trace.WriteLine("Error in ViewModels, object=" + obj +
                                ", method=" + method +
                                " , inner exception message=" +
                                e.InnerException.Message);
                throw e.InnerException;
            }
            else
            {
                Trace.WriteLine("Error in ViewModels, object=" + obj +
                                ", method=" + method + " , message=" +
                                e.Message);
                throw e;
            }
        }

        public bool LoadCollections()
        {
            bool createOk = false;

            try
            {
                DALUtils dalUtil = new DALUtils();
                createOk = dalUtil.LoadCollections();
            }
            catch(Exception ex)
            {
                ErrorRoutine(ex, "ViewModelUtils", "LoadCollections");
            }

            return createOk;
        }

    }
}
