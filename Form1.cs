using InputSimulatorStandard;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;


/*Hello thanks for looking at my code
 * this is my first time ever attempting to program software.
 *  so if you're looking at this code to learn from me, keep in mind I might not
 *  be the best reference for good code. 
 *  Anyways, "itsdun" is my youtube channel 
 *  enjoy -dun   */







namespace DunsAutoClicker
{
    public partial class Nothing : Form
    {


        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);


        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);


        public bool AppOn = false;  //toggling the clicking
        bool Settingkey; // check if currently setting a key
        IntPtr thiswindow; // reference to the main window
        Keys newkey;// the new keybind set by user
        Keys modifierkey; // reference to modifer key (shift,alt,control)
        bool doubleclick; // toggle double click
        bool movingoverlay; // toggle overlay move made

        Form Overlay_ = new Form();  // create a reference to the overlay form
        Label overlaytext_ = new Label(); // reference to text on overlay


        //refernece to screen size
        int ScreenHeight_ = Screen.PrimaryScreen.Bounds.Width & Screen.PrimaryScreen.Bounds.Height;
        int ScreenWidth_ = Screen.PrimaryScreen.Bounds.Width & Screen.PrimaryScreen.Bounds.Width;


        // GUI color change speed
        int ColorSpeed = 1200;


        //setting a default delay
        public int ClickDelay = 10;



        // Starting code
        public Nothing()
        {
            InitializeComponent();
            label3.Text = "Tab"; // set the keybind text
            textBox1.Text = ClickDelay.ToString(); // show the current clicks per second

            //Setting default check box states
            checkBox1.Checked = true;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = true;
            checkBox5.Checked = false;


            //  GUI style config
            StartButton.FlatStyle = FlatStyle.Flat;
            StartButton.FlatAppearance.BorderSize = 0;

            movebutton.FlatStyle = FlatStyle.Flat;
            movebutton.FlatAppearance.BorderSize = 0;

            textBox1.TabStop = false;
            textBox1.BorderStyle = BorderStyle.None;

            bindbutton.FlatAppearance.BorderSize = 0;
            bindbutton.FlatStyle = FlatStyle.Flat;

            //disabling moving text
            label4.Hide();
            label5.Hide();
            label6.Hide();


            delaychangecolor();//start changing colors

            CreateOverlay();// create the overlay window
        }


        //create the overlay
        public void CreateOverlay() {

           
            Overlay_.StartPosition = FormStartPosition.Manual;
            Overlay_.Opacity = .70f;
            Overlay_.Name = "overlay";
            Overlay_.BackColor = Color.Black;
            Overlay_.TopMost = true;
            Overlay_.ShowInTaskbar = false;
            Overlay_.ControlBox = false;
            Overlay_.FormBorderStyle = FormBorderStyle.FixedSingle;
            Overlay_.Size = new System.Drawing.Size(200, 50);
            Overlay_.Location = new Point(Screen.PrimaryScreen.Bounds.Right - Overlay_.Width, 0);
         


            //creating text
            Overlay_.Controls.Add(overlaytext_);//add text to the overlay
            overlaytext_.Text = "Not Clicking";
            overlaytext_.AutoSize = true;
            overlaytext_.Location = new Point(25, 0);
            overlaytext_.Font = new Font("Calibri", 18);
            overlaytext_.ForeColor = Color.White;
           
            overlaytext_.Padding = new Padding(6);
            overlaytext_.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
           

            // showing the overlay at start
            overlaytext_.Show();
            Overlay_.Show();



         

        }

       

        // on load 
        private void Nothing_Load(object sender, EventArgs e)
        {
            PressIt(); // call the code to click 
            thiswindow = FindWindow(null, "Dun's AutoClicker"); // finding the main window
            RegisterHotKey(thiswindow, 1, 0, (uint)Keys.Tab); // register starting keybind
        }

        //  Clicking 
        public async void PressIt()
        {

            var sim = new InputSimulator(); // define simulator

            // do click if app is on (if clicking is toggled on)
            if (AppOn == true)
            {
                //left click
                if (checkBox1.Checked == true)
                {

                    if (doubleclick == true)
                    {
                        sim.Mouse.LeftButtonDoubleClick();
                    }
                    else
                    {
                        sim.Mouse.LeftButtonClick();
                    }

                }

                //rightclick
                if (checkBox2.Checked == true)
                {

                    if (doubleclick == true)
                    {
                        sim.Mouse.RightButtonDoubleClick();
                    }
                    else
                    {
                        sim.Mouse.RightButtonClick();
                    }
                }

                //MiddleClick
                if (checkBox3.Checked == true)
                {
                    if (doubleclick == true)
                    {
                        sim.Mouse.MiddleButtonDoubleClick();
                    }
                    else
                    {
                        sim.Mouse.MiddleButtonClick();
                    }
                 
                }

                await Task.Delay(1000 / ClickDelay); // wait time

                PressIt();
            }
        }

        //button to start
        private void StartButton_Click(object sender, EventArgs e)
        {
            ToggleApp();
            enableStartButton();
        }


        //wait some time before enabling the start button again (this makes it so the auto clicker doesn't immediately click stop)
        private async void enableStartButton() {
            StartButton.Enabled = false;
            await Task.Delay(1000);
            StartButton.Enabled = true;
        }


        // turn app off/on
        void ToggleApp()
        {

            if (Settingkey == false)  // dont enable the autoclicker if currenting setting a keybind
            {
                if (AppOn == true)
                {
                    StartButton.Text = "Start";
                    AppOn = false;
                    overlaytext_.Text = "Not Clicking";
                    overlaytext_.ForeColor = Color.White;
                }
                else
                {
                    AppOn = true;
                    StartButton.Text = "Stop";
                    overlaytext_.Text = "Clicking";
                    overlaytext_.ForeColor = Color.Red;
                    PressIt();
                }
            }
        }

        //changing delay from text field 
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //set click delay to what in in the text box
            bool canconvert;
            int num = 0;
            canconvert = int.TryParse(textBox1.Text, out num); // checking if what the user entered is a int


            if (canconvert == true)
            {
                ClickDelay = int.Parse(textBox1.Text);
            }
        }

        //detecting keybind 
        protected override void WndProc(ref Message keyPressed)
        {

            // the hotkey has been pressed

            if (keyPressed.Msg == 0x0312)
            {


                if (Settingkey == true) { // if ur setting key and press the current key,  just keep it
                    Settingkey = false;

                    ToggleGUI(true);
                }

                ToggleApp();
            }
            base.WndProc(ref keyPressed);
        }

        //  button to activate keybind set 
        private void bindbutton_Click(object sender, EventArgs e)
        {

            Settingkey = true; // user is now setting a keybind
            bindbutton.Text = "Press a key..";



            //disabling gui
            ToggleGUI(false);

        }

        //   Detecting key presses \\
        private void Nothing_KeyDown(object sender, KeyEventArgs e)
        {



            // if you pressed a key ,  and you're currently in keybind set mode
            if (Settingkey == true)
            {


                //checking for modifiers
                if (e.KeyCode.ToString() == "ShiftKey" || e.KeyCode.ToString() == "ControlKey" || e.KeyCode.ToString() == "Alt") {
                    return;
                }

                //setting an int to reference each modifier key
                int mod = 0;
                if (e.Modifiers == Keys.Alt) mod |= 1;
                if (e.Modifiers == Keys.Control) mod |= 2;
                if (e.Modifiers == Keys.Shift) mod |= 4;




           
                UnregisterHotKey(thiswindow, 1);//delete old hotkey

                //convert string name , to key



                     //setting the key pressed as "newkey" reference
                    newkey = (Keys)Enum.Parse(typeof(Keys), e.KeyCode.ToString(), true);
                   //setting the modifier 
                    modifierkey = (Keys)Enum.Parse(typeof(Keys), e.Modifiers.ToString(), true);

                if (e.Modifiers.ToString() != "None") // if there is a modifier
                {
                    RegisterHotKey(thiswindow, 1, (uint)mod, (uint)newkey);//add modifier to key
                    label3.Text = e.Modifiers.ToString() + " + " + e.KeyCode.ToString();//change text
                }
                else { // if no modifier
                    RegisterHotKey(thiswindow, 1, 0, (uint)newkey);//register the new key
                    label3.Text = e.KeyCode.ToString();//change text
                }








                // enable all the GUI after the new key is set
                ToggleGUI(true);



                Settingkey = false; //disable setting key mode
                bindbutton.Text = "Hotkey";
            }






            // moving the overlay with arrow keys if you're currently in move made
            if (movingoverlay == true)
            {
                if (e.KeyCode == Keys.Left)
                {
                    Overlay_.Location = new Point(Overlay_.Location.X - 10, Overlay_.Location.Y);

                }

                if (e.KeyCode == Keys.Right)
                {
                    Overlay_.Location = new Point(Overlay_.Location.X + 10, Overlay_.Location.Y);

                }

                if (e.KeyCode == Keys.Up)
                {
                    Overlay_.Location = new Point(Overlay_.Location.X, Overlay_.Location.Y - 10);

                }

                if (e.KeyCode == Keys.Down)
                {
                    Overlay_.Location = new Point(Overlay_.Location.X, Overlay_.Location.Y + 10);

                }


            }

            //disabling move mode
            if (e.KeyCode == Keys.Escape && movingoverlay == true) {
                movingoverlay = false;
                movebutton.Text = "Move";
                label4.Hide();
                label5.Hide();
                label6.Hide();

                ToggleGUI(true);
            }


     

        }

        //  handling check boxes
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox3.Checked = false;
                checkBox1.Checked = false;
            }
        }

        // toggling overlay with check box
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                Overlay_.Show();
            }
            else {
                Overlay_.Hide();
            }
        }





        //delaying color change
        private async void delaychangecolor() {

            Color lightblue_ = Color.FromArgb(179,175,255);
            Color DarkerBlue = Color.FromArgb(109, 102, 255);
            Color pinkish = Color.FromArgb(209, 112, 255);



            ChangeColor(lightblue_);
            await Task.Delay(ColorSpeed);
            ChangeColor(DarkerBlue);
            await Task.Delay(ColorSpeed);
            ChangeColor(pinkish);
            await Task.Delay(ColorSpeed);
            delaychangecolor();
        }

        //Changing Color 
        public void ChangeColor(Color newcolor) {

            checkBox1.ForeColor = newcolor;
            checkBox2.ForeColor = newcolor;
            checkBox3.ForeColor = newcolor;
            checkBox4.ForeColor = newcolor;
            checkBox5.ForeColor = newcolor;
            label1.ForeColor = newcolor;

            textBox1.BackColor = newcolor;


            StartButton.BackColor = newcolor;
            movebutton.BackColor = newcolor;
            bindbutton.BackColor = newcolor;
            textBox1.BackColor = newcolor;

        }



        //double click box
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                doubleclick = true;

            }
            else {
                doubleclick = false;
            }
            
        }
       
        
        //toggling move mode
        private void movebutton_Click(object sender, EventArgs e)
        {
            if (movingoverlay == false)
            {
                movingoverlay = true;
                
                movebutton.Text = "Moving...";
            

                label4.Show();
                label5.Show();
                label6.Show();

            }
            else {
                movingoverlay = false;
                movebutton.Text = "Move";
                label4.Hide();
                label5.Hide();
                label6.Hide();
            }
        }



        //disable and enable the gui
        private void ToggleGUI(bool state_) {
            StartButton.Enabled = state_;
            bindbutton.Enabled = state_;
            checkBox1.Enabled = state_;
            checkBox2.Enabled = state_;
            checkBox3.Enabled = state_;
            checkBox4.Enabled = state_;
            checkBox5.Enabled = state_;
            textBox1.Enabled = state_;
        }



    }
}
