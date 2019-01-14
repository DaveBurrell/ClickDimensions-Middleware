using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace FormCapture
{
    /// <summary>
    /// FormCapture
    /// </summary>
    public class form : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string url = String.Empty;

            this.WriteTraceHeader();

            Enquiry enquiry = new Enquiry(context);
            string responsefromfc = String.Empty;

            TraceProcessor.Verbose(string.Format("hp = \"{0}\"", enquiry.hp));

            if (enquiry.hp.Equals(String.Empty))
            {
                try
                {
                    // Post form to Click Dimensions
                    responsefromfc = AddClick(enquiry);
                }
                #region Error Trapping
                catch (ApplicationException ex)
                {
                    //Non fatal error, ignore.
                    TraceProcessor.Warning(string.Format("WARNING: {0}", ex.Message));
                }
                catch (Exception ex)
                {
                    TraceProcessor.Error(string.Format("EXCEPTION: {0}", ex.ToString()));
                    responsefromfc = string.Format("EXCEPTION: {0}", ex.Message);
                }
                #endregion Error Trapping

                TraceProcessor.Information(string.Format("responsefromfc = \"{0}\"", responsefromfc));

                #region Compose redirect URL

                if (responsefromfc.Equals("success") && enquiry.enquiry_type.Equals("CustomForm Form"))
                {
                    url = string.Format("{0}?success=1", enquiry.return_url);
                }
                else if (responsefromfc.Equals("error") && enquiry.enquiry_type.Equals("CustomForm Form"))
                {
                    url = string.Format("{0}?success=0", enquiry.return_url);
                }

                // Default case
                else
                {
                    url = string.Format("{0}?success=0", enquiry.return_url);
                }

                #endregion Compose redirect URL
            }
            else
            {
                // Copmpose redirect URL
                url = string.Format("{0}?success=spam", enquiry.return_url);
            }

            // Redirect.
            TraceProcessor.Information(string.Format("Redirect: {0}", url), true);
            context.Response.Redirect(url);
        }  

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region Private Methods

        /// <summary>
        /// Post form to Click Dimensions
        /// </summary>
        private string AddClick(Enquiry FormEnquiry)      
        {
            if (!FormEnquiry.hp.Equals(String.Empty))
                throw new ApplicationException("Robot check failed.  hp field was not empty.");

            #region Build Form Data Request

            string formCaptureUrl = null;
            string visitorKey = FormEnquiry.visitkey;
            string postData = null;

            switch (FormEnquiry.enquiry_type)
            {    
                case "CustomForm Form":
                    //Form Capture String Generated
                    formCaptureUrl = Properties.Settings.Default.CustomForm_FormCaptureURL;
                    postData = String.Format($"{FormEnquiry.ToPostDataString()}");
                    break;

                default:
                    throw new ApplicationException("Unexpected Enquiry Type.");
            }


            TraceProcessor.Verbose(string.Format("formCaptureUrl = \"{0}\"", formCaptureUrl));
            TraceProcessor.Verbose(string.Format("postData = \"{0}\"", postData));

            #endregion Build Form Data Request

            // Post data to Click Dimensions Server
            string serverResponse = this.PostToServer(formCaptureUrl, postData);

            return serverResponse;
        }

        /// <summary>
        /// Post the form data to Click Dimensions.
        /// </summary>
        /// <returns>Click Dimensions Response.</returns>
        private string PostToServer(string FormCaptureURL, string PostData)
        {
            try
            {
                string serverResponse = String.Empty;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FormCaptureURL);
                // Set the Method property of the request to POST.
                request.Method = "POST";
                // Create POST data and convert it to a byte array.
                byte[] byteArray = Encoding.UTF8.GetBytes(PostData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // set referer
                request.Referer = Properties.Settings.Default.Referrer;
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse response = request.GetResponse();
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                serverResponse = reader.ReadToEnd();
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();

                return serverResponse;
            }
            catch
            {
                throw;
            }
        }

        
        /// <summary>
        /// Write out the Trace Level of the application.
        /// </summary>
        private void WriteTraceHeader()
        {
            TraceProcessor.Error(String.Format("========================== START {0} ==========================", DateTime.Now.ToString()));
            TraceProcessor.Error("-------------------------- TraceLevelSwitch - Error Tracing Enabled");
            TraceProcessor.Warning("-------------------------- TraceLevelSwitch - Warning Tracing Enabled");
            TraceProcessor.Information("-------------------------- TraceLevelSwitch - Info Tracing Enabled");
            TraceProcessor.Verbose("-------------------------- TraceLevelSwitch - Verbose Tracing Enabled", true);
        }

        #endregion Private Methods
    }
}