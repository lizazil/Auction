using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Auction.Common;

namespace Auction
{
    public partial class Buyer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadLookUp();
            }
            if (Session["Login"] != null)
            {
                lgnControl.Visible = false;
                IDMain.Visible = true;
            }

        }

        private void LoadLookUp()
        {
            DBAccess dba = new DBAccess();
            DataSet ds = dba.GetLookUp();

            Session["Lookup"] = ds;   // holds data throgh all the pages for a current user
        }

        protected void lnkAddNewBuyer_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AddNewBuyer.aspx");
        }

        protected void lnkEditBuyerInfo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/EditBuyer.aspx");
        }

        protected void lnkBuyerInfo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BuyerInfo.aspx");
        }

        protected void lgnControl_Authenticate(object sender, AuthenticateEventArgs e)
        {
            lblLoginMessage.Text = "";
            if (string.IsNullOrEmpty(lgnControl.UserName))
            {
                lblLoginMessage.Text = "Please enter User Name";
            }
            if (string.IsNullOrEmpty(lgnControl.Password))
            {
                lblLoginMessage.Text = "Please enter Password";
            }

            string userName = System.Configuration.ConfigurationManager.AppSettings["userName"];
            string pass = System.Configuration.ConfigurationManager.AppSettings["pass"];

            if (lgnControl.UserName == userName && lgnControl.Password == pass)
            {
                IDMain.Visible = true;
                lgnControl.Visible = false;
                Session["Login"] = userName;   // holds data throgh all the pages for a current user
            }
            else
                lblLoginMessage.Text = "Wrong User Name or Password";           
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            IDMain.Visible = false;
            lgnControl.Visible = true;
        }
    }
}