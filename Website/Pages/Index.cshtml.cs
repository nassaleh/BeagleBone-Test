using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BeagleBone;

namespace Website.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<PinRecord> records;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            DBHelper dB = new DBHelper();

            StringBuilder sb = new StringBuilder();

            foreach (var pin in dB.GetRecords())
            {
                sb.AppendLine($"ID: {pin.Id}\t|Pin: {pin.Gpio}|\t|Value: {pin.PinValue}\t|Time: {pin.Timestamp}");
            }

            ViewData["Table"] = sb.ToString();
            //records = dB.GetRecords().ToList();
        }
    }
}
