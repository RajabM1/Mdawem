using System;
using System.Collections.Generic;
using System.Text;

namespace MdawemApp.Models
{
    public class VactionRequestModel
    {
        public string Type { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Reason { get; set; }
        public string UserID { get; set; }
        public string Status { get; set; }


        public string Dateofrequest { get; set; }

        public VactionRequestModel()
        {
            Status = "Awaiting";
            Dateofrequest = DateTime.Today.ToString("dd/MM/yyyy");

        }

    }

}
