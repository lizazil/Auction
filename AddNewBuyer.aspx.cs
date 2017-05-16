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
    public partial class AddNewBuyer : System.Web.UI.Page
    {
        string errMessage = null;
        string standartFormat = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropDown();
            }
        }

        private void LoadDropDown()
        {
            DataSet ds = (DataSet)Session["Lookup"];

            ddlGender.DataSource = ds.Tables[0];
            ddlGender.DataValueField = "GenderID";
            ddlGender.DataTextField = "Gender";
            ddlGender.DataBind();
            ddlGender.Items.Insert(0, "---Please select---");

            ddlRace.DataSource = ds.Tables[1];
            ddlRace.DataValueField = "RaceID";
            ddlRace.DataTextField = "Race";
            ddlRace.DataBind();
            ddlRace.Items.Insert(0, "---Please select---");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Buyer.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int buyerIdOut = 0;
            int flag = 0;
            if (IsValidData())
            {
                lblErrMessage.Text = null;
                lblErrMessageUp.Text = null;
                var dba = new DBAccess();
                flag = dba.SaveBuyerInfo(txtFirstName.Text, txtLastName.Text, Convert.ToInt32(ddlGender.SelectedValue), 
                                  Convert.ToInt32(ddlRace.SelectedValue), txtAddress.Text, txtCity.Text, ddlState.SelectedValue, 
                                  txtZip.Text, standartFormat, txtEMail.Text, ref buyerIdOut, 0);
                if (flag == 0)
                {
                    lblErrMessage.Text = "There is a problem with saving data";
                    lblErrMessageUp.Text = "There is a problem with saving data";
                }
                else
                {
                    lblErrMessage.Text = "The Information For Buyer " + buyerIdOut + " Added Successfully";
                }
            }
            else
            {
                lblErrMessage.Text = errMessage.ToString();
                lblErrMessageUp.Text = errMessage.ToString();
            }
        }

        private bool IsValidData()
        {
            return
                Validator.IsPresent(txtFirstName.Text, "First Name", ref errMessage)
             && Validator.IsPresent(txtLastName.Text, "Last Name", ref errMessage)
             && Validator.IsSelected(ddlGender, "Gender", ref errMessage)
             && Validator.IsSelected(ddlRace, "Race", ref errMessage)
             && Validator.IsPresent(txtAddress.Text, "Address", ref errMessage)
             && Validator.IsPresent(txtCity.Text, "City", ref errMessage)
             && Validator.IsChecked(rdoUSA, rdoCanada, "USA or Canada", ref errMessage)
             && Validator.IsSelected(ddlState, "State", ref errMessage)
             && Validator.IsPresent(txtZip.Text, "Zip Code", ref errMessage)
             && Validator.IsValidZip(txtZip.Text, "Zip Code",  txtCountry.Text, ref errMessage)
             && Validator.IsValidPhoneNumber(txtPhone.Text, "Phone Number", ref standartFormat, ref errMessage)
             && Validator.IsPresent(txtEMail.Text, "E-Mail", ref errMessage)
            ;
        }

        protected void rdoUSA_CheckedChanged(object sender, EventArgs e)
        {
            rdoCanada.Checked = false;
            txtCountry.Text = "US";
            LoadStateDropDown();
        }

        protected void rdoCanada_CheckedChanged(object sender, EventArgs e)
        {
            rdoUSA.Checked = false;
            txtCountry.Text = "CA";
            LoadStateDropDown();
        }

        private void LoadStateDropDown()
        {
            DataSet ds = (DataSet)Session["Lookup"];

            pnMain.Visible = true;

            if (rdoUSA.Checked)
            {
                ddlState.DataSource = ds.Tables[2];
                ddlState.DataValueField = "StateID";
                ddlState.DataTextField = "State";
                ddlState.DataBind();
                ddlState.Items.Insert(0, "---Please Select---");
            }
            else if (rdoCanada.Checked)
            {   
                ddlState.DataSource = ds.Tables[3];
                ddlState.DataValueField = "StateID";
                ddlState.DataTextField = "State";
                ddlState.DataBind();
                ddlState.Items.Insert(0, "---Please Select---");
            }
        }
    }
}