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
    public partial class EditBuyer : System.Web.UI.Page
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
            ddlGender.Items.Insert(0, "---Please Select---");

            ddlRace.DataSource = ds.Tables[1];
            ddlRace.DataValueField = "RaceID";
            ddlRace.DataTextField = "Race";
            ddlRace.DataBind();
            ddlRace.Items.Insert(0, "---Please Select---");

            ddlUSAStates.DataSource = ds.Tables[2];
            ddlUSAStates.DataValueField = "StateID";
            ddlUSAStates.DataTextField = "State";
            ddlUSAStates.DataBind();
            ddlUSAStates.Items.Insert(0, "---Please Select---");

            ddlCanadaStates.DataSource = ds.Tables[3];
            ddlCanadaStates.DataValueField = "StateID";
            ddlCanadaStates.DataTextField = "State";
            ddlCanadaStates.DataBind();
            ddlCanadaStates.Items.Insert(0, "---Please Select---");

            ddlBuyers.DataSource = ds.Tables[4];
            ddlBuyers.DataValueField = "BuyerID";
            ddlBuyers.DataTextField = "BuyerName";
            ddlBuyers.DataBind();
            ddlBuyers.Items.Insert(0, "---Please Select---");

        }

        string country = null;
        protected void ddlBuyers_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblErrMessage.Text = null;
           
            try
            {
                if (ddlBuyers.SelectedIndex > 0)
                {
                    DBAccess dba = new DBAccess();
                    int buyerId = Convert.ToInt32(ddlBuyers.SelectedValue);
                    DataTable dt = dba.GetBuyerInfo(buyerId);

                    if (dt.Rows.Count > 0)
                    {
                        pnMain.Visible = true;
                       
                        DataRow dr = dt.Rows[0];
                        txtFirstName.Text = dr["FirstName"].ToString();
                        txtLastName.Text = dr["LastName"].ToString();
                        txtBuyerID.Text = dr["BuyerID"].ToString();
                        ddlGender.SelectedValue = dr["GenderID"].ToString();
                        ddlRace.SelectedValue = dr["RaceID"].ToString();
                        txtAddress.Text = dr["Address"].ToString();
                        txtCity.Text = dr["City"].ToString();
                        
                        if (dr["CountryID"].Equals("US"))
                        {
                            chkUSA.Checked = true;
                            ddlUSAStates.SelectedValue = dr["StateID"].ToString();
                            ddlUSAStates.Visible = true;
                            ddlCanadaStates.Visible = false;
                        }
                        else
                        {
                            chkCA.Checked = true;
                            ddlCanadaStates.SelectedValue = dr["StateID"].ToString();
                            ddlUSAStates.Visible = false;
                            ddlCanadaStates.Visible = true;
                        }
                        
                        txtZip.Text = dr["Zip"].ToString();
                        txtPhone.Text = dr["PhoneNumber"].ToString();
                        txtEMail.Text = dr["Email"].ToString();
                    }
                }
                else
                {
                    pnMain.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblErrMessage.Text = ex.Message;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Buyer.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Buyer.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblErrMessage.Text = null;
            int flag = 0;
            int buyerIDOut = 0;
            string state = null;

            if (chkUSA.Checked)
                country = "US";
            else
                country = "CA";

            if (IsValidData())
            {
                try
                {
                    if (ddlBuyers.SelectedIndex > 0)
                    {
                        DBAccess dba = new DBAccess();

                        if (chkUSA.Checked)
                            state = ddlUSAStates.SelectedValue;
                        else
                            state = ddlCanadaStates.SelectedValue;

                        int buyerId = Convert.ToInt32(ddlBuyers.SelectedValue);

                        flag = dba.SaveBuyerInfo(txtFirstName.Text, txtLastName.Text, Convert.ToInt32(ddlGender.SelectedValue),
                                                 Convert.ToInt32(ddlRace.SelectedValue), txtAddress.Text, txtCity.Text, state,
                                                 txtZip.Text, txtPhone.Text, txtEMail.Text, ref buyerIDOut, buyerId);
                        if (flag == 1)
                        {
                            lblErrMessage.Text = "Information for Buyer " + txtBuyerID.Text + " Updated Successfully.";
                            DataSet ds = dba.GetLookUp();
                            Session["Lookup"] = ds;
                            LoadDropDown();
                        }
                        else
                        {
                            lblErrMessage.Text = "Problem to Update information for Buyer " + txtBuyerID.Text;
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblErrMessage.Text = ex.Message;
                }
            }
            else
            {
                lblErrMessage.Text = errMessage.ToString();
            }
                       
        }

        private bool IsValidData()
        {
            return
                   Validator.IsValidZip(txtZip.Text, "Zip Code", country, ref errMessage)
                && Validator.IsValidPhoneNumber(txtPhone.Text, "Phone", ref standartFormat, ref errMessage);
        }

        protected void chkUSA_CheckedChanged(object sender, EventArgs e)
        {
            chkCA.Checked = false;
            ddlUSAStates.Visible = true;
            ddlCanadaStates.Visible = false;
        }

        protected void chkCA_CheckedChanged(object sender, EventArgs e)
        {
            chkUSA.Checked = false;
            ddlCanadaStates.Visible = true;
            ddlUSAStates.Visible = false;

        }
    }
}