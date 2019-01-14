using System.Text;
using System.Web;
namespace FormCapture
{
    public class Enquiry
    {
        #region Ctor

        public Enquiry(HttpContext Context)
        {
            TraceProcessor.Verbose("---------------------------------------------");
            TraceProcessor.Verbose(string.Format("enquiry_type = \"{0}\"", Context.Request["enquiry_type"]));
            enquiry_type = Context.Request["enquiry_type"];
         
            TraceProcessor.Verbose(string.Format("firstname = \"{0}\"", Context.Request["firstname"]));
            firstname = Context.Request["firstname"];

            TraceProcessor.Verbose(string.Format("lastname = \"{0}\"", Context.Request["lastname"]));
            lastname = Context.Request["lastname"];

            TraceProcessor.Verbose(string.Format("email = \"{0}\"", Context.Request["email"]));
            email = Context.Request["email"];

            TraceProcessor.Verbose(string.Format("visitkey = \"{0}\"", Context.Request["visitkey"]));
            visitkey = Context.Request["visitkey"];

            TraceProcessor.Verbose(string.Format("return_url = \"{0}\"", Context.Request["return_url"]));
            return_url = Context.Request["return_url"];

           TraceProcessor.Verbose(string.Format("hp = \"{0}\"", Context.Request["hp"]));
            hp = Context.Request["hp"];
            TraceProcessor.Verbose("---------------------------------------------");

        }

        #endregion Ctor

        #region Methods

        public string ToPostDataString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat($"enquiry_type={enquiry_type.ToEncodedString()}");
            sb.AppendFormat($"&firstname={firstname.ToEncodedString()}");
            sb.AppendFormat($"&lastname={lastname.ToEncodedString()}");
            sb.AppendFormat($"&email={email.ToEncodedString()}");
            sb.AppendFormat($"&cd_visitorkey={visitkey.ToEncodedString()}");
            sb.AppendFormat($"&return_url={return_url}"); // DO not encode.
            return sb.ToString();
        }

        #endregion Methods

        #region Properties

        public string enquiry_type { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string visitkey { get; set; }
        public string return_url { get; set; }
        public string enquiry_category { get; set; }
        public string hp { get; set; }
        #endregion Properties
    }
}