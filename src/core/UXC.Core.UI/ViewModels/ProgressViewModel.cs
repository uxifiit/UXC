using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.UI;

namespace UXC.Core.ViewModels
{
    public abstract class ProgressViewModel : BindableBase
    {
        private bool isLoading = false;
        public bool IsLoading
        {
            get { return isLoading; }
            protected set { Set(ref isLoading, value); }
        }

        private bool isLoaded = false;
        public bool IsLoaded
        {
            get { return isLoaded; }
            protected set { Set(ref isLoaded, value); }
        }

        private bool isError = false;
        public bool IsError
        {
            get { return isError; }
            protected set
            {
                Set(ref isError, value);
                //if (value == false)
                //{
                //    ErrorMessage = String.Empty;
                //}
            }
        }

        //private string errorMessage = String.Empty;
        //public string ErrorMessage
        //{
        //    get { return errorMessage; }
        //    protected set
        //    {
        //        Set(ref errorMessage, value);
        //        if (String.IsNullOrWhiteSpace(value) == false)
        //        {
        //            IsError = true;
        //        }
        //    }
        //}
    }
}
