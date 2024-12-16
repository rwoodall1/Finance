using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.Text;
using ApiBindingModels;
using Core;
using Utilities;
namespace Services
{
    public class EmailHelper
    {
        public string BuildBody()
        {

            //    string vHtml = @"<table style=width:900px>
            //  <tr>
            //    <th colspan=4>Order Details</th>
            //  </tr>
            //<br/>
            //  <tr>
            //    <th>
            //      Product
            //    </th>
            //    <th style=width:35%>
            //      Information
            //    </th>
            //    <th style=width:35%>
            //      Quantity
            //    </th>
            //    <th style=width:25%>
            //      Price
            //    </th>
            //  </tr><tbody>";
            //    string rows = "";
            //    foreach (var item in Items)
            //    {

            //        string row = @"<tr>
            //        <td style=text-align:center>" + item.ItemTitle+ @"</td><br/>

            //    <td style=text-align:center>
            //        <b>" + item.ItemTitle + @"</b><br/><br/>
            //      <table style = width:900px;text-align:center>
            //    <tr >
            //        <td style = text-align:right> First Name:</td>
            //        <td style = text-align:left>" + item.ChildInformation.FullName + @"</td>

            //    </tr>

            //    <tr>
            //        <td style=text-align:right> Class:</td>
            //        <td style =text-align:left>" + item.ChildInformation.Class + @"</td>

            //    </tr>
            //    <tr>
            //        <td style=text-align:right> Teacher:</td>
            //        <td style=text-align:left>" + item.ChildInformation.Teacher + @"</td>

            //    </tr>"+
            //          @" <tr><td style = text-align:right> Msg:</td><td style = text-align:left>" + item.PersMsg +@"</td></tr>" 
            //         +@"</table>
            //        </td>
            //        <td style=text-align:center>"
            //            + item.Quantity+ @"
            //        </td>
            //      <td style=text-align:center>$" +
            //        item.ItemTotal
            //      +@"</td></tr> ";
            //        rows+=rows+ row;
            //    }
            //    string vHtml2 = @"<tr>
            //      <td colspan=2>
            //      </td>
            //      <td style=text-align:right>
            //        Subtotal:
            //      </td>
            //      <td style=text-align:center>$" +
            //        subTotal
            //         + @"
            //      </td>
            //    </tr>
            //    <tr>
            //      <td colspan =2>
            //       </td>

            //       <td style = text-align:right >
            //           Tax:
            //      </td>
            //      <td style=text-align:center>$" +
            //     tax 
            //      + @"</td>
            //    </tr>
            //    <tr>
            //      <td colspan=2>
            //     </td> 
            //       <td  style=text-align:right>
            //           Total:
            //      </td>
            //      <td style=text-align:center>$" +
            //        total 
            //      +@"</td>
            //    </tr>
            //  </tbody>
            //</table>";
            //    return vHtml+ rows +vHtml2  ;
            return "";
        }
        //+ item.Icon1 != null? "<tr><td>Icon 1:</td><td><div>" + item.Icon1.Name + "-" + item.Icon1.Id + @"</div></td>":""+
            //  item.Icon2 != null? "<tr><td>Icon 2:</td><td><div>" + item.Icon2.Name + "-" + item.Icon2.Id + @"</div></td>":""+
            //  item.Icon3 != null? "<tr><td>Icon 3:</td><td><div>" + item.Icon3.Name + "-" + item.Icon3.Id + @"</div></td>":""+
            //   item.Icon4 != null? "<tr><td>Icon 4:</td><td><div>" + item.Icon4.Name + "-" + item.Icon4.Id + @"</div></td>":""+

      
        public string BuildEmail(string Body)
        {
            var template=@"
              <style>
			    html, body, div, span, applet, object, iframe, h1, h2, h3, h4, h5, h6, p, blockquote, pre, a, abbr, acronym, address, big, cite, code, del, dfn, em, font, img, ins, kbd, q, s, samp, small, strike, strong, sub, sup, tt, var, dl, dt, dd, ol, ul, li, fieldset, form, label, legend, table, caption, tbody, tfoot, thead, tr, th, td {
			    margin:0; padding:0; border:0; outline:0; font-weight:inherit; font-style:inherit; font-family:inherit; font-size:100%; vertical-align:baseline; }
			    td {vertical-align:top}
			    h1,h2,h3,h4,h5,h6,strong {font-weight:bold;} h1,h2,h3,h4,h5,h6 {margin:.25em 0;}
			    h1 {font-size:120%;} h2 {font-size:110.0%;} h3 {font-size:108%;} 	h4 {font-size:100%;}
			    em{font-style:italic}
			    ul {margin:5px 0px 0px 35px;padding-left:0px;} ol {;margin:5px 0px 0px 35px;padding-left:0px;} li {padding-bottom:5px}
			    sup {vertical-align:top}
			    table {border-collapse:collapse;border-spacing:0;}
			    fieldset,img {border:0;}
			    hr{height:1px;margin-bottom:5px;border-width:0px}
			    /* Font Settings *//* 10px=77% | 11px=85% | 12px=93% | 13px=100% | 14px=108% | 15px=116% | 16px=123.1% | 17px=131% | 18px=138.5% | 19px=146.5% | 20px=153.9% | 21px=161.6% | 22px=167% | 23px=174% | 24px=182% | 25px=189% | 26px=197  */
			    body{font:13px/1.231 arial,helvetica,clean,sans-serif;*font-size:small;*font:x-small;}
			    select,input,button,textarea,button{font:85% arial,helvetica,clean,sans-serif;}
			    table{font-size:inherit;font:100%;}
			    pre,code,kbd,samp,tt{font-family:monospace;*font-size:108%;line-height:100%;}
			    /* Template Settings */
			    a, a:visited, a:hover, a:active {color:#652d8a}
			    .right{text-align:right;}
			    .center{text-align:center;}
		    </style>
			<table border='0' cellspacing='0' cellpadding='0' width='1024' align='center' style='max-width: 600px;border:1px solid #333'>
				<tbody>
					<tr>
						<td style='background:#fff;width:10px' width='10'>&nbsp;</td>
						<td style='background:#fff; color:#FFFFFF;padding:10px'><span style='padding:10px;text-align:left;background:#fff; color:#FFFFFF;vertical-align:bottom;font-size:12px;'><a href='https://www.memorybook.com' alt='Lifeline Services' title='Lifeline Services'><img src='http://www.memorybook.com/images/logos/mem_jost_logo280x70.jpg' alt='Memory Book Company' title='Memory Book Company' style='border:0px' border='0' width='141' height='48'></a></span></td>
						<td style='padding:10px;text-align:right;background:#fff; color:#FFFFFF;vertical-align:bottom;font-size:12px;' width='230'>&nbsp;</td>
						<td style='background:#fff;width:10px;' width='10'>&nbsp;</td>
					</tr>
                    <tr><td colspan='4' style='height:20px;background:#5D7B9D'></td></tr>
					<tr>
						<td style='background:#fff;'>&nbsp;</td>
						<td colspan='2' style='padding: 20px;background-color:#FFFFFF;color:#333333'>
							<div style='font-size:18px; line-height:130%;font:arial,helvetica,clean,sans-serif;'>"
                                + Body
                            + @"</div>
						</td>
						<td style='background:#fff;'>&nbsp;</td>
					</tr>
				</tbody>
			</table>
            ";

            return template;
        }

        public ApiProcessingResult SendEmail(string Subject, string ToAddresses,  string Body, string BCCAddresses = null, OutlookAttachement attachment = null)
        {
            var processingResult = new ApiProcessingResult<string>() { IsError = false, Data = "Success" };
            var brandedHtml = BuildEmail(Body);
            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = ApplicationConfig.SMTPConfig.Server;
                    //smtpClient.Port = ApplicationConfig.SMTPConfig.Port;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtpClient.UseDefaultCredentials = false;
                    //smtpClient.Credentials = new NetworkCredential(ApplicationConfig.SMTPConfig.User, ApplicationConfig.SMTPConfig.Password);
                    //smtpClient.EnableSsl = true;
                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(ApplicationConfig.SMTPConfig.From);
                        mailMessage.Subject = Subject;
                        mailMessage.Body = brandedHtml;
                        mailMessage.IsBodyHtml = true;

                        if (attachment != null)
                        {
                          
                                mailMessage.Attachments.Add(new Attachment(attachment.Path+attachment.Name, "" ));


                        }

                        mailMessage.To.Add(ToAddresses);
                        //if (CCAddresses != null && CCAddresses.Length > 0)
                        //{
                        //    mailMessage.CC.Add(CCAddresses);
                        //}
                        if (BCCAddresses != null && BCCAddresses.Length > 0)
                        {
                            mailMessage.Bcc.Add(BCCAddresses);
                        }

                        smtpClient.ServicePoint.MaxIdleTime = 2;
                        smtpClient.ServicePoint.ConnectionLimit = 1;

                        try
                        {
                            smtpClient.Send(mailMessage);

                        }
                        catch (Exception ex)
                        {
                            processingResult.IsError = true;
                            processingResult.Errors.Add(new ApiProcessingError(ex.Message, "Failed to send email", ""));
                            
                        }
                        finally
                        {
                            smtpClient.Dispose();
                        }
                        return processingResult;
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
            return processingResult;
        }
    }
}
