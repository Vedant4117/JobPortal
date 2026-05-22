using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public int JobSeekerId { get; set; }
        public int JobId { get; set; }
        public DateTime AppliedDate {  get; set; }
        public string? Status {  get; set; }
        public DateOnly CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
