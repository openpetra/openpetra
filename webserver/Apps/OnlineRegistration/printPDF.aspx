<%@ Page Language="C#" %>
<%@ Assembly Name="Ict.Common" %>
<%@ Assembly Name="Ict.Common.IO" %>
<%@ Assembly Name="Ict.Common.Printing" %>
<%@ Assembly Name="PdfSharp" %>
<%@ Assembly Name="Jayrock.Json" %>
<%@ Assembly Name="Ict.Petra.Server.lib.MPartner" %>
<%@ Import Namespace="Ict.Common" %>
<%@ Import Namespace="Ict.Common.IO" %>
<%@ Import Namespace="Ict.Common.Printing" %>
<%@ Import Namespace="PdfSharp" %>
<%@ Import Namespace="PdfSharp.Drawing" %>
<%@ Import Namespace="PdfSharp.Pdf" %>
<%@ Import Namespace="PdfSharp.Pdf.IO" %>
<%@ Import Namespace="System.Drawing.Printing" %>
<%@ Import Namespace="System.Net.Mail" %>
<%@ Import Namespace="Jayrock.Json" %>
<%@ Import Namespace="Jayrock.Json.Conversion" %>
<%@ Import Namespace="Ict.Petra.Server.MPartner.Import" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web" %>

<%
	
new TAppSettingsManager(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "web.config");
new TLogging(TAppSettingsManager.GetValue("Server.LogFile"));

// eg. content of jsondata.txt:
// {"RegistrationOffice":"43000000","RegistrationCountryCode":"en-US","EventIdentifier":"asdf.23","Role":"TS-TEEN","LastName":"asdf","FirstName":"asdf","NickName":"asf","Street":"asfd","Postcode":"asdf234","City":"asfd","Country":"GB","Phone":"asdf","Mobile":"asdf","Email":"sample@example.org","DateOfBirth":"27/11/1978","Gender":"Male","ImageID":"6616b230b605d369afd4697466b9f2ba.jpg","MedicalNeeds":"","EmergencyFirstName":"asdf","EmergencyLastName":"asdf","EmergencyRelationship":"Parent","EmergencyPhone":"asdf","ExistingPartnerKey":"If you cannot find it, just leave this empty.","Vegetarian":"No","TShirtStyle":"M (Boys Cut)","TShirtSize":"M (Medium)","Travel":"asdf","LegalImprint":"on"}";

StreamReader sr = new StreamReader("jsondata.txt");
string AJSONFormData=sr.ReadToEnd();
sr.Close();
TLogging.Log(AJSONFormData);
				Ict.Common.Catalog.Init("en-GB", "en-GB");
                
                // will set the correct language code for parsing dates in the json data string
                string RequiredCulture = string.Empty;
                AJSONFormData = TJsonTools.RemoveContainerControls(AJSONFormData, ref RequiredCulture);
                
                TApplicationFormData data = (TApplicationFormData)JsonConvert.Import(typeof(TApplicationFormData),
                    AJSONFormData);
				Int64 NewPersonPartnerKey = 815;
                data.RawData = AJSONFormData;

				string pdfIdentifier;
                string pdfFilename = TImportPartnerForm.GeneratePDF(NewPersonPartnerKey, data.registrationcountrycode, data, out pdfIdentifier);
                try
                {
                   // TImportPartnerForm.SendEmail(NewPersonPartnerKey, data.registrationcountrycode, data, pdfFilename);
                }
                catch (Exception e)
                {
                    TLogging.Log(e.Message);
                    TLogging.Log(e.StackTrace);
                }
				
				Response.Write("<a href=\"downloadPDF.aspx?pdf-id=" + pdfIdentifier + "\">Download PDF</a>");
%>