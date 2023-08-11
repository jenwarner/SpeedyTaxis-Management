using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using WebService.Models;
using SpeedyTaxis.Processors;
using SpeedyTaxis.Repositories;

namespace SpeedyTaxis
{
    public partial class Management : Form
    {
        public Management()
        {
            InitializeComponent();
        }
        bool moving;
        Point offset;
        Point original;
        private BindingSource bindingSource1 = new BindingSource();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        string fN, sN;
        int driverID = 0;
        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void titlePanel_MouseDown(object sender, MouseEventArgs e)
        {
            moving = true;
            titlePanel.Capture = true;
            offset = MousePosition;
            original = this.Location;
        }

        private void titlePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!moving)
                return;

            int x = original.X + MousePosition.X - offset.X;
            int y = original.Y + MousePosition.Y - offset.Y;

            this.Location = new Point(x, y);
        }

        private void titlePanel_MouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
            titlePanel.Capture = false;
        }

        private void employeeBtn_Click(object sender, EventArgs e)
        {
            accentDisciplinaryPanel.Visible = false;
            menuDataTabControl.Visible = true;
            accentLogoutPanel.Visible = false;
            accentTrainingPanel.Visible = false;
            accentQualificationsPanel.Visible = false;
            accentSearchPanel.Visible = false;
            accentEmployeePanel.Visible = true;
            searchBtn.ForeColor = Color.Black;
            searchBtn.BackColor = Color.Gold;

            // show employee tab
            menuDataTabControl.SelectTab(employeeTab);
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            accentDisciplinaryPanel.Visible = false;
            menuDataTabControl.Visible = true;
            accentLogoutPanel.Visible = false;
            accentTrainingPanel.Visible = false;
            accentEmployeePanel.Visible = false;
            accentQualificationsPanel.Visible = false;
            accentSearchPanel.Visible = false;
            searchBtn.ForeColor = Color.Gold;
            searchBtn.BackColor = Color.Black;

            // show search tab
            menuDataTabControl.SelectTab(searchTab);
        }

        private void qualificationsBtn_Click(object sender, EventArgs e)
        {
            accentDisciplinaryPanel.Visible = false;
            menuDataTabControl.Visible = true;
            accentLogoutPanel.Visible = false;
            accentTrainingPanel.Visible = false;
            accentEmployeePanel.Visible = false;
            accentSearchPanel.Visible = false;
            accentQualificationsPanel.Visible = true;
            searchBtn.ForeColor = Color.Black;
            searchBtn.BackColor = Color.Gold;

            // show qualifications tab
            menuDataTabControl.SelectTab(qualificationsTab);   
        }

        private void trainingBtn_Click(object sender, EventArgs e)
        {
            accentDisciplinaryPanel.Visible = false;
            menuDataTabControl.Visible = true;
            accentLogoutPanel.Visible = false;
            accentEmployeePanel.Visible = false;
            accentSearchPanel.Visible = false;
            accentQualificationsPanel.Visible = false;
            accentTrainingPanel.Visible = true;
            searchBtn.ForeColor = Color.Black;
            searchBtn.BackColor = Color.Gold;

            // show training tab
            menuDataTabControl.SelectTab(trainingTab);   
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            accentDisciplinaryPanel.Visible = false;
            accentEmployeePanel.Visible = false;
            accentSearchPanel.Visible = false;
            accentQualificationsPanel.Visible = false;
            accentTrainingPanel.Visible = false;
            accentLogoutPanel.Visible = true;
            searchBtn.ForeColor = Color.Black;
            searchBtn.BackColor = Color.Gold;

            // show logout panel
            logoutPanel.Visible = true;
            logoutLbl.Visible = true;
            yesLogoutBtn.Visible = true;
            noLogoutBtn.Visible = true;

            // disable main menu controls
            employeeBtn.Enabled = false;
            searchBtn.Enabled = false;
            qualificationsBtn.Enabled = false;
            trainingBtn.Enabled = false;
            disciplinaryBtn.Enabled = false;
        }

        private void yesLogoutBtn_Click(object sender, EventArgs e)
        {
            // log user out

            // back to login screen
            this.Hide();
            LoginForm lf = new LoginForm();
            lf.ShowDialog();
        }

        private void noLogoutBtn_Click(object sender, EventArgs e)
        {
            logoutPanel.Visible = false;
            logoutLbl.Visible = false;
            yesLogoutBtn.Visible = false;
            noLogoutBtn.Visible = false;
            accentLogoutPanel.Visible = false;

            // re-enable main menu controls
            employeeBtn.Enabled = true;
            searchBtn.Enabled = true;
            qualificationsBtn.Enabled = true;
            trainingBtn.Enabled = true;
            disciplinaryBtn.Enabled = true;

            // show panel that is active/visible/not hidden
            // if statement?
        }

        private void Management_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'speedytaxisDataSet.Driver' table. You can move, or remove it, as needed.
            this.driverTableAdapter1.Fill(this.speedytaxisDataSet.Driver);
            // TODO: This line of code loads data into the 'speedytaxisDataSet3.Training' table. You can move, or remove it, as needed.
            this.trainingTableAdapter.Fill(this.speedytaxisDataSet3.Training);
            // TODO: This line of code loads data into the 'speedytaxisDataSet2.Qualification' table. You can move, or remove it, as needed.
            this.qualificationTableAdapter.Fill(this.speedytaxisDataSet2.Qualification);
            // TODO: This line of code loads data into the 'speedytaxisDataSet1.Driver' table. You can move, or remove it, as needed.
            this.driverTableAdapter.Fill(this.speedytaxisDataSet1.Driver);
            //menuDataTabControl.ItemSize = new Size(menuDataTabControl.ItemSize.Width, 1);
            menuDataTabControl.Appearance = TabAppearance.FlatButtons;
            menuDataTabControl.ItemSize = new Size(0, 1);
            menuDataTabControl.SizeMode = TabSizeMode.Fixed;

            // ascending order
            this.dataGridView1.Sort(this.dataGridView1.Columns[0], ListSortDirection.Ascending);
            this.dataGridView2.Sort(this.dataGridView2.Columns[0], ListSortDirection.Ascending);
            this.dataGridView3.Sort(this.dataGridView3.Columns[0], ListSortDirection.Ascending);
            this.dataGridView4.Sort(this.dataGridView4.Columns[0], ListSortDirection.Ascending);
            this.dataGridView5.Sort(this.dataGridView5.Columns[0], ListSortDirection.Ascending);
            this.dataGridView6.Sort(this.dataGridView6.Columns[0], ListSortDirection.Ascending);
            this.dataGridView7.Sort(this.dataGridView7.Columns[0], ListSortDirection.Ascending);


            this.dataGridView1.DefaultCellStyle.Font = new Font("Microsoft YaHei UI", 10);
            connLbl.Text = DBConnectivity.ActiveConnection();

            // expiring qualifications
            List<Driver> driver = new List<Driver>();
            driver = QualificationManagerRepository.FindExpiringQualifcationsAndDriver();
            foreach (var d in driver)
            {
                richTextBox7.AppendText(d.DriverName+ "\n");
            }
            // expiring training
            //List<Driver> driver2 = new List<Driver>();
            driver = TrainingManagerRepository.FindExpiringTrainingAndDriver();
            foreach (var d in driver)
            {
                richTextBox6.AppendText(d.DriverName + "\n");
            }
            
            //richTextBox7.Text = DBConnectivity.FindExpiringQualifcationsAndDriver().ToString();
        }

        private void disciplinaryBtn_Click(object sender, EventArgs e)
        {
            menuDataTabControl.Visible = true;
            accentLogoutPanel.Visible = false;
            accentDisciplinaryPanel.Visible = true;
            accentEmployeePanel.Visible = false;
            accentSearchPanel.Visible = false;
            accentQualificationsPanel.Visible = false;
            accentTrainingPanel.Visible = false;
            searchBtn.ForeColor = Color.Black;
            searchBtn.BackColor = Color.Gold;

            // show disciplinary tab
            menuDataTabControl.SelectTab(disciplinaryTab); 
        }

        private void driverEditBtn_Click(object sender, EventArgs e)
        {
            // get ID of selected driver
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            int columnindex = 0;

            //connLbl.Text = dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(); // test

            // add selected driver data to editing form
            // get conn to database
            firstNameTxtBox.Text = DriverManagerRepository.LoadDriverInfoFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "first name");
            surnameTxtBox.Text = DriverManagerRepository.LoadDriverInfoFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "surname");
            sexCB.Text = DriverManagerRepository.LoadDriverInfoFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "sex");
            phoneNumberTxtBox.Text = DriverManagerRepository.LoadDriverInfoFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "phone number");
            licenceIDTxtBox.Text = DriverManagerRepository.LoadDriverInfoFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "licence id");
            addressTxtBox.Text = DriverManagerRepository.LoadDriverInfoFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "address");
            employmentDateDTP.Text = DriverManagerRepository.LoadDriverInfoFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "employment date");
            // training
           // connLbl.Text = DBConnectivity.CheckDriverTrainingFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "advanced driving course");
            // advanced driving course
            if (TrainingManagerRepository.CheckDriverTrainingFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "advanced driving course") == dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString())
            {
                adcChckBox.Checked = true;
                adcDTP.Text = TrainingManagerRepository.LoadDriverTrainingFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "advanced driving course", "expiry date");
                
            }
            else
            {
                adcChckBox.Checked = false;
                adcDTP.Text = "";
            }
            // driving at night
            if (TrainingManagerRepository.CheckDriverTrainingFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "driving at night") == dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString())
            {
                danChckBox.Checked = true;
                danDTP.Text = TrainingManagerRepository.LoadDriverTrainingFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "driving at night", "expiry date");
            }
            else
            {
                danChckBox.Checked = false;
                danDTP.Text = "";
            }
            // cyclist awareness
            if (TrainingManagerRepository.CheckDriverTrainingFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "cyclist awareness") == dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString())
            {
                caChckBox.Checked = true;
                caDTP.Text = TrainingManagerRepository.LoadDriverTrainingFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "cyclist awareness", "expiry date");
            }
            else
            {
                caChckBox.Checked = false;
                caDTP.Text = "";
            }
            // reduce fuel consumption
            if (TrainingManagerRepository.CheckDriverTrainingFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "reduce fuel consumption") == dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString())
            {
                rfcChckBox.Checked = true;
                rfcDTP.Text = TrainingManagerRepository.LoadDriverTrainingFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "reduce fuel consumption", "expiry date");
            }
            else
            {
                rfcChckBox.Checked = false;
                rfcDTP.Text = "";
            }
            // qualifications
            // gt: central london
            if (QualificationManagerRepository.CheckDriverQualificationsFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "gt: central london") == dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString())
            {
                clChckBox.Checked = true;
                clDTP.Text = QualificationManagerRepository.LoadDriverQualificationsFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "gt: central london", "expiry date");

            }
            else
            {
                clChckBox.Checked = false;
                clDTP.Text = "";
            }
            // gt: north london
            if (QualificationManagerRepository.CheckDriverQualificationsFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "gt: north london") == dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString())
            {
                nlChckBox.Checked = true;
                nlDTP.Text = QualificationManagerRepository.LoadDriverQualificationsFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "gt: north london", "expiry date");

            }
            else
            {
                nlChckBox.Checked = false;
                nlDTP.Text = "";
            }
            // gt: south london
            if (QualificationManagerRepository.CheckDriverQualificationsFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "gt: south london") == dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString())
            {
                slChckBox.Checked = true;
                slDTP.Text = QualificationManagerRepository.LoadDriverQualificationsFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "gt: south london", "expiry date");

            }
            else
            {
                slChckBox.Checked = false;
                slDTP.Text = "";
            }
            // gt: east london
            if (QualificationManagerRepository.CheckDriverQualificationsFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "gt: east london") == dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString())
            {
                elChckBox.Checked = true;
                elDTP.Text = QualificationManagerRepository.LoadDriverQualificationsFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "gt: east london", "expiry date");

            }
            else
            {
                elChckBox.Checked = false;
                elDTP.Text = "";
            }
            // gt: west london
            if (QualificationManagerRepository.CheckDriverQualificationsFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "gt: west london") == dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString())
            {
                wlChckBox.Checked = true;
                wlDTP.Text = QualificationManagerRepository.LoadDriverQualificationsFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "gt: west london", "expiry date");

            }
            else
            {
                wlChckBox.Checked = false;
                wlDTP.Text = "";
            }
            // driving licence
            if (QualificationManagerRepository.CheckDriverQualificationsFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "driving licence") == dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString())
            {
                dlChckBox.Checked = true;
                dlDTP.Text = QualificationManagerRepository.LoadDriverQualificationsFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString(), "driving licence", "expiry date");

            }
            else
            {
                dlChckBox.Checked = false;
                dlDTP.Text = "";
            }
            

        }

        private void submitDriverBtn_Click(object sender, EventArgs e)
        {
            // vaildation errors
            if (firstNameTxtBox.Text == "" || surnameTxtBox.Text == "" || sexCB.Text == "" || addressTxtBox.Text == "" || employmentDateDTP.Text == "" || phoneNumberTxtBox.Text == "" || licenceIDTxtBox.Text == "")
            {
                errorLbl.Text = "You must not leave fields blank.";
            }
            else
            {
                // if dID exists in Driver table, update data. otherwise create new entry
                // get ID of selected driver
                int rowindex = dataGridView1.CurrentCell.RowIndex;
                //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
                int columnindex = 0;

                // connLbl.Text = "success";
                //connLbl.Text = DBConnectivity.CheckDriverExistsFromID(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString());
                //this if statement always passes so need different logic.... two parameters - number and "new" or "update"?
                if (DriverManagerRepository.CheckDriverExistsFromLicenceID(licenceIDTxtBox.Text) == "existing")
                {
                    // update data
                    //SaveTrainingFromDriverLicenceID();
                    UpdateDriver();
                    // DriverManagerRepository.UpdateDriver(firstNameTxtBox.Text, surnameTxtBox.Text, sexCB.Text, phoneNumberTxtBox.Text, employmentDateDTP.Text, licenceIDTxtBox.Text, addressTxtBox.Text, int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                    // TRAINING
                    // advanced driving course
                     if (adcChckBox.Checked == true)
                     {
                        // need to check if training exists from driver ID. if it does then update, otherwise create new entry
                        if (TrainingManagerRepository.CheckDriverTrainingExistsFromdID(int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()), "Advanced Driving Course") == "existing")
                        {
                            TrainingManagerRepository.UpdateDriverTrainingFromID("advanced driving course", adcDTP.Text, int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                        }
                        else
                        {
                            // create new
                            TrainingManagerRepository.AddDriverTrainingFromLicenceID("advanced driving course", adcDTP.Text, licenceIDTxtBox.Text);
                        }
                        
                     }
                     else if (adcChckBox.Checked == false)
                     {
                        //delete record if it exists
                        TrainingManagerRepository.DeleteDriverTrainingFromID("advanced driving course", int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                     }
                    // driving at night
                    if (danChckBox.Checked == true)
                    {
                        if (TrainingManagerRepository.CheckDriverTrainingExistsFromdID(int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()), "Driving at Night") == "existing")
                        {
                            TrainingManagerRepository.UpdateDriverTrainingFromID("driving at night", danDTP.Text, int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                        }
                        else
                        {
                            // create new
                            TrainingManagerRepository.AddDriverTrainingFromLicenceID("driving at night", danDTP.Text, licenceIDTxtBox.Text);
                        }                
                    }
                    else if (danChckBox.Checked == false)
                    {
                        //delete record if it exists
                        TrainingManagerRepository.DeleteDriverTrainingFromID("driving at night", int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                    }
                    // cyclist awareness
                    if (caChckBox.Checked == true)
                    {
                        if (TrainingManagerRepository.CheckDriverTrainingExistsFromdID(int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()), "Cyclist Awareness") == "existing")
                        {
                            TrainingManagerRepository.UpdateDriverTrainingFromID("cyclist awareness", caDTP.Text, int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                        }
                        else
                        {
                            // create new
                            TrainingManagerRepository.AddDriverTrainingFromLicenceID("cyclist awareness", caDTP.Text, licenceIDTxtBox.Text);
                        }                        
                    }
                    else if (caChckBox.Checked == false)
                    {
                        //delete record if it exists
                        TrainingManagerRepository.DeleteDriverTrainingFromID("cyclist awareness", int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                    }
                    // reduce fuel consumption
                    if (rfcChckBox.Checked == true)
                    {
                        if (TrainingManagerRepository.CheckDriverTrainingExistsFromdID(int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()), "Reduce Fuel Consumption") == "existing")
                        {
                            TrainingManagerRepository.UpdateDriverTrainingFromID("reduce fuel consumption", rfcDTP.Text, int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                        }
                        else
                        {
                            // create new
                            TrainingManagerRepository.AddDriverTrainingFromLicenceID("reduce fuel consumption", rfcDTP.Text, licenceIDTxtBox.Text);
                        }                   
                    }
                    else if (rfcChckBox.Checked == false)
                    {
                        //delete record if it exists
                        TrainingManagerRepository.DeleteDriverTrainingFromID("reduce fuel consumption", int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                    }
                    // QUALIFICATIONS
                    // gt: central london
                    if (clChckBox.Checked == true)
                    {
                        if (QualificationManagerRepository.CheckDriverQualificationExistsFromdID(int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()), "Central London") == "existing")
                        {
                            QualificationManagerRepository.UpdateDriverQualificationsFromID("central london", clDTP.Text, int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                        }
                        else
                        {
                            // create new
                            QualificationManagerRepository.AddDriverQualificationsFromLicenceID("central london", clDTP.Text, licenceIDTxtBox.Text);
                        }
                        
                    }
                    else if (clChckBox.Checked == false)
                    {
                        //delete record if it exists
                        QualificationManagerRepository.DeleteDriverQualificationsFromID("central london", int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                    }
                    // gt: north london
                    if (nlChckBox.Checked == true)
                    {
                        if (QualificationManagerRepository.CheckDriverQualificationExistsFromdID(int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()), "North London") == "existing")
                        {
                            QualificationManagerRepository.UpdateDriverQualificationsFromID("north london", nlDTP.Text, int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                        }
                        else
                        {
                            // create new
                            QualificationManagerRepository.AddDriverQualificationsFromLicenceID("north london", nlDTP.Text, licenceIDTxtBox.Text);
                        }
                        
                    }
                    else if (nlChckBox.Checked == false)
                    {
                        //delete record if it exists
                        QualificationManagerRepository.DeleteDriverQualificationsFromID("north london", int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                    }
                    // gt: south london
                    if (slChckBox.Checked == true)
                    {
                        if (QualificationManagerRepository.CheckDriverQualificationExistsFromdID(int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()), "South London") == "existing")
                        {
                            QualificationManagerRepository.UpdateDriverQualificationsFromID("south london", slDTP.Text, int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                        }
                        else
                        {
                            // create new
                            QualificationManagerRepository.AddDriverQualificationsFromLicenceID("south london", slDTP.Text, licenceIDTxtBox.Text);
                        }
                        
                    }
                    else if (slChckBox.Checked == false)
                    {
                        //delete record if it exists
                        QualificationManagerRepository.DeleteDriverQualificationsFromID("south london", int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                    }
                    // gt: east london
                    if (elChckBox.Checked == true)
                    {
                        if (QualificationManagerRepository.CheckDriverQualificationExistsFromdID(int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()), "East London") == "existing")
                        {
                            QualificationManagerRepository.UpdateDriverQualificationsFromID("east london", elDTP.Text, int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                        }
                        else
                        {
                            // create new
                            QualificationManagerRepository.AddDriverQualificationsFromLicenceID("east london", elDTP.Text, licenceIDTxtBox.Text);
                        }
                        
                    }
                    else if (elChckBox.Checked == false)
                    {
                        //delete record if it exists
                        QualificationManagerRepository.DeleteDriverQualificationsFromID("east london", int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                    }
                    // gt: west london
                    if (wlChckBox.Checked == true)
                    {
                        if (QualificationManagerRepository.CheckDriverQualificationExistsFromdID(int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()), "West London") == "existing")
                        {
                            QualificationManagerRepository.UpdateDriverQualificationsFromID("west london", wlDTP.Text, int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                        }
                        else
                        {
                            // create new
                            QualificationManagerRepository.AddDriverQualificationsFromLicenceID("west london", wlDTP.Text, licenceIDTxtBox.Text);
                        }
                        
                    }
                    else if (wlChckBox.Checked == false)
                    {
                        //delete record if it exists
                        QualificationManagerRepository.DeleteDriverQualificationsFromID("west london", int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                    }
                    // driving licence
                    if (dlChckBox.Checked == true)
                    {
                        if (QualificationManagerRepository.CheckDriverQualificationExistsFromdID(int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()), "Driving Licence") == "existing")
                        {
                            QualificationManagerRepository.UpdateDriverQualificationsFromID("driving licence", dlDTP.Text, int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                        }
                        else
                        {
                            // create new
                            QualificationManagerRepository.AddDriverQualificationsFromLicenceID("driving licence", dlDTP.Text, licenceIDTxtBox.Text);
                        }
                        
                    }
                    else if (dlChckBox.Checked == false)
                    {
                        //delete record if it exists
                        QualificationManagerRepository.DeleteDriverQualificationsFromID("driving licence", int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
                    }
                    
                    OleDbConnection conn = DBConnectivity.GetConn();
                    OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Driver", conn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "Driver");
                    dataGridView1.DataSource = ds;
                    dataGridView1.DataMember = "Driver";
                    conn.Close();
                    //connLbl.Text = "existing";

                    errorLbl.Text = "Driver updated successfully.";
                }
                else if (DriverManagerRepository.CheckDriverExistsFromLicenceID(licenceIDTxtBox.Text) == "new")
                {
                    // add driver to database via web service
                    //comment out for now bc don't want to keep adding items to database
                    // DBConnectivity.AddDriver(firstNameTxtBox.Text, surnameTxtBox.Text, sexCB.Text, phoneNumberTxtBox.Text, employmentDateDTP.Text, licenceIDTxtBox.Text, addressTxtBox.Text);
                    SaveDriver();
                    // TRAINING
                   // SaveTrainingFromDriverLicenceID();
                    // advanced driving course
                    if (adcChckBox.Checked == true)
                    {
                        TrainingManagerRepository.AddDriverTrainingFromLicenceID("advanced driving course", adcDTP.Text, licenceIDTxtBox.Text);
                    }
                    // driving at night
                    if (danChckBox.Checked == true)
                    {
                        TrainingManagerRepository.AddDriverTrainingFromLicenceID("driving at night", danDTP.Text, licenceIDTxtBox.Text);
                    }
                    // cyclist awareness
                    if (caChckBox.Checked == true)
                    {
                        TrainingManagerRepository.AddDriverTrainingFromLicenceID("cyclist awareness", caDTP.Text, licenceIDTxtBox.Text);
                    }
                    // reduce fuel consumption
                    if (rfcChckBox.Checked == true)
                    {
                        TrainingManagerRepository.AddDriverTrainingFromLicenceID("reduce fuel consumption", rfcDTP.Text, licenceIDTxtBox.Text);
                    }

                    // QUALIFICATIONS
                    // gt: central london
                    if (clChckBox.Checked == true)
                    {
                        QualificationManagerRepository.AddDriverQualificationsFromLicenceID("central london", clDTP.Text, licenceIDTxtBox.Text);
                    }
                    // gt: north london
                    if (nlChckBox.Checked == true)
                    {
                        QualificationManagerRepository.AddDriverQualificationsFromLicenceID("north london", nlDTP.Text, licenceIDTxtBox.Text);
                    }
                    // gt: south london
                    if (slChckBox.Checked == true)
                    {
                        QualificationManagerRepository.AddDriverQualificationsFromLicenceID("south london", slDTP.Text, licenceIDTxtBox.Text);
                    }
                    // gt: east london
                    if (elChckBox.Checked == true)
                    {
                        QualificationManagerRepository.AddDriverQualificationsFromLicenceID("east london", elDTP.Text, licenceIDTxtBox.Text);
                    }
                    // gt: west london
                    if (wlChckBox.Checked == true)
                    {
                        QualificationManagerRepository.AddDriverQualificationsFromLicenceID("west london", wlDTP.Text, licenceIDTxtBox.Text);
                    }
                    // driving licence
                    if (dlChckBox.Checked == true)
                    {
                        TrainingManagerRepository.AddDriverTrainingFromLicenceID("driving licence", dlDTP.Text, licenceIDTxtBox.Text);
                    }

                    // refresh dataset
                    OleDbConnection conn = DBConnectivity.GetConn();
                    OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Driver", conn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "Driver");
                    dataGridView1.DataSource = ds;
                    dataGridView1.DataMember = "Driver";
                    conn.Close();
                    //connLbl.Text = "new";
                    
                    errorLbl.Text = "New driver added successfully.";
                }
                else
                {
                    errorLbl.Text = "New driver NOT added.";
                }
            }
            
            
        }
        
        private void driverDeleteBtn_Click(object sender, EventArgs e)
        {
            
            // delete row/entry from grid and database
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            int columnindex = 0;
            DeleteDriver(int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
            /*
            DBConnectivity.DeleteDriverFromID(int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
            DBConnectivity.DeleteDriverTrainingFromID("all", int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));
            DBConnectivity.DeleteDriverQualificationsFromID("all", int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString()));

            // refresh dataset
            OleDbConnection conn = DBConnectivity.GetConn();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Driver", conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "Driver");
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "Driver";
            conn.Close();

            errorLbl.Text = "Driver removed successfully.";*/

        }

        private void editQualBtn_Click(object sender, EventArgs e)
        {
            // get ID of selected qualification
            int rowindex = dataGridView2.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            int columnindex = 0;
            int columnindex2 = 1;
            textBox8.Text = QualificationManagerRepository.LoadQualificationFromID(int.Parse(dataGridView2.Rows[rowindex].Cells[columnindex].Value.ToString()), dataGridView2.Rows[rowindex].Cells[columnindex2].Value.ToString(), "title");
            richTextBox5.Text = QualificationManagerRepository.LoadQualificationFromID(int.Parse(dataGridView2.Rows[rowindex].Cells[columnindex].Value.ToString()), dataGridView2.Rows[rowindex].Cells[columnindex2].Value.ToString(), "description");
            dateTimePicker11.Text = QualificationManagerRepository.LoadQualificationFromID(int.Parse(dataGridView2.Rows[rowindex].Cells[columnindex].Value.ToString()), dataGridView2.Rows[rowindex].Cells[columnindex2].Value.ToString(), "dueDate");
            

        }

        private void editTrainBtn_Click(object sender, EventArgs e)
        {
            // get ID of selected qualification
            int rowindex = dataGridView5.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            int columnindex = 0;
            int columnindex2 = 1;
            textBox7.Text = TrainingManagerRepository.LoadTrainingFromID(int.Parse(dataGridView5.Rows[rowindex].Cells[columnindex].Value.ToString()), dataGridView5.Rows[rowindex].Cells[columnindex2].Value.ToString(), "title");
            richTextBox1.Text = TrainingManagerRepository.LoadTrainingFromID(int.Parse(dataGridView5.Rows[rowindex].Cells[columnindex].Value.ToString()), dataGridView5.Rows[rowindex].Cells[columnindex2].Value.ToString(), "description");
            dateTimePicker20.Text = TrainingManagerRepository.LoadTrainingFromID(int.Parse(dataGridView5.Rows[rowindex].Cells[columnindex].Value.ToString()), dataGridView5.Rows[rowindex].Cells[columnindex2].Value.ToString(), "dueDate");
            textBox5.Text = TrainingManagerRepository.LoadTrainingFromID(int.Parse(dataGridView5.Rows[rowindex].Cells[columnindex].Value.ToString()), dataGridView5.Rows[rowindex].Cells[columnindex2].Value.ToString(), "time");
            // Completed?
            if (TrainingManagerRepository.LoadTrainingFromID(int.Parse(dataGridView5.Rows[rowindex].Cells[columnindex].Value.ToString()), dataGridView5.Rows[rowindex].Cells[columnindex2].Value.ToString(), "completed") == "Yes") 
            {
                checkBox11.Checked = true;
            }
            else
            {
                checkBox11.Checked = false;
            }
        }

        private void deleteQualBtn_Click(object sender, EventArgs e)
        {
            // delete row/entry from grid and database
            int rowindex = dataGridView2.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            int columnindex = 0;
            //DBConnectivity.DeleteQualificationFromID(int.Parse(dataGridView2.Rows[rowindex].Cells[columnindex].Value.ToString()));
            //DBConnectivity.DeleteQualificationFromID("all", int.Parse(dataGridView2.Rows[rowindex].Cells[columnindex].Value.ToString()));
            //DBConnectivity.DeleteDriverQualificationsFromID("all", int.Parse(dataGridView2.Rows[rowindex].Cells[columnindex].Value.ToString()));
            DeleteQualification(int.Parse(dataGridView2.Rows[rowindex].Cells[columnindex].Value.ToString()));
            // refresh dataset
            OleDbConnection conn = DBConnectivity.GetConn();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Qualification", conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "Qualification");
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "Qualification";
            conn.Close();

            errorLbl.Text = "Qualification removed successfully.";
        }

        private void deleteTrainBtn_Click(object sender, EventArgs e)
        {
            // delete row/entry from grid and database
            int rowindex = dataGridView5.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            int columnindex = 0;
            //DBConnectivity.DeleteTrainingFromID(int.Parse(dataGridView5.Rows[rowindex].Cells[columnindex].Value.ToString()));
            DeleteTraining(int.Parse(dataGridView5.Rows[rowindex].Cells[columnindex].Value.ToString()));
            // refresh dataset
            OleDbConnection conn = DBConnectivity.GetConn();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Training", conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "Training");
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "Training";
            conn.Close();

            errorLbl.Text = "Training removed successfully.";
        }

        private void addQualBtn_Click(object sender, EventArgs e)
        {
            // vaildation errors
            if (textBox8.Text == "" || richTextBox5.Text == "" || dateTimePicker11.Text == "")
            {
                label28.Text = "You must not leave fields blank.";
            }
            else
            {
                // if ID exists in qualification table, update data. otherwise create new entry
                // get ID of selected qualification
                int rowindex = dataGridView2.CurrentCell.RowIndex;
                //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
                int columnindex = 0;
                int columnindex2 = 2;
                // get dID of selected row and check if it and the title exists in qualification table already. if it does update, otherwise create new 
                if (QualificationManagerRepository.CheckDriverQualificationExistsFromdID(int.Parse(dataGridView2.Rows[rowindex].Cells[columnindex2].Value.ToString()), textBox8.Text) == "existing")
                {
                    //TODO
                    //connLbl.Text = "existing";
                    UpdateQualification();
                    //QualificationManagerRepository.UpdateQualificationFromID(int.Parse(dataGridView2.Rows[rowindex].Cells[columnindex].Value.ToString()), textBox8.Text, richTextBox5.Text, dateTimePicker11.Text);
                    
                    //label42.Text = "Qualification updated successfully.";
                }
                else if (QualificationManagerRepository.CheckDriverQualificationExistsFromdID(int.Parse(dataGridView2.Rows[rowindex].Cells[columnindex2].Value.ToString()), textBox8.Text) == "new")
                {
                    // connLbl.Text = "new";
                    //assign a driver by selecting them in the other gridview? apply dID --- assign btn. if driver id == training dID = error

                    //DBConnectivity.AddQualification(textBox8.Text, richTextBox5.Text, dateTimePicker11.Text);
                    SaveQualification();
                    //label42.Text = "New qualification added successfully.";
                }
            }
        }

        private void addTrainBtn_Click(object sender, EventArgs e)
        {
            // vaildation errors
            if (textBox7.Text == "" || richTextBox1.Text == "" || dateTimePicker20.Text == "" || textBox5.Text == "")
            {
                label42.Text = "You must not leave fields blank.";
            }
            else
            {
                // if ID exists in Training table, update data. otherwise create new entry
                // get ID of selected training
                int rowindex = dataGridView5.CurrentCell.RowIndex;
                //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
                //int columnindex = 0;
                int columnindex2 = 2;
                // get dID of selected row and check if it and the title exists in training table already. if it does update, otherwise create new 
                if (TrainingManagerRepository.CheckDriverTrainingExistsFromdID(int.Parse(dataGridView5.Rows[rowindex].Cells[columnindex2].Value.ToString()), textBox7.Text) == "existing")
                {
                    //TODO
                    //connLbl.Text = "existing";
                    UpdateTraining();
                   /* if (checkBox11.Checked == true)
                    {
                        TrainingManagerRepository.UpdateTrainingFromID(int.Parse(dataGridView5.Rows[rowindex].Cells[columnindex].Value.ToString()), textBox7.Text, richTextBox1.Text, dateTimePicker20.Text, textBox5.Text, "Yes");
                    }
                    else if (checkBox11.Checked == false)
                    {
                        TrainingManagerRepository.UpdateTrainingFromID(int.Parse(dataGridView5.Rows[rowindex].Cells[columnindex].Value.ToString()), textBox7.Text, richTextBox1.Text, dateTimePicker20.Text, textBox5.Text, "No");
                    }
                    label42.Text = "Training updated successfully.";*/
                }
                else if (TrainingManagerRepository.CheckDriverTrainingExistsFromdID(int.Parse(dataGridView5.Rows[rowindex].Cells[columnindex2].Value.ToString()), textBox7.Text) == "new")
                {
                    SaveTraining();
                   // connLbl.Text = "new";
                    // assign a driver by selecting them in the other gridview? apply dID --- assign btn. if driver id == training dID = error
                    /*if (checkBox11.Checked == true)
                    {
                        DBConnectivity.AddTraining(textBox7.Text, richTextBox1.Text, dateTimePicker20.Text, textBox5.Text, "Yes"); 
                    }
                    else if (checkBox11.Checked == false)
                    {
                        DBConnectivity.AddTraining(textBox7.Text, richTextBox1.Text, dateTimePicker20.Text, textBox5.Text, "No");
                    }
                    label42.Text = "New training added successfully.";*/
                }
            }
        }

        private void assignDriverToTrainingBtn_Click(object sender, EventArgs e)
        {
            // assign a driver by selecting them in the other gridview? apply dID --- assign btn. if driver id == training dID = error
            int rowindex = dataGridView4.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            int columnindex = 0; // driver id
            // title of training
            int rowindex2 = dataGridView5.CurrentCell.RowIndex;
            int columnindex2 = 1; 
            // if already exists in training table with same title
            if (TrainingManagerRepository.CheckDriverTrainingExistsFromdID(int.Parse(dataGridView5.Rows[rowindex2].Cells[columnindex2].Value.ToString()), dataGridView5.Rows[rowindex2].Cells[columnindex2].Value.ToString()) == dataGridView4.Rows[rowindex].Cells[columnindex].Value.ToString()) // get driver id and title of training
            {
                label42.Text = "Driver already assigned to this training!";
            }
            else
            {
                TrainingManagerRepository.AssignDriverToTrainingFromID(int.Parse(dataGridView5.Rows[rowindex2].Cells[columnindex2].Value.ToString()), dataGridView5.Rows[rowindex2].Cells[columnindex2].Value.ToString(), int.Parse(dataGridView4.Rows[rowindex].Cells[columnindex].Value.ToString()));
                label42.Text = "Driver assigned.";
            }
        }

        private void assignDriverToQualificationBtn_Click(object sender, EventArgs e)
        {
            // assign a driver by selecting them in the other gridview? apply dID --- assign btn. if driver id == training dID = error
            int rowindex = dataGridView3.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            int columnindex = 0; // driver id
            // title of training
            int rowindex2 = dataGridView2.CurrentCell.RowIndex;
            int columnindex2 = 1;
            // if already exists in training table with same title
            if (QualificationManagerRepository.CheckDriverQualificationExistsFromdID(int.Parse(dataGridView2.Rows[rowindex2].Cells[columnindex2].Value.ToString()), dataGridView2.Rows[rowindex2].Cells[columnindex2].Value.ToString()) == dataGridView3.Rows[rowindex].Cells[columnindex].Value.ToString()) // get driver id and title of training
            {
                label42.Text = "Driver already assigned to this training!";
            }
            else
            {
                QualificationManagerRepository.AssignDriverToQualificationFromID(int.Parse(dataGridView2.Rows[rowindex2].Cells[columnindex2].Value.ToString()), dataGridView2.Rows[rowindex2].Cells[columnindex2].Value.ToString(), int.Parse(dataGridView3.Rows[rowindex].Cells[columnindex].Value.ToString()));
                label42.Text = "Driver assigned.";
            }
        }

        private void searchPageBtn_Click(object sender, EventArgs e)
        {
            // split string in search text box
            string[] tmp = searchTxtBox.Text.Split(' ');
            fN = tmp[0];
            sN = tmp[1]; // name needs to have two words separated by a space. not ideal but...
            // find if name exists first and if there are any duplicates
            if (DriverManagerRepository.CheckDriverExistsFromName(fN, sN) == 1)
            {
                label43.Text = "";
                // one driver exists
                // get driver ID
                driverID = DriverManagerRepository.GetDriverIDFromName(fN, sN);
                // probably doesn't work... commenting out for now... but it's there...
                //GetDriverIDWithName();
                // show driver information
                // qualifications
                List<String> q = new List<String>();
                q = QualificationManagerRepository.FindQualifcationsForDriver(driverID);
                richTextBox2.Lines = q.ToArray();
                // training
                List<String> t = new List<String>();
                t = TrainingManagerRepository.FindTrainingForDriver(driverID);
                richTextBox3.Lines = t.ToArray();

                // personal info
               List<Driver> driver = DriverManagerRepository.LoadDriver(driverID);
              // nameLbl.Text = driverID.ToString();
                
                foreach (var d in driver)
                {
                    nameLbl.Text = d.FirstName + " " + d.Surname;
                    phoneNumberLbl.Text = d.PhoneNumber.ToString();
                    licenceIDLbl.Text = d.LicenceID;
                    startedEmploymentLbl.Text = d.EmploymentDate;
                    richTextBox4.Text = d.Address;
                }
                
            }
            else if (DriverManagerRepository.CheckDriverExistsFromName(fN, sN) > 1)
            {
                label43.Text = "";
                // multiple drivers exist with same name
                // popup panel with clickable links to driver they want to view.
                panel17.Visible = true;
                //populate dataGridView6
                // get multiple names
                List<Driver> dm = DriverManagerRepository.GetDuplicateDriverNames(fN, sN);
                int i = 1;
                foreach (var d in dm)
                {
                    //connLbl.Text = d.DriverName;
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView6); 
                    //row.Cells[0].Value = i.ToString();
                    row.Cells[0].Value = d.ID;
                    row.Cells[1].Value = d.DriverName;

                    dataGridView6.Rows.Add(row);
                    i++;
                }              
            }
            else
            {
                // driver doesn't exist
                // error msg
                label43.Text = "Driver doesn't exist";
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView6.Rows.Clear();
            dataGridView6.Refresh();
            panel17.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // grab selected row from table and display in search page
            // get ID of selected qualification
            int rowindex = dataGridView6.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            int columnindex = 0;
            // show driver information
            // qualifications
            driverID = int.Parse(dataGridView6.Rows[rowindex].Cells[columnindex].Value.ToString());
            List<String> q = new List<String>();
            q = QualificationManagerRepository.FindQualifcationsForDriver(driverID);
            richTextBox2.Lines = q.ToArray();
            // training
            List<String> t = new List<String>();
            t = TrainingManagerRepository.FindTrainingForDriver(driverID);
            richTextBox3.Lines = t.ToArray();

            // personal info
            List<Driver> driver = DriverManagerRepository.LoadDriver(driverID);
            // nameLbl.Text = driverID.ToString();

            foreach (var d in driver)
            {
                nameLbl.Text = d.FirstName + " " + d.Surname;
                phoneNumberLbl.Text = d.PhoneNumber.ToString();
                licenceIDLbl.Text = d.LicenceID;
                startedEmploymentLbl.Text = d.EmploymentDate;
                richTextBox4.Text = d.Address;
            }
            dataGridView6.Rows.Clear();
            dataGridView6.Refresh();
            panel17.Visible = false;
        }

        private void dataGridView7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // get ID of selected driver
            int rowindex = dataGridView7.CurrentCell.RowIndex;
            int columnindex = 0;
            int dID = int.Parse(dataGridView7.Rows[rowindex].Cells[columnindex].Value.ToString());
            // display selected driver's disciplinary records in the list box. title only.

            var conn = DBConnectivity.GetConn();
            using (conn)
            {
                conn.Open();

                // use a SqlAdapter to execute the query
                using (OleDbDataAdapter a = new OleDbDataAdapter("SELECT * FROM DisciplinaryRecord WHERE dID = " + dID, conn))
                {
                    // fill a data table
                    var t = new DataTable();
                    a.Fill(t);

                    // Bind the table to the list box
                    listBox7.DisplayMember = "title";
                    listBox7.ValueMember = "ID";
                    listBox7.DataSource = t;
                }
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            // disciplinary records edit
            // get ID of selected qualification
            int rowindex = dataGridView7.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            //int columnindex = 0;
            //int columnindex2 = 1;
            textBox10.Text = DisciplinaryRecordManagerRepository.LoadDisciplinaryRecordFromID(int.Parse(listBox7.SelectedValue.ToString()), "title");
            richTextBox8.Text = DisciplinaryRecordManagerRepository.LoadDisciplinaryRecordFromID(int.Parse(listBox7.SelectedValue.ToString()), "description");
            dateTimePicker13.Text = DisciplinaryRecordManagerRepository.LoadDisciplinaryRecordFromID(int.Parse(listBox7.SelectedValue.ToString()), "date");
            textBox11.Text = DisciplinaryRecordManagerRepository.LoadDisciplinaryRecordFromID(int.Parse(listBox7.SelectedValue.ToString()), "category");

        }
        private void button14_Click(object sender, EventArgs e)
        {
            // delete disciplinary record
           // DriverManagerRepository.DeleteDisciplinaryRecordFromID(int.Parse(listBox7.SelectedValue.ToString()));
            DeleteDisciplinaryRecord(int.Parse(listBox7.SelectedValue.ToString()));
        }
        private void button15_Click(object sender, EventArgs e)
        {
            // add / update discplinary record to database
            // vaildation errors
            if (textBox10.Text == "" || richTextBox8.Text == "" || dateTimePicker13.Text == "" || textBox11.Text == "")
            {
                label28.Text = "You must not leave fields blank.";
            }
            else
            {
                // get id of disciplinary record
                int id = int.Parse(listBox7.SelectedValue.ToString());
                // test all textboxes against database to determine if record needs to update or create new.
                if (DisciplinaryRecordManagerRepository.CheckDriverDisciplinaryRecordExistsFromdAllColumns(id, textBox10.Text, richTextBox8.Text, dateTimePicker13.Text, textBox11.Text) == "existing")
                {
                    UpdateDisciplinaryRecord();
                }
                else if (DisciplinaryRecordManagerRepository.CheckDriverDisciplinaryRecordExistsFromdAllColumns(id, textBox10.Text, richTextBox8.Text, dateTimePicker13.Text, textBox11.Text) == "new")
                {
                    SaveDisciplinaryRecord();
                }
            }

        }

        #region web service methods
        public async Task<String> SaveDriver()
        {
            Driver driver = new Driver();
            driver.FirstName = firstNameTxtBox.Text;
            driver.Surname = surnameTxtBox.Text;
            driver.Sex = sexCB.Text;
            driver.PhoneNumber = long.Parse(phoneNumberTxtBox.Text);
            driver.EmploymentDate = employmentDateDTP.Text;
            driver.LicenceID = licenceIDTxtBox.Text;
            driver.Address = addressTxtBox.Text;
            driver.DriverName = firstNameTxtBox.Text + " " + surnameTxtBox.Text;

            var success = await DriverManagerProcessor.ProcessDriver(driver);

            if (success)
            {
                errorLbl.Text = "New driver added successfully.";
            }
            else
            {
                errorLbl.Text = "nope";
            }
            return "done";
        }
        public async Task<String> SaveTraining()
        {
            Training training = new Training();
            training.Title = textBox7.Text;
            training.Description = richTextBox1.Text;
            training.Time = textBox5.Text;
            training.ExpiryDate = dateTimePicker20.Text;

            if (checkBox11.Checked)
            {
                training.Completed = "Yes";
            }
            else if (checkBox11.Checked == false)
            {
                training.Completed = "No";
            }

            var success = await TrainingManagerProcessor.ProcessTraining(training);

            if (success)
            {
                errorLbl.Text = "New training added successfully.";
            }
            else
            {
                errorLbl.Text = "nope";
            }
            return "done";
        }
        public async Task<String> SaveQualification()
        {
            Qualification qualification = new Qualification();
            qualification.Title = textBox8.Text;
            qualification.Description = richTextBox5.Text;
            qualification.ExpiryDate = dateTimePicker11.Text;

            var success = await QualificationManagerProcessor.ProcessQualification(qualification);

            if (success)
            {
                errorLbl.Text = "New qualification added successfully.";
            }
            else
            {
                errorLbl.Text = "nope";
            }
            return "done";
        }
        public async Task<String> SaveDisciplinaryRecord()
        {
            DisciplinaryRecord disciplinaryRecord = new DisciplinaryRecord();
            disciplinaryRecord.Title = textBox10.Text;
            disciplinaryRecord.Description = richTextBox8.Text;
            disciplinaryRecord.Category = richTextBox5.Text;
            disciplinaryRecord.DateAdded = textBox11.Text;

            var success = await DisciplinaryRecordManagerProcessor.ProcessDisciplinaryRecord(disciplinaryRecord);

            if (success)
            {
                errorLbl.Text = "New disciplinary record added successfully.";
            }
            else
            {
                errorLbl.Text = "nope";
            }
            return "done";
        }
        public async Task<String> SaveTrainingFromDriverLicenceID()
        {
            Driver driver = new Driver();
            Training training = new Training();
            driver.FirstName = firstNameTxtBox.Text;
            driver.Surname = surnameTxtBox.Text;
            driver.Sex = sexCB.Text;
            driver.PhoneNumber = long.Parse(phoneNumberTxtBox.Text);
            driver.EmploymentDate = employmentDateDTP.Text;
            driver.LicenceID = licenceIDTxtBox.Text;
            driver.Address = addressTxtBox.Text;
            driver.DriverName = firstNameTxtBox.Text + " " + surnameTxtBox.Text;


            // advanced driving course
            if (adcChckBox.Checked == true)
            {
                // DBConnectivity.AddDriverTrainingFromLicenceID("advanced driving course", adcDTP.Text, licenceIDTxtBox.Text);
                training.Type = "advanced driving course";
                training.Title = "advanced driving course";
                training.ExpiryDate = adcDTP.Text;

            }
            // driving at night
            if (danChckBox.Checked == true)
            {
                //DBConnectivity.AddDriverTrainingFromLicenceID("driving at night", danDTP.Text, licenceIDTxtBox.Text);
                training.Type = "driving at night";
            }
            // cyclist awareness
            if (caChckBox.Checked == true)
            {
                //DBConnectivity.AddDriverTrainingFromLicenceID("cyclist awareness", caDTP.Text, licenceIDTxtBox.Text);
                training.Type = "cyclist awareness";
            }
            // reduce fuel consumption
            if (rfcChckBox.Checked == true)
            {
                //DBConnectivity.AddDriverTrainingFromLicenceID("reduce fuel consumption", rfcDTP.Text, licenceIDTxtBox.Text);
                training.Type = "reduce fuel consumption";
            }


            var success = await TrainingManagerProcessor.ProcessTrainingFromDriverLicenceID(training, driver);

            if (success)
            {
                errorLbl.Text = "New driver added successfully.";
            }
            else
            {
                connLbl.Text = "nope";
            }
            return "done";
        }
        public async Task<String> DeleteDriver(int id)
        {
            // delete row/entry from grid and database
            //int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            //int columnindex = 0;
            //id = int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString());

            //var success = await DriverManagerProcessor.ProcessDriver(driver);
            var success = await DriverManagerProcessor.ProcessDeleteDriver(id);

            if (success)
            {
                errorLbl.Text = "Driver deleted successfully.";

                OleDbConnection conn = DBConnectivity.GetConn();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Driver", conn);
                DataSet ds = new DataSet();
                da.Fill(ds, "Driver");
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "Driver";
                conn.Close();
            }
            else
            {
                connLbl.Text = "nope";
            }
            return "done";
        }
        public async Task<String> DeleteTraining(int id)
        {
            // delete row/entry from grid and database
            //int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            //int columnindex = 0;
            //id = int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString());

            //var success = await DriverManagerProcessor.ProcessDriver(driver);
            var success = await TrainingManagerProcessor.ProcessDeleteTraining(id);

            if (success)
            {
                errorLbl.Text = "Training deleted successfully.";

                OleDbConnection conn = DBConnectivity.GetConn();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Training", conn);
                DataSet ds = new DataSet();
                da.Fill(ds, "Training");
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "Training";
                conn.Close();
            }
            else
            {
                connLbl.Text = "nope";
            }
            return "done";
        }
        public async Task<String> DeleteQualification(int id)
        {
            // delete row/entry from grid and database
            //int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            //int columnindex = 0;
            //id = int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString());

            //var success = await DriverManagerProcessor.ProcessDriver(driver);
            var success = await QualificationManagerProcessor.ProcessDeleteQualification(id);

            if (success)
            {
                errorLbl.Text = "Qualification deleted successfully.";

                OleDbConnection conn = DBConnectivity.GetConn();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Qualification", conn);
                DataSet ds = new DataSet();
                da.Fill(ds, "Qualification");
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "Qualification";
                conn.Close();
            }
            else
            {
                connLbl.Text = "nope";
            }
            return "done";
        }
        public async Task<String> DeleteDisciplinaryRecord(int id)
        {
            // delete row/entry from grid and database
            //int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            //int columnindex = 0;
            //id = int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString());

            //var success = await DriverManagerProcessor.ProcessDriver(driver);
            var success = await DisciplinaryRecordManagerProcessor.ProcessDeleteDisciplinaryRecord(id);

            if (success)
            {
                label45.Text = "Disciplinary Record deleted successfully.";

                /*OleDbConnection conn = DBConnectivity.GetConn();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM DisciplinaryRecord", conn);
                DataSet ds = new DataSet();
                da.Fill(ds, "DisciplinaryRecord");
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "Qualification";
                conn.Close();*/
            }
            else
            {
                connLbl.Text = "nope";
            }
            return "done";
        }
        public async Task<String> UpdateDriver()
        {
            // if dID exists in Driver table, update data. otherwise create new entry
            // get ID of selected driver
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            int columnindex = 0;
            Driver driver = new Driver();
            driver.ID = int.Parse(dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString());
            driver.FirstName = firstNameTxtBox.Text;
            driver.Surname = surnameTxtBox.Text;
            driver.Sex = sexCB.Text;
            driver.PhoneNumber = long.Parse(phoneNumberTxtBox.Text);
            driver.EmploymentDate = employmentDateDTP.Text;
            driver.LicenceID = licenceIDTxtBox.Text;
            driver.Address = addressTxtBox.Text;
            driver.DriverName = firstNameTxtBox.Text + " " + surnameTxtBox.Text;

            var success = await DriverManagerProcessor.ProcessUpdateDriver(driver);

            if (success)
            {
                errorLbl.Text = "Driver updated successfully.";
            }
            else
            {
                errorLbl.Text = "nope";
            }
            return "done";
        }
        public async Task<String> UpdateTraining()
        {
            // if ID exists in Training table, update data. otherwise create new entry
            // get ID of selected training
            int rowindex = dataGridView5.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            int columnindex = 0;
            //int columnindex2 = 2;
            Training training = new Training();
            training.ID = int.Parse(dataGridView5.Rows[rowindex].Cells[columnindex].Value.ToString());
            training.Title = textBox7.Text;
            training.Description = richTextBox1.Text;
            training.DueDate = dateTimePicker20.Text;
            training.Time = textBox5.Text;
            if (checkBox11.Checked == true)
            {
                training.Completed = "Yes";
            }
            else if (checkBox11.Checked == false)
            {
                training.Completed = "No";
            }
            var success = await TrainingManagerProcessor.ProcessUpdateTraining(training);

            if (success)
            {
                errorLbl.Text = "Training updated successfully.";
            }
            else
            {
                errorLbl.Text = "nope";
            }
            return "done";
        }
        public async Task<String> UpdateQualification()
        {
            // if ID exists in qualification table, update data. otherwise create new entry
            // get ID of selected qualification
            int rowindex = dataGridView2.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            int columnindex = 0;
            //int columnindex2 = 2;
            Qualification qualification = new Qualification();
            //QualificationManagerRepository.UpdateQualificationFromID(int.Parse(dataGridView2.Rows[rowindex].Cells[columnindex].Value.ToString()), textBox8.Text, richTextBox5.Text, dateTimePicker11.Text);
            qualification.ID = int.Parse(dataGridView2.Rows[rowindex].Cells[columnindex].Value.ToString());
            qualification.Title = textBox8.Text;
            qualification.Description = richTextBox5.Text;
            qualification.DueDate = dateTimePicker11.Text;

            var success = await QualificationManagerProcessor.ProcessUpdateQualification(qualification);

            if (success)
            {
                errorLbl.Text = "Qualification updated successfully.";
            }
            else
            {
                errorLbl.Text = "nope";
            }
            return "done";
        }
        public async Task<String> UpdateDisciplinaryRecord()
        {
            // if ID exists in qualification table, update data. otherwise create new entry
            // get ID of selected qualification
            //int rowindex = dataGridView2.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            //int columnindex = 0;
            //int columnindex2 = 2;
            DisciplinaryRecord disciplinaryRecord = new DisciplinaryRecord();
            //QualificationManagerRepository.UpdateQualificationFromID(int.Parse(dataGridView2.Rows[rowindex].Cells[columnindex].Value.ToString()), textBox8.Text, richTextBox5.Text, dateTimePicker11.Text);
            disciplinaryRecord.ID = int.Parse(listBox7.SelectedValue.ToString());
            disciplinaryRecord.Title = textBox10.Text;
            disciplinaryRecord.Description = richTextBox8.Text;
            disciplinaryRecord.Title = textBox11.Text;
            disciplinaryRecord.DateAdded = dateTimePicker13.Text;

            var success = await DisciplinaryRecordManagerProcessor.ProcessUpdateDisciplinaryRecord(disciplinaryRecord);

            if (success)
            {
                errorLbl.Text = "Disciplinary record updated successfully.";
            }
            else
            {
                errorLbl.Text = "nope";
            }
            return "done";
        }

        private void logDataBtn_Click(object sender, EventArgs e)
        {
            // show employee tab
            menuDataTabControl.SelectTab(logDataTab);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            GetCalcOfLogData();

            

        }
        public async Task<string> GetCalcOfLogData()
        {
            // get input values from log data tab
            string startOfJourney = textBox3.Text;
            string endOfJourney = textBox2.Text;
            string startOfDay = textBox4.Text;
            string endOfDay = textBox1.Text;

            // validation - needs to be in xx:xx format

            // store variables in log data object
            LogData logData = new LogData();

            logData.StartOfJourney = startOfJourney;
            logData.EndOfJourney = endOfJourney;
            logData.StartOfDay = startOfDay;
            logData.EndOfDay = endOfDay;
            
            // contact service
            dynamic data = await DriverManagerRepository.CalcLogData(logData);

            // return values to labels
            //dynamic data = DriverManagerRepository.CalcLogData(logData);
            label54.Text = data.LengthOfJourney.ToString();
            label55.Text = data.LengthOfDay.ToString();
            label56.Text = data.AmountOfHoursWorked.ToString();
            label57.Text = data.TimeOfLogCalculation.ToString();

            return "done";

        }
        public async Task<String> GetDriverIDWithName()
        {
            // split string in search text box
            string[] tmp = searchTxtBox.Text.Split(' ');
            fN = tmp[0];
            sN = tmp[1];

            Driver driver = new Driver();
            driver.FirstName = fN;
            driver.Surname = sN;

            var success = await DriverManagerProcessor.ProcessGetDriverIDWithName(driver);

            if (success > 0)
            {
                driverID = success;
            }
            else
            {
                errorLbl.Text = "nope";
            }
            return "done";
        }
        #endregion
    }
}
