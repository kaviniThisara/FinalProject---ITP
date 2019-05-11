using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoomRservation
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        public frmMain(string text)
        {
            Text = text;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            pnlHome.BringToFront();
        }

        private void btnAvailability_Click(object sender, EventArgs e)
        {
            pnlAvailbility.BringToFront();
            loadRoomBookings();
         
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            pnlHome.BringToFront();
            btnViewAll_Click(sender, e);
        }


        private void btnBooking_Click(object sender, EventArgs e)
        {
            cmbSuiteRoom.Text = "0";
            cmbDeluxeRoom.Text = "0";
            cmbStandardRoom.Text = "0";
            pnlForm1.BringToFront();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }



        //method call



        private void clearAddCustomerForm()
        {
            txtboxContactID.Clear();
            txtboxFirstName.Clear();
            txtboxLastName.Clear();
            textBoxNIC.Clear();
            txtboxAddress.Clear();
            txtboxContactNumber.Clear();
            adults.Clear();
            children.Clear();
            cmbDeluxeRoom.SelectedIndex = 0;
            cmbSuiteRoom.SelectedIndex = 0;
            cmbStandardRoom.SelectedIndex = 0;

        }






        /*    private void cmbRoom_SelectedIndexChanged(object sender, EventArgs e)
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Text.Contains(cmbSuiteRoom.Text))
                {
                    MessageBox.Show("no item is selected");
                }
            }
       */


        private void btnSubmit_Click(object sender, EventArgs e)
        {


            if (txtboxFirstName.Text == "" || txtboxLastName.Text == "" || textBoxNIC.Text == "" || txtboxAddress.Text == "" || txtboxContactNumber.Text == "" || adults.Text == "" || children.Text == "")
            {
                MessageBox.Show("Please fill the fields");
            }



            else if (!ValidationRoomRes.validateName(txtboxFirstName.Text) || txtboxFirstName.Text.Equals(String.Empty))
            {
                MessageBox.Show("Please enter a valid first Name");
            }
            else if (!ValidationRoomRes.validateName(txtboxLastName.Text) || txtboxLastName.Text.Equals(String.Empty))
            {
                MessageBox.Show("Please enter a valid Last Name");
            }
            else if (!ValidationRoomRes.validateNIC(textBoxNIC.Text) || textBoxNIC.Text.Equals(String.Empty))
            {
                MessageBox.Show("Please enter a valid NIC");
            }

            else if (!ValidationRoomRes.validatePhoneNo(txtboxContactNumber.Text) || txtboxContactNumber.Text.Equals(String.Empty))
            {
                MessageBox.Show("Please enter a valid Phone number");
            }
            else if (!ValidationRoomRes.validateNumbers(adults.Text) || adults.Text.Equals(String.Empty))
            {
                MessageBox.Show("Please enter  number of adults 1 - 12");
            }
            else if (!ValidationRoomRes.validateChildren(children.Text) || children.Text.Equals(String.Empty))
            {
                MessageBox.Show("Please enter  number of children  0-12");
            }
            else if (cmbDeluxeRoom.Text == "" && cmbSuiteRoom.Text == "" && cmbStandardRoom.Text == "")
            {
                MessageBox.Show("Please enter number of rooms");
            }

            else if (!radioMale.Checked && !radioFemale.Checked)
            {
                MessageBox.Show("You forgot to select the gender!");
            }
            else
            {


                ContactClass c = new ContactClass();
                //c.ContactID = Int32.Parse(txtboxContactID.Text);
                c.FirstName = txtboxFirstName.Text;
                c.LastName = txtboxLastName.Text;
                c.NIC = textBoxNIC.Text;
                c.ContactNo = txtboxContactNumber.Text;
                c.Address = txtboxAddress.Text;
                String gender = "";

                if (radioMale.Checked)
                {
                    gender = "Male";
                }
                else if (radioFemale.Checked)
                {
                    gender = "Female";

                }

                c.Gender = gender;
                c.checkin = dob.Value.ToShortDateString();
                c.checkout = dateTimePicker1.Value.ToShortDateString();

                c.adults = adults.Text;
                c.children = children.Text;
                c.DeluxeRoom = cmbDeluxeRoom.Text;
                c.SuiteRoom = cmbSuiteRoom.Text;
                c.StandardRoom = cmbStandardRoom.Text;

                // c.Room = cmbRoom.Text;
                // c.Room = Int32.Parse(cmbSuiteRoom.Text);

                c.InsertCustomer();
                getRoomPrice();
                addRoomBookings();
                dgvAllCustomers.DataSource = loadAllCustomers();                   
            }
        }

        private void addRoomBookings()
        {
            String nic = textBoxNIC.Text;
            String checkin = dob.Value.ToString("yyyy-MM-dd");
            String checkout = dateTimePicker1.Value.ToString("yyyy-MM-dd");         

            insertBooking(nic, chkD1, checkin, checkout);
            insertBooking(nic, chkD2, checkin, checkout);
            insertBooking(nic, chkD3, checkin, checkout);
            insertBooking(nic, chkS1, checkin, checkout);
            insertBooking(nic, chkS2, checkin, checkout);
            insertBooking(nic, chkS3, checkin, checkout);
            insertBooking(nic, chkST1, checkin, checkout);
            insertBooking(nic, chkST2, checkin, checkout);
            insertBooking(nic, chkST3, checkin, checkout);

        }
        private void insertBooking(String nic, CheckBox c, String checkin, String checkout)
        {
            if (c.Checked)
            {
                using (DBConect db = new DBConect())
                {
                    char[] x = c.Text.ToCharArray();
                    string roomType = x[0].ToString();
                    string roomNo = x[1].ToString();


                    String q = "INSERT INTO `room_booking`(`customerNic`, `RoomType`, `RoomNo`, `checkin`, `checkout`) VALUES ('" + nic + "','"+roomType+"','"+roomNo+"','"+ checkin + "','"+checkout+"')";
                    MySqlCommand cmd = new MySqlCommand(q, db.con);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {


            if (txtboxFirstName.Text == "" || txtboxLastName.Text == "" || textBoxNIC.Text == "" || txtboxAddress.Text == "" || txtboxContactNumber.Text == "" || adults.Text == "" || children.Text == "")
            {
                MessageBox.Show("Please fill the fields");
            }



            else if (!ValidationRoomRes.validateName(txtboxFirstName.Text) || txtboxFirstName.Text.Equals(String.Empty))
            {
                MessageBox.Show("Please enter a valid first Name");
            }
            else if (!ValidationRoomRes.validateName(txtboxLastName.Text) || txtboxLastName.Text.Equals(String.Empty))
            {
                MessageBox.Show("Please enter a valid Last Name");
            }
            else if (!ValidationRoomRes.validateNIC(textBoxNIC.Text) || textBoxNIC.Text.Equals(String.Empty))
            {
                MessageBox.Show("Please enter a valid NIC");
            }

            else if (!ValidationRoomRes.validatePhoneNo(txtboxContactNumber.Text) || txtboxContactNumber.Text.Equals(String.Empty))
            {
                MessageBox.Show("Please enter a valid Phone number");
            }
            else if (!ValidationRoomRes.validateNumbers(adults.Text) || adults.Text.Equals(String.Empty))
            {
                MessageBox.Show("Please enter  number of adults 1 - 12");
            }
            else if (!ValidationRoomRes.validateChildren(children.Text) || children.Text.Equals(String.Empty))
            {
                MessageBox.Show("Please enter  number of children  0-12");
            }
            else if (cmbDeluxeRoom.Text == "" && cmbSuiteRoom.Text == "" && cmbStandardRoom.Text == "")
            {
                MessageBox.Show("Please enter number of rooms");
            }

            else if (!radioMale.Checked && !radioFemale.Checked)
            {
                MessageBox.Show("You forgot to select the gender!");
            }
            else
            {
                ContactClass c = new ContactClass();
                c.ContactID = Int32.Parse(txtboxContactID.Text);
                c.FirstName = txtboxFirstName.Text;
                c.LastName = txtboxLastName.Text;
                c.NIC = textBoxNIC.Text;
                c.ContactNo = txtboxContactNumber.Text;
                c.Address = txtboxAddress.Text;
                String gender = "";

                if (radioMale.Checked)
                {
                    gender = "Male";
                }
                else if (radioFemale.Checked)
                {
                    gender = "Female";

                }

                c.Gender = gender;
                c.checkin = dob.Value.ToString("yyyy-MM-dd");
                c.checkout = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                c.adults = adults.Text;
                c.children = children.Text;
                c.DeluxeRoom = cmbDeluxeRoom.Text;
                c.SuiteRoom = cmbSuiteRoom.Text;
                c.StandardRoom = cmbStandardRoom.Text;
                //  c.Room = cmbRoom.Text;
                //  c.Room = Int32.Parse(cmbSuiteRoom.Text);

                c.UpdateCustomer();

                clearAddCustomerForm();
                dgvAllCustomers.DataSource = loadAllCustomers();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtboxContactID.Text != "")
            {

                ContactClass c = new ContactClass();
                ContactClass d = new ContactClass();
                d = c.Search("ContactID = '" + txtboxContactID.Text + "'");

                txtboxFirstName.Text = d.FirstName;
                txtboxLastName.Text = d.LastName;
                textBoxNIC.Text = d.NIC;
                txtboxContactNumber.Text = d.ContactNo;
                txtboxAddress.Text = d.Address;

                if (d.Gender.Equals("Male"))
                {
                    radioMale.Checked = true;
                }
                else
                {
                    radioFemale.Checked = false;
                }

                dob.Value = Convert.ToDateTime(d.checkin);
                dateTimePicker1.Value = Convert.ToDateTime(d.checkout);
                // new DateTime(int year, int month, int date);
                //  c.DateOfBirth = dob.Value.ToString("yyyy-MM-dd");

                // txtAge.Text = d.age;
                adults.Text = d.adults;
                children.Text = d.children;
                cmbDeluxeRoom.Text = (d.DeluxeRoom.ToString()).Equals("") ? "0" : d.DeluxeRoom.ToString();
                cmbSuiteRoom.Text = d.SuiteRoom.ToString().Equals("")?"0": d.SuiteRoom.ToString();
                cmbStandardRoom.Text = d.StandardRoom.ToString().Equals("")?"0": d.StandardRoom.ToString();

                // cmbSuiteRoom.Text = d.Room.ToString();

                 getRoomPrice();
            }
        }




        public DataTable loadAllCustomers()
        {
            DataTable dt = new DataTable();

            using (DBConect db = new DBConect())
            {
                String q = "select ContactID as 'Customer ID ',FirstName as 'First Name',LastName as 'Last Name' from Customers";
                MySqlCommand cmd = new MySqlCommand(q, db.con);
                MySqlDataReader r = cmd.ExecuteReader();
                dt.Load(r);
            }
            return dt;
        }
        /*
               private void btnCheckForRoomType_Click(object sender, EventArgs e)
               {
                   dgvAvailability.DataSource = getAvailabilityForRoomType(cmbAvailableType.Text);
               }
*/

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            dgvAllCustomers.DataSource = loadAllCustomers();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtboxFirstName.Text == "" || txtboxLastName.Text == "" || textBoxNIC.Text == "" || txtboxAddress.Text == "" || txtboxContactNumber.Text == "" || adults.Text == "" || children.Text == "")
            {
                MessageBox.Show("Please enter field you want to delete");
            }
            else
            {
                ContactClass c = new ContactClass();
                c.ContactID = Int32.Parse(txtboxContactID.Text);

                c.deleteCustomer();

                clearAddCustomerForm();
                dgvAllCustomers.DataSource = loadAllCustomers();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearAddCustomerForm();
        }


        private void txtboxContactID_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            frmLogin f = new frmLogin();
            f.Show();
            this.Close();
        }

        private void dgvAllCustomers_MouseClick_1(object sender, MouseEventArgs e)
        {
            String contactID = dgvAllCustomers.SelectedRows[0].Cells[0].Value.ToString();
            txtboxContactID.Text = contactID;
            btnSearch_Click(sender, e);
        }

        private void button6_Click(object sender, EventArgs e)
        {

            if(isAvailable() != null)
            {
                String availableRoomList = isAvailable();
                MessageBox.Show(availableRoomList);
              

            }    

        }


        private List<RoomBooking> getRoomBookingsForName(String Name)
        {
            List<RoomBooking> result = new List<RoomBooking>();

            foreach(RoomBooking room in roomBookList)
            {
                if (room.RoomName.Equals(Name))
                {
                    result.Add(room);
                }
            }

            return result;
        }
        private List<String> getRoomNameList(String type)
        {
            List<String> result = new List<String>();
            using (DBConect db = new DBConect())
            {
                String q = "select * from rooms where Type='"+type+"'";
                MySqlCommand cmd = new MySqlCommand(q, db.con);
                MySqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    result.Add(r["Type"].ToString() + r["RoomNo"].ToString());
                }
            }

            
            return result;                
        }

        private Boolean isBlocked(RoomBooking room)
        {
            DateTime uiRoomCheckin = Convert.ToDateTime(CheckIn.Value.ToString());
            DateTime uiRoomCheckout = Convert.ToDateTime(CheckOut.Value.ToString());
            DateTime bookinCheckin = room.checkInDate;
            DateTime bookinCheckout =room.checkOutDate;

            if(uiRoomCheckin <= bookinCheckin && bookinCheckout <= uiRoomCheckout)
            {
                return true;
            }else if(uiRoomCheckin <= bookinCheckout && uiRoomCheckin >= bookinCheckin) 
            {
                return true;
            }else if(bookinCheckin <= uiRoomCheckout && bookinCheckout >= uiRoomCheckout)
            {
                return true;
            }else
            {
                return false;
            }

        }
      

        private String isAvailable()
        {
            Dictionary<String, Boolean> roomsWithAvailability = new Dictionary<string, bool>();

            foreach(String roomName in getRoomNameList(cmbRoomTypes.Text))
            {
                roomsWithAvailability.Add(roomName, true);
            }
                    
          
            if(roomBookList.Count > 0)    //list count
            {
                foreach(String roomName in getRoomNameList(cmbRoomTypes.Text))
                {
                    foreach(RoomBooking room in getRoomBookingsForName(roomName))
                    {
                        if (isBlocked(room))
                        {
                            roomsWithAvailability[roomName] = false;  //if the room is not available it is fall
                        
                        }
                       
                    }
                    
                }
                StringBuilder result = new StringBuilder(" ");
                foreach(KeyValuePair<String,Boolean> entry in roomsWithAvailability)
                {
                    if (entry.Value)
                    {
                        result.Append(entry.Key + " Room  ");
                        lblavailable.Text = result.ToString();
                    }
                   
                }
                return result.ToString();

            }
            return null;

        }

        List<RoomBooking> roomBookList = new List<RoomBooking>();
        private void loadRoomBookings()
        {

            roomBookList.Clear();
            using (DBConect db = new DBConect())
            {
                String q = "select * from room_booking";
                MySqlCommand cmd = new MySqlCommand(q, db.con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DateTime checkin = Convert.ToDateTime(reader["checkin"].ToString());
                    DateTime checkOut = Convert.ToDateTime(reader["checkout"].ToString());
                    roomBookList.Add(new RoomBooking(reader["RoomType"].ToString(), reader["RoomNo"].ToString(), checkin, checkOut));
                }

            }
        }

        private void btnTotal_Click(object sender, EventArgs e)
        {
                    if (ValidationRoomRes.validateDiscountText(txtDiscount.Text))
            {
                double discount;

                if (!txtDiscount.Text.Equals(""))
                {
                    discount = Double.Parse(txtDiscount.Text);
                }
                else
                {
                    discount = 1;
                }
                //  double price = Double.Parse(lblPrice1.Text) * Int32.Parse(cmbRoom.Text);
                double discountPrice = Double.Parse(lblPrice1.Text) * (Int32.Parse(cmbSuiteRoom.Text) + Int32.Parse(cmbSuiteRoom.Text) + Int32.Parse(cmbStandardRoom.Text)) * discount / 100;
                double totalPrice = (Double.Parse(lblPrice1.Text)*(Int32.Parse(cmbSuiteRoom.Text) + Int32.Parse(cmbSuiteRoom.Text) + Int32.Parse(cmbStandardRoom.Text))) - discountPrice;
                lblTotal1.Text = totalPrice.ToString();
            }
            else
            {
                MessageBox.Show("Disount should be a number between 0 and 100", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Report_Click(object sender, EventArgs e)
        {

        }

        /*
       private void btnCheckForRoomType_Click(object sender, EventArgs e)
       {
           String q;
           if (type.Equals(""))
           {
               q = "select r.RoomID as 'Room ID',r.Type as 'Room Type',c.FirstName as 'First Name', b.CheckIn as 'Check In', b.CheckOut as 'Check Out' from rooms r,room_booking b,customers c where r.RoomID = b.RoomID AND c.ContactID = b.CustomerID";
           }
           else
           {
               q = "select r.RoomID as 'Room ID',r.Type as 'Room Type',c.FirstName as 'First Name', b.CheckIn as 'Check In', b.CheckOut as 'Check Out' from rooms r,room_booking b,customers c where r.RoomID = b.RoomID AND c.ContactID = b.CustomerID AND r.Type = '" + type + "'";
           }


           DataTable dt = new DataTable();

           using (DBConect db = new DBConect())
           {
               MySqlCommand cmd = new MySqlCommand(q, db.con);
               MySqlDataReader r = cmd.ExecuteReader();

               dt.Load(r);
               return dt;
           }*/

        private void getRoomPrice()
        {
            String q = "select Type,Price from roomsPrice";
            Double total = 0;
            Dictionary<String, int> roomPriceList = new Dictionary<string, int>();
            //String q = "select Price from roomsPrice where Type = '" + cmbDeluxeRoom.Text + "' or Type = '" + cmbSuiteRoom .Text+ "' or type = '" + cmbStandardRoom.Text + "'";
            using (DBConect db = new DBConect())
            {
                MySqlCommand cmd = new MySqlCommand(q, db.con);
                MySqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        roomPriceList.Add(r[0].ToString(), Int32.Parse(r[1].ToString()));                                               
                    }
                }            
            }
            total = Int32.Parse(cmbDeluxeRoom.Text) * roomPriceList["Deluxe Room"] + Int32.Parse(cmbSuiteRoom.Text) * roomPriceList["Suite Room"] + Int32.Parse(cmbStandardRoom.Text) * roomPriceList["Standard Room"];

            lblPrice1.Text = total.ToString();
        }




        private void chkDiscount_CheckedChanged(object sender, EventArgs e)
        {

            if (chkDiscount.Checked)
            {
                lblDiscount.Enabled = true;
                txtDiscount.Enabled = true;
            }
            else
            {
                lblDiscount.Enabled = false;
                txtDiscount.Enabled = false;
            }
        }

        private void dgvAvailability_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public DataTable loadAllBookings()
        {
            DataTable dt = new DataTable();

            using (DBConect db = new DBConect())
            {
                String q = "select customerNic as 'customerNic ',Room type as 'Room type',RoomNo as 'Last Name' from Room_Booking";
                MySqlCommand cmd = new MySqlCommand(q, db.con);
                MySqlDataReader r = cmd.ExecuteReader();
                dt.Load(r);
            }
            return dt;
        }

        private void btnI_Click(object sender, EventArgs e)
        {
            txtboxFirstName.Text = "Ghan";
            txtboxLastName.Text = "Perera";
            textBoxNIC.Text = "96805236oV";
            txtboxContactNumber.Text= "07145698321";
            txtboxAddress.Text= "Malabe";
            dob.Text = "2019-09-20";
            dateTimePicker1.Text= "2019-09-23";
            adults.Text= "6";
            children.Text = "3";
            cmbDeluxeRoom.Text =" 1";
            cmbSuiteRoom.Text= "2";
            cmbStandardRoom.Text = "2";


        }

        private void btnU_Click(object sender, EventArgs e)
        {

            txtboxFirstName.Text = "Kumari";
            txtboxLastName.Text = "Wasala";
            textBoxNIC.Text = "68805236oV";
            txtboxContactNumber.Text = "0714428968";
            txtboxAddress.Text = "Kurunegala";
            dob.Text= "2019-10-30";
            dateTimePicker1.Text = "2019-10-31";
            adults.Text = "8";
            children.Text = "4";
            cmbDeluxeRoom.Text = " 2";
            cmbSuiteRoom.Text = "0";
            cmbStandardRoom.Text = "3";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CheckIn.Text = "2019-05-11";
            CheckOut.Text = "2019-10-31";
            cmbRoomTypes.Text = "D";
        }

        private void btnDEmo_Click(object sender, EventArgs e)
        {
            CheckIn.Text = "2019-10-30";
            CheckOut.Text = "2019-10-31";
            cmbRoomTypes.Text = "S";
        }
    }
}